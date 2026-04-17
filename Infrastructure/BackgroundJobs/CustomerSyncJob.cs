using Hangfire;
using Infrastructure.BackgroundJobs.Interfaces;
using crm_api.Data;
using crm_api.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.Common;

namespace Infrastructure.BackgroundJobs
{
    [DisableConcurrentExecution(timeoutInSeconds: 300)]
    [AutomaticRetry(Attempts = 3, DelaysInSeconds = new[] { 60, 120, 300 })]
    public class CustomerSyncJob : ICustomerSyncJob
    {
        private const string RecurringJobId = "erp-customer-sync-job";
        private readonly IUnitOfWork _unitOfWork;
        private readonly IErpService _erpService;
        private readonly CmsDbContext _db;
        private readonly ILocalizationService _localizationService;
        private readonly ILogger<CustomerSyncJob> _logger;

        public CustomerSyncJob(
            IUnitOfWork unitOfWork,
            IErpService erpService,
            CmsDbContext db,
            ILocalizationService localizationService,
            ILogger<CustomerSyncJob> logger)
        {
            _unitOfWork = unitOfWork;
            _erpService = erpService;
            _db = db;
            _localizationService = localizationService;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation(_localizationService.GetLocalizedString("CustomerSyncJob.Started"));

            var erpResponse = await _erpService.GetCarisAsync(null);
            if (erpResponse == null || !erpResponse.Success)
            {
                var message = erpResponse?.ExceptionMessage ?? erpResponse?.Message ?? _localizationService.GetLocalizedString("CustomerSyncJob.ErpFetchFailed");
                var ex = new InvalidOperationException(message);
                await LogRecordFailureAsync("ERP_FETCH", ex);
                _logger.LogWarning("Customer sync aborted: ERP fetch failed. Message: {Message}", message);
                return;
            }

            if (erpResponse?.Data == null || erpResponse.Data.Count == 0)
            {
                var message = "Customer sync returned zero ERP records for a full sync request.";
                _logger.LogWarning(message);
                throw new InvalidOperationException(message);
            }

            _logger.LogInformation("Customer sync fetched {Count} ERP records from ERP.", erpResponse.Data.Count);

            var customerColumns = await GetCustomerColumnNamesAsync();
            var salesRepCodeColumnName = ResolveColumnName(customerColumns, "SalesRepCode", "SalesRepcode");
            var groupCodeColumnName = ResolveColumnName(customerColumns, "GroupCode");
            var supportsSalesRepCode = salesRepCodeColumnName != null;
            var supportsGroupCode = groupCodeColumnName != null;
            var existingCustomers = await LoadExistingCustomersAsync(salesRepCodeColumnName, groupCodeColumnName);

            if (!supportsSalesRepCode || !supportsGroupCode)
            {
                _logger.LogWarning(
                    "Customer sync is running in compatibility mode for RII_CUSTOMER. Supports SalesRepCode: {SupportsSalesRepCode}, Supports GroupCode: {SupportsGroupCode}",
                    supportsSalesRepCode,
                    supportsGroupCode);
            }

            var createdCount = 0;
            var updatedCount = 0;
            var reactivatedCount = 0;
            var skippedCount = 0;
            var failedCount = 0;
            var failedCodes = new List<string>();

            foreach (var erpCustomer in erpResponse.Data)
            {
                var code = erpCustomer.CariKod?.Trim() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(code))
                {
                    skippedCount++;
                    continue;
                }

                try
                {
                    var customer = existingCustomers.TryGetValue(code, out var existingCustomer)
                        ? existingCustomer
                        : null;

                    var name = PreserveRequired(erpCustomer.CariIsim, code, 200);
                    var taxOffice = PreserveOptional(erpCustomer.VergiDairesi, 100);
                    var taxNumber = PreserveOptional(erpCustomer.VergiNumarasi, 50);
                    var tcknNumber = PreserveOptional(erpCustomer.TcknNumber, 11);
                    var email = PreserveOptional(erpCustomer.Email, 100);
                    var website = PreserveOptional(erpCustomer.Web, 100);
                    var phone1 = PreserveOptional(erpCustomer.CariTel, 100);
                    var address = PreserveOptional(erpCustomer.CariAdres, 500);
                    var salesRepCode = PreserveOptional(erpCustomer.PlasiyerKodu, 50);
                    var groupCode = PreserveOptional(erpCustomer.GrupKodu, 50);
                    var branchCode = erpCustomer.SubeKodu;
                    var businessUnitCode = erpCustomer.IsletmeKodu;
                    var nowUtc = DateTime.UtcNow;

                    if (customer == null)
                    {
                        await InsertCustomerAsync(
                            code,
                            name,
                            taxOffice,
                            taxNumber,
                            tcknNumber,
                            email,
                            website,
                            phone1,
                            address,
                            salesRepCode,
                            groupCode,
                            branchCode,
                            businessUnitCode,
                            salesRepCodeColumnName,
                            groupCodeColumnName,
                            nowUtc);

                        existingCustomers[code] = new CustomerSyncRecord
                        {
                            CustomerCode = code,
                            CustomerName = name,
                            TaxOffice = taxOffice,
                            TaxNumber = taxNumber,
                            TcknNumber = tcknNumber,
                            Email = email,
                            Website = website,
                            Phone1 = phone1,
                            Address = address,
                            SalesRepCode = supportsSalesRepCode ? salesRepCode : null,
                            GroupCode = supportsGroupCode ? groupCode : null,
                            BranchCode = branchCode,
                            BusinessUnitCode = businessUnitCode,
                            IsDeleted = false,
                            IsERPIntegrated = true,
                            ERPIntegrationNumber = code
                        };
                        createdCount++;
                        continue;
                    }

                    var updated = false;
                    var reactivated = false;
                    var wasDeleted = customer.IsDeleted;

                    if (!StringEquals(customer.CustomerName, name)) { customer.CustomerName = name; updated = true; }
                    if (!StringEquals(customer.TaxOffice, taxOffice)) { customer.TaxOffice = taxOffice; updated = true; }
                    if (!StringEquals(customer.TaxNumber, taxNumber)) { customer.TaxNumber = taxNumber; updated = true; }
                    if (!StringEquals(customer.TcknNumber, tcknNumber)) { customer.TcknNumber = tcknNumber; updated = true; }
                    if (!StringEquals(customer.Email, email)) { customer.Email = email; updated = true; }
                    if (!StringEquals(customer.Website, website)) { customer.Website = website; updated = true; }
                    if (!StringEquals(customer.Phone1, phone1)) { customer.Phone1 = phone1; updated = true; }
                    if (!StringEquals(customer.Address, address)) { customer.Address = address; updated = true; }
                    if (supportsSalesRepCode && !StringEquals(customer.SalesRepCode, salesRepCode)) { customer.SalesRepCode = salesRepCode; updated = true; }
                    if (supportsGroupCode && !StringEquals(customer.GroupCode, groupCode)) { customer.GroupCode = groupCode; updated = true; }
                    if (customer.BranchCode != branchCode) { customer.BranchCode = branchCode; updated = true; }
                    if (customer.BusinessUnitCode != businessUnitCode) { customer.BusinessUnitCode = businessUnitCode; updated = true; }

                    if (wasDeleted)
                    {
                        customer.IsDeleted = false;
                        updated = true;
                        reactivated = true;
                    }

                    if (customer.IsERPIntegrated != true) { customer.IsERPIntegrated = true; updated = true; }
                    if (customer.ERPIntegrationNumber != code) { customer.ERPIntegrationNumber = code; updated = true; }

                    if (!updated)
                    {
                        continue;
                    }

                    await UpdateCustomerAsync(
                        customer.Id,
                        code,
                        name,
                        taxOffice,
                        taxNumber,
                        tcknNumber,
                        email,
                        website,
                        phone1,
                        address,
                        salesRepCode,
                        groupCode,
                        branchCode,
                        businessUnitCode,
                        salesRepCodeColumnName,
                        groupCodeColumnName,
                        nowUtc,
                        wasDeleted);

                    if (reactivated)
                    {
                        reactivatedCount++;
                    }
                    else
                    {
                        updatedCount++;
                    }

                    customer.IsDeleted = false;
                    customer.CustomerName = name;
                    customer.TaxOffice = taxOffice;
                    customer.TaxNumber = taxNumber;
                    customer.TcknNumber = tcknNumber;
                    customer.Email = email;
                    customer.Website = website;
                    customer.Phone1 = phone1;
                    customer.Address = address;
                    if (supportsSalesRepCode) customer.SalesRepCode = salesRepCode;
                    if (supportsGroupCode) customer.GroupCode = groupCode;
                    customer.BranchCode = branchCode;
                    customer.BusinessUnitCode = businessUnitCode;
                    customer.IsERPIntegrated = true;
                    customer.ERPIntegrationNumber = code;
                }
                catch (Exception ex)
                {
                    failedCount++;
                    failedCodes.Add(code);
                    await LogRecordFailureAsync(code, ex);
                    _db.ChangeTracker.Clear();
                }
            }

            if (failedCount > 0)
            {
                var sampleFailedCodes = string.Join(", ", failedCodes.Take(10));
                throw new InvalidOperationException(
                    $"Customer sync completed with record failures. ERP={erpResponse.Data.Count}, Created={createdCount}, Updated={updatedCount}, Reactivated={reactivatedCount}, Failed={failedCount}, Skipped={skippedCount}, SampleFailedCodes=[{sampleFailedCodes}]");
            }

            if (erpResponse.Data.Count > 0 &&
                existingCustomers.Count == 0 &&
                createdCount == 0 &&
                updatedCount == 0 &&
                reactivatedCount == 0)
            {
                throw new InvalidOperationException(
                    $"Customer sync finished without mirroring any ERP customer. ERP={erpResponse.Data.Count}, Created={createdCount}, Updated={updatedCount}, Reactivated={reactivatedCount}, Skipped={skippedCount}");
            }

            _logger.LogInformation(
                "Customer sync completed. created={Created}, updated={Updated}, reactivated={Reactivated}, failed={Failed}, skipped={Skipped}.",
                createdCount,
                updatedCount,
                reactivatedCount,
                failedCount,
                skippedCount);
                _logger.LogInformation(_localizationService.GetLocalizedString("CustomerSyncJob.Completed"));
        }

        private async Task<HashSet<string>> GetCustomerColumnNamesAsync()
        {
            var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var connection = _db.Database.GetDbConnection();
            var shouldClose = connection.State != ConnectionState.Open;

            if (shouldClose)
            {
                await connection.OpenAsync().ConfigureAwait(false);
            }

            try
            {
                await using var command = connection.CreateCommand();
                command.CommandText = @"
SELECT COLUMN_NAME
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'RII_CUSTOMER';";

                await using var reader = await command.ExecuteReaderAsync().ConfigureAwait(false);
                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    if (!reader.IsDBNull(0))
                    {
                        result.Add(reader.GetString(0));
                    }
                }
            }
            finally
            {
                if (shouldClose)
                {
                    await connection.CloseAsync().ConfigureAwait(false);
                }
            }

            return result;
        }

        private async Task<Dictionary<string, CustomerSyncRecord>> LoadExistingCustomersAsync(string? salesRepCodeColumnName, string? groupCodeColumnName)
        {
            var selectColumns = new List<string>
            {
                "[Id]",
                "[CustomerCode]",
                "[CustomerName]",
                "[TaxOffice]",
                "[TaxNumber]",
                "[TcknNumber]",
                "[Email]",
                "[Website]",
                "[Phone1]",
                "[Address]",
                "[BranchCode]",
                "[BusinessUnitCode]",
                "[IsDeleted]",
                "[IS_ERP_INTEGRATED]",
                "[ERP_INTEGRATION_NUMBER]"
            };

            if (!string.IsNullOrWhiteSpace(salesRepCodeColumnName))
            {
                selectColumns.Add($"[{salesRepCodeColumnName}]");
            }

            if (!string.IsNullOrWhiteSpace(groupCodeColumnName))
            {
                selectColumns.Add($"[{groupCodeColumnName}]");
            }

            var records = new Dictionary<string, CustomerSyncRecord>(StringComparer.OrdinalIgnoreCase);
            var connection = _db.Database.GetDbConnection();
            var shouldClose = connection.State != ConnectionState.Open;

            if (shouldClose)
            {
                await connection.OpenAsync().ConfigureAwait(false);
            }

            try
            {
                await using var command = connection.CreateCommand();
                command.CommandText = $"SELECT {string.Join(", ", selectColumns)} FROM [dbo].[RII_CUSTOMER];";

                await using var reader = await command.ExecuteReaderAsync().ConfigureAwait(false);
                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    var code = ReadNullableString(reader, "CustomerCode");
                    if (string.IsNullOrWhiteSpace(code))
                    {
                        continue;
                    }

                    records[code.Trim()] = new CustomerSyncRecord
                    {
                        Id = ReadInt64(reader, "Id"),
                        CustomerCode = code.Trim(),
                        CustomerName = ReadNullableString(reader, "CustomerName") ?? string.Empty,
                        TaxOffice = ReadNullableString(reader, "TaxOffice"),
                        TaxNumber = ReadNullableString(reader, "TaxNumber"),
                        TcknNumber = ReadNullableString(reader, "TcknNumber"),
                        Email = ReadNullableString(reader, "Email"),
                        Website = ReadNullableString(reader, "Website"),
                        Phone1 = ReadNullableString(reader, "Phone1"),
                        Address = ReadNullableString(reader, "Address"),
                        SalesRepCode = !string.IsNullOrWhiteSpace(salesRepCodeColumnName) ? ReadNullableString(reader, salesRepCodeColumnName) : null,
                        GroupCode = !string.IsNullOrWhiteSpace(groupCodeColumnName) ? ReadNullableString(reader, groupCodeColumnName) : null,
                        BranchCode = ReadInt16(reader, "BranchCode"),
                        BusinessUnitCode = ReadInt16(reader, "BusinessUnitCode"),
                        IsDeleted = ReadBoolean(reader, "IsDeleted"),
                        IsERPIntegrated = ReadBoolean(reader, "IS_ERP_INTEGRATED"),
                        ERPIntegrationNumber = ReadNullableString(reader, "ERP_INTEGRATION_NUMBER")
                    };
                }
            }
            finally
            {
                if (shouldClose)
                {
                    await connection.CloseAsync().ConfigureAwait(false);
                }
            }

            return records;
        }

        private async Task InsertCustomerAsync(
            string code,
            string name,
            string? taxOffice,
            string? taxNumber,
            string? tcknNumber,
            string? email,
            string? website,
            string? phone1,
            string? address,
            string? salesRepCode,
            string? groupCode,
            short branchCode,
            short businessUnitCode,
            string? salesRepCodeColumnName,
            string? groupCodeColumnName,
            DateTime nowUtc)
        {
            var columns = new List<string>
            {
                "[CustomerCode]",
                "[CustomerName]",
                "[TaxOffice]",
                "[TaxNumber]",
                "[TcknNumber]",
                "[Email]",
                "[Website]",
                "[Phone1]",
                "[Address]",
                "[BranchCode]",
                "[BusinessUnitCode]",
                "[Year]",
                "[IsDeleted]",
                "[IS_ERP_INTEGRATED]",
                "[ERP_INTEGRATION_NUMBER]",
                "[LAST_SYNC_DATE]",
                "[CreatedDate]",
                "[COUNT_TRIED_BY]"
            };

            var parameterNames = new List<string>
            {
                "@CustomerCode",
                "@CustomerName",
                "@TaxOffice",
                "@TaxNumber",
                "@TcknNumber",
                "@Email",
                "@Website",
                "@Phone1",
                "@Address",
                "@BranchCode",
                "@BusinessUnitCode",
                "@Year",
                "@IsDeleted",
                "@IsERPIntegrated",
                "@ERPIntegrationNumber",
                "@LastSyncDate",
                "@CreatedDate",
                "@CountTriedBy"
            };

            if (!string.IsNullOrWhiteSpace(salesRepCodeColumnName))
            {
                columns.Add($"[{salesRepCodeColumnName}]");
                parameterNames.Add("@SalesRepCode");
            }

            if (!string.IsNullOrWhiteSpace(groupCodeColumnName))
            {
                columns.Add($"[{groupCodeColumnName}]");
                parameterNames.Add("@GroupCode");
            }

            var sql = $@"
INSERT INTO [dbo].[RII_CUSTOMER] ({string.Join(", ", columns)})
VALUES ({string.Join(", ", parameterNames)});";

            var parameters = new List<object>
            {
                CreateSqlParameter("@CustomerCode", code),
                CreateSqlParameter("@CustomerName", name),
                CreateSqlParameter("@TaxOffice", taxOffice),
                CreateSqlParameter("@TaxNumber", taxNumber),
                CreateSqlParameter("@TcknNumber", tcknNumber),
                CreateSqlParameter("@Email", email),
                CreateSqlParameter("@Website", website),
                CreateSqlParameter("@Phone1", phone1),
                CreateSqlParameter("@Address", address),
                CreateSqlParameter("@BranchCode", branchCode),
                CreateSqlParameter("@BusinessUnitCode", businessUnitCode),
                CreateSqlParameter("@Year", nowUtc.Year.ToString()),
                CreateSqlParameter("@IsDeleted", false),
                CreateSqlParameter("@IsERPIntegrated", true),
                CreateSqlParameter("@ERPIntegrationNumber", code),
                CreateSqlParameter("@LastSyncDate", nowUtc),
                CreateSqlParameter("@CreatedDate", nowUtc),
                CreateSqlParameter("@CountTriedBy", 0),
            };

            if (!string.IsNullOrWhiteSpace(salesRepCodeColumnName))
            {
                parameters.Add(CreateSqlParameter("@SalesRepCode", (object?)salesRepCode));
            }

            if (!string.IsNullOrWhiteSpace(groupCodeColumnName))
            {
                parameters.Add(CreateSqlParameter("@GroupCode", (object?)groupCode));
            }

            await _db.Database.ExecuteSqlRawAsync(sql, parameters.ToArray()).ConfigureAwait(false);
        }

        private async Task UpdateCustomerAsync(
            long id,
            string code,
            string name,
            string? taxOffice,
            string? taxNumber,
            string? tcknNumber,
            string? email,
            string? website,
            string? phone1,
            string? address,
            string? salesRepCode,
            string? groupCode,
            short branchCode,
            short businessUnitCode,
            string? salesRepCodeColumnName,
            string? groupCodeColumnName,
            DateTime nowUtc,
            bool wasDeleted)
        {
            var setClauses = new List<string>
            {
                "[CustomerName] = @CustomerName",
                "[TaxOffice] = @TaxOffice",
                "[TaxNumber] = @TaxNumber",
                "[TcknNumber] = @TcknNumber",
                "[Email] = @Email",
                "[Website] = @Website",
                "[Phone1] = @Phone1",
                "[Address] = @Address",
                "[BranchCode] = @BranchCode",
                "[BusinessUnitCode] = @BusinessUnitCode",
                "[IS_ERP_INTEGRATED] = @IsERPIntegrated",
                "[ERP_INTEGRATION_NUMBER] = @ERPIntegrationNumber",
                "[LAST_SYNC_DATE] = @LastSyncDate",
                "[UpdatedDate] = @UpdatedDate",
                "[UpdatedBy] = @UpdatedBy"
            };

            if (!string.IsNullOrWhiteSpace(salesRepCodeColumnName))
            {
                setClauses.Add($"[{salesRepCodeColumnName}] = @SalesRepCode");
            }

            if (!string.IsNullOrWhiteSpace(groupCodeColumnName))
            {
                setClauses.Add($"[{groupCodeColumnName}] = @GroupCode");
            }

            if (wasDeleted)
            {
                setClauses.Add("[IsDeleted] = 0");
                setClauses.Add("[DeletedDate] = NULL");
                setClauses.Add("[DeletedBy] = NULL");
            }

            var sql = $@"
UPDATE [dbo].[RII_CUSTOMER]
SET {string.Join(", ", setClauses)}
WHERE [Id] = @Id;";

            var parameters = new List<object>
            {
                CreateSqlParameter("@Id", id),
                CreateSqlParameter("@CustomerName", name),
                CreateSqlParameter("@TaxOffice", taxOffice),
                CreateSqlParameter("@TaxNumber", taxNumber),
                CreateSqlParameter("@TcknNumber", tcknNumber),
                CreateSqlParameter("@Email", email),
                CreateSqlParameter("@Website", website),
                CreateSqlParameter("@Phone1", phone1),
                CreateSqlParameter("@Address", address),
                CreateSqlParameter("@BranchCode", branchCode),
                CreateSqlParameter("@BusinessUnitCode", businessUnitCode),
                CreateSqlParameter("@IsERPIntegrated", true),
                CreateSqlParameter("@ERPIntegrationNumber", code),
                CreateSqlParameter("@LastSyncDate", nowUtc),
                CreateSqlParameter("@UpdatedDate", nowUtc),
                CreateSqlParameter("@UpdatedBy", DBNull.Value),
            };

            if (!string.IsNullOrWhiteSpace(salesRepCodeColumnName))
            {
                parameters.Add(CreateSqlParameter("@SalesRepCode", (object?)salesRepCode));
            }

            if (!string.IsNullOrWhiteSpace(groupCodeColumnName))
            {
                parameters.Add(CreateSqlParameter("@GroupCode", (object?)groupCode));
            }

            await _db.Database.ExecuteSqlRawAsync(sql, parameters.ToArray()).ConfigureAwait(false);
        }

        private async Task LogRecordFailureAsync(string code, Exception ex)
        {
            _logger.LogError(ex, "Customer sync record failed. CustomerCode: {CustomerCode}", code);

            try
            {
                _db.JobFailureLogs.Add(new JobFailureLog
                {
                    JobId = $"{RecurringJobId}:{code}:{DateTime.UtcNow:yyyyMMddHHmmssfff}",
                    JobName = $"{typeof(CustomerSyncJob).FullName}.ExecuteAsync",
                    FailedAt = DateTime.UtcNow,
                    Reason = $"CustomerCode={code}",
                    ExceptionType = ex.GetType().FullName,
                    ExceptionMessage = ex.Message,
                    StackTrace = ex.StackTrace?.Length > 8000 ? ex.StackTrace[..8000] : ex.StackTrace,
                    Queue = "default",
                    RetryCount = 0,
                    CreatedDate = DateTimeProvider.Now,
                    IsDeleted = false
                });
                await _db.SaveChangesAsync();
            }
            catch (Exception logEx)
            {
                _logger.LogWarning(logEx, "Customer sync failure could not be written to RII_JOB_FAILURE_LOG. CustomerCode: {CustomerCode}", code);
            }
        }

        private static string PreserveRequired(string? value, string fallback, int maxLength)
        {
            var preserved = PreserveOptional(value, maxLength);
            if (string.IsNullOrEmpty(preserved))
            {
                return fallback.Length > maxLength ? fallback[..maxLength] : fallback;
            }

            return preserved;
        }

        private static string? PreserveOptional(string? value, int maxLength)
        {
            if (value == null)
            {
                return null;
            }

            return value.Length > maxLength ? value[..maxLength] : value;
        }

        private static string? ResolveColumnName(HashSet<string> availableColumns, params string[] candidates)
        {
            foreach (var candidate in candidates)
            {
                var resolved = availableColumns.FirstOrDefault(x => string.Equals(x, candidate, StringComparison.OrdinalIgnoreCase));
                if (!string.IsNullOrWhiteSpace(resolved))
                {
                    return resolved;
                }
            }

            return null;
        }

        private static bool StringEquals(string? left, string? right)
        {
            return string.Equals(left ?? string.Empty, right ?? string.Empty, StringComparison.Ordinal);
        }

        private static object CreateSqlParameter(string name, object? value)
        {
            return new Microsoft.Data.SqlClient.SqlParameter(name, value ?? DBNull.Value);
        }

        private static string? ReadNullableString(DbDataReader reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);
        }

        private static long ReadInt64(DbDataReader reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            return reader.GetInt64(ordinal);
        }

        private static short ReadInt16(DbDataReader reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            return reader.GetInt16(ordinal);
        }

        private static bool ReadBoolean(DbDataReader reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            return !reader.IsDBNull(ordinal) && reader.GetBoolean(ordinal);
        }

        private sealed class CustomerSyncRecord
        {
            public long Id { get; set; }
            public string CustomerCode { get; set; } = string.Empty;
            public string CustomerName { get; set; } = string.Empty;
            public string? TaxOffice { get; set; }
            public string? TaxNumber { get; set; }
            public string? TcknNumber { get; set; }
            public string? Email { get; set; }
            public string? Website { get; set; }
            public string? Phone1 { get; set; }
            public string? Address { get; set; }
            public string? SalesRepCode { get; set; }
            public string? GroupCode { get; set; }
            public short BranchCode { get; set; }
            public short BusinessUnitCode { get; set; }
            public bool IsDeleted { get; set; }
            public bool IsERPIntegrated { get; set; }
            public string? ERPIntegrationNumber { get; set; }
        }
    }
}
