using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace cms_webapi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RII_ACTIVITY",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Subject = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ActivityType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PotentialCustomerId = table.Column<long>(type: "bigint", nullable: true),
                    ErpCustomerCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Priority = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContactId = table.Column<long>(type: "bigint", nullable: true),
                    AssignedUserId = table.Column<long>(type: "bigint", nullable: true),
                    ActivityDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActivityTypeId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_ACTIVITY", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RII_ACTIVITY_TYPE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_ACTIVITY_TYPE", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RII_APPROVAL_AUTHORITY",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    MaxApprovalAmount = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    ApprovalLevel = table.Column<int>(type: "int", nullable: false),
                    CanFinalize = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    RequireUpperManagement = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_APPROVAL_AUTHORITY", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RII_APPROVAL_QUEUE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuotationId = table.Column<long>(type: "bigint", nullable: false),
                    QuotationLineId = table.Column<long>(type: "bigint", nullable: true),
                    AssignedToUserId = table.Column<long>(type: "bigint", nullable: false),
                    ApprovalLevel = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SequenceOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    IsCurrent = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_APPROVAL_QUEUE", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RII_APPROVAL_RULE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApprovalAuthorityId = table.Column<long>(type: "bigint", nullable: false),
                    ForwardToUpperManagement = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ForwardToLevel = table.Column<int>(type: "int", nullable: true),
                    RequireFinanceApproval = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_APPROVAL_RULE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_APPROVAL_RULE_RII_APPROVAL_AUTHORITY_ApprovalAuthorityId",
                        column: x => x.ApprovalAuthorityId,
                        principalTable: "RII_APPROVAL_AUTHORITY",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_APPROVAL_TRANSACTION",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentId = table.Column<long>(type: "bigint", nullable: false),
                    LineId = table.Column<long>(type: "bigint", nullable: true),
                    ApprovalLevel = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    ApprovedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    RequestedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ActionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_APPROVAL_TRANSACTION", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RII_APPROVAL_WORKFLOW",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerTypeId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    MinAmount = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    MaxAmount = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_APPROVAL_WORKFLOW", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RII_CITY",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ERPCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CountryId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_CITY", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RII_CONTACT",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    TitleId = table.Column<long>(type: "bigint", nullable: false),
                    CountryId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_CONTACT", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RII_COUNTRY",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    ERPCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_COUNTRY", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RII_CUSTOMER",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerCode = table.Column<string>(type: "nvarchar(100)", maxLength: 50, nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(250)", maxLength: 200, nullable: false),
                    CustomerTypeId = table.Column<long>(type: "bigint", nullable: true),
                    TaxOffice = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TaxNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TcknNumber = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    SalesRepCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GroupCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreditLimit = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    BranchCode = table.Column<short>(type: "smallint", nullable: false),
                    BusinessUnitCode = table.Column<short>(type: "smallint", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Phone1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Phone2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CountryId = table.Column<long>(type: "bigint", nullable: true),
                    CityId = table.Column<long>(type: "bigint", nullable: true),
                    DistrictId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    Year = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    COMPLETION_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IS_COMPLETED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IS_PENDING_APPROVAL = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    APPROVAL_STATUS = table.Column<bool>(type: "bit", nullable: true),
                    REJECTED_REASON = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    APPROVED_BY_USER_ID = table.Column<long>(type: "bigint", nullable: true),
                    APPROVAL_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IS_ERP_INTEGRATED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ERP_INTEGRATION_NUMBER = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LAST_SYNC_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    COUNT_TRIED_BY = table.Column<int>(type: "int", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_CUSTOMER", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_CUSTOMER_RII_CITY_CityId",
                        column: x => x.CityId,
                        principalTable: "RII_CITY",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_CUSTOMER_RII_COUNTRY_CountryId",
                        column: x => x.CountryId,
                        principalTable: "RII_COUNTRY",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_CUSTOMER_TYPE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_CUSTOMER_TYPE", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RII_DISTRICT",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ERPCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CityId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_DISTRICT", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_DISTRICT_RII_CITY_CityId",
                        column: x => x.CityId,
                        principalTable: "RII_CITY",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_DOCUMENT_SERIAL_TYPE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RuleType = table.Column<int>(type: "int", nullable: false),
                    CustomerTypeId = table.Column<long>(type: "bigint", nullable: true),
                    SalesRepId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_DOCUMENT_SERIAL_TYPE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_DOCUMENT_SERIAL_TYPE_RII_CUSTOMER_TYPE_CustomerTypeId",
                        column: x => x.CustomerTypeId,
                        principalTable: "RII_CUSTOMER_TYPE",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_PASSWORD_RESET_REQUEST",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    TokenHash = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedByUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_PASSWORD_RESET_REQUEST", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RII_PAYMENT_TYPE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_PAYMENT_TYPE", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RII_PRICING_RULE_HEADER",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RuleType = table.Column<int>(type: "int", nullable: false),
                    RuleCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RuleName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CustomerId = table.Column<long>(type: "bigint", nullable: true),
                    ErpCustomerCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BranchCode = table.Column<short>(type: "smallint", nullable: true),
                    PriceIncludesVat = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_PRICING_RULE_HEADER", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_PRICING_RULE_HEADER_RII_CUSTOMER_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "RII_CUSTOMER",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_PRICING_RULE_LINE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PricingRuleHeaderId = table.Column<long>(type: "bigint", nullable: false),
                    StokCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MinQuantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    MaxQuantity = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    FixedUnitPrice = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    CurrencyCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "TRY"),
                    DiscountRate1 = table.Column<decimal>(type: "decimal(18,6)", nullable: false, defaultValue: 0m),
                    DiscountAmount1 = table.Column<decimal>(type: "decimal(18,6)", nullable: false, defaultValue: 0m),
                    DiscountRate2 = table.Column<decimal>(type: "decimal(18,6)", nullable: false, defaultValue: 0m),
                    DiscountAmount2 = table.Column<decimal>(type: "decimal(18,6)", nullable: false, defaultValue: 0m),
                    DiscountRate3 = table.Column<decimal>(type: "decimal(18,6)", nullable: false, defaultValue: 0m),
                    DiscountAmount3 = table.Column<decimal>(type: "decimal(18,6)", nullable: false, defaultValue: 0m),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_PRICING_RULE_LINE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_PRICING_RULE_LINE_RII_PRICING_RULE_HEADER_PricingRuleHeaderId",
                        column: x => x.PricingRuleHeaderId,
                        principalTable: "RII_PRICING_RULE_HEADER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RII_PRICING_RULE_SALESMAN",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PricingRuleHeaderId = table.Column<long>(type: "bigint", nullable: false),
                    SalesmanId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_PRICING_RULE_SALESMAN", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_PRICING_RULE_SALESMAN_RII_PRICING_RULE_HEADER_PricingRuleHeaderId",
                        column: x => x.PricingRuleHeaderId,
                        principalTable: "RII_PRICING_RULE_HEADER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RII_PRODUCT_PRICING",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ErpProductCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ErpGroupCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ListPrice = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    CostPrice = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Discount1 = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    Discount2 = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    Discount3 = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_PRODUCT_PRICING", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RII_PRODUCT_PRICING_GROUP_BY",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ErpGroupCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ListPrice = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    CostPrice = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Discount1 = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    Discount2 = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    Discount3 = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_PRODUCT_PRICING_GROUP_BY", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RII_QUOTATION",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PotentialCustomerId = table.Column<long>(type: "bigint", nullable: true),
                    ErpCustomerCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContactId = table.Column<long>(type: "bigint", nullable: true),
                    ValidUntil = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ShippingAddressId = table.Column<long>(type: "bigint", nullable: true),
                    RepresentativeId = table.Column<long>(type: "bigint", nullable: true),
                    ActivityId = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PaymentTypeId = table.Column<long>(type: "bigint", nullable: true),
                    OfferType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OfferDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OfferNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RevisionNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RevisionId = table.Column<long>(type: "bigint", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HasCustomerSpecificDiscount = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Total = table.Column<decimal>(type: "decimal(18,6)", nullable: false, defaultValue: 0m),
                    GrandTotal = table.Column<decimal>(type: "decimal(18,6)", nullable: false, defaultValue: 0m),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    Year = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    CompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsPendingApproval = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ApprovalStatus = table.Column<bool>(type: "bit", nullable: true),
                    RejectedReason = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ApprovedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsERPIntegrated = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ERPIntegrationNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LastSyncDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CountTriedBy = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_QUOTATION", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_QUOTATION_RII_ACTIVITY_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "RII_ACTIVITY",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_QUOTATION_RII_CONTACT_ContactId",
                        column: x => x.ContactId,
                        principalTable: "RII_CONTACT",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_QUOTATION_RII_CUSTOMER_PotentialCustomerId",
                        column: x => x.PotentialCustomerId,
                        principalTable: "RII_CUSTOMER",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_QUOTATION_RII_PAYMENT_TYPE_PaymentTypeId",
                        column: x => x.PaymentTypeId,
                        principalTable: "RII_PAYMENT_TYPE",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_QUOTATION_EXCHANGE_RATE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuotationId = table.Column<long>(type: "bigint", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ExchangeRate = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    ExchangeRateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsOfficial = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_QUOTATION_EXCHANGE_RATE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_QUOTATION_EXCHANGE_RATE_RII_QUOTATION_QuotationId",
                        column: x => x.QuotationId,
                        principalTable: "RII_QUOTATION",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RII_QUOTATION_LINE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuotationId = table.Column<long>(type: "bigint", nullable: false),
                    ProductCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    DiscountRate1 = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    DiscountAmount1 = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    DiscountRate2 = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    DiscountAmount2 = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    DiscountRate3 = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    DiscountAmount3 = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    VatRate = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    VatAmount = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    LineTotal = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    LineGrandTotal = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ApprovalStatus = table.Column<int>(type: "int", nullable: false),
                    PricingRuleHeaderId = table.Column<long>(type: "bigint", nullable: true),
                    RelatedStockId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_QUOTATION_LINE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_QUOTATION_LINE_RII_PRICING_RULE_HEADER_PricingRuleHeaderId",
                        column: x => x.PricingRuleHeaderId,
                        principalTable: "RII_PRICING_RULE_HEADER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_RII_QUOTATION_LINE_RII_QUOTATION_QuotationId",
                        column: x => x.QuotationId,
                        principalTable: "RII_QUOTATION",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RII_SHIPPING_ADDRESS",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ContactPerson = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    CountryId = table.Column<long>(type: "bigint", nullable: true),
                    CityId = table.Column<long>(type: "bigint", nullable: true),
                    DistrictId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_SHIPPING_ADDRESS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_SHIPPING_ADDRESS_RII_CITY_CityId",
                        column: x => x.CityId,
                        principalTable: "RII_CITY",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_SHIPPING_ADDRESS_RII_COUNTRY_CountryId",
                        column: x => x.CountryId,
                        principalTable: "RII_COUNTRY",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_SHIPPING_ADDRESS_RII_CUSTOMER_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "RII_CUSTOMER",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_SHIPPING_ADDRESS_RII_DISTRICT_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "RII_DISTRICT",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_STOCK",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ErpStockCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StockName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    UreticiKodu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GrupKodu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GrupAdi = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Kod1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Kod1Adi = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Kod2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Kod2Adi = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Kod3 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Kod3Adi = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Kod4 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Kod4Adi = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Kod5 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Kod5Adi = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    BranchCode = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_STOCK", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RII_STOCK_DETAIL",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockId = table.Column<long>(type: "bigint", nullable: false),
                    HtmlDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TechnicalSpecsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_STOCK_DETAIL", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_STOCK_DETAIL_RII_STOCK_StockId",
                        column: x => x.StockId,
                        principalTable: "RII_STOCK",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RII_STOCK_IMAGE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockId = table.Column<long>(type: "bigint", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    AltText = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_STOCK_IMAGE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_STOCK_IMAGE_RII_STOCK_StockId",
                        column: x => x.StockId,
                        principalTable: "RII_STOCK",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RII_STOCK_RELATION",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockId = table.Column<long>(type: "bigint", nullable: false),
                    RelatedStockId = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsMandatory = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_STOCK_RELATION", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_STOCK_RELATION_RII_STOCK_RelatedStockId",
                        column: x => x.RelatedStockId,
                        principalTable: "RII_STOCK",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_STOCK_RELATION_RII_STOCK_StockId",
                        column: x => x.StockId,
                        principalTable: "RII_STOCK",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RII_TITLE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_TITLE", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RII_USER_AUTHORITY",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_USER_AUTHORITY", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RII_USERS",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    IsEmailConfirmed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LastLoginDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_USERS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_USERS_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_USERS_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_USERS_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_USERS_RII_USER_AUTHORITY_RoleId",
                        column: x => x.RoleId,
                        principalTable: "RII_USER_AUTHORITY",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_USER_DETAIL",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ProfilePictureUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Height = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Weight = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Gender = table.Column<byte>(type: "tinyint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_USER_DETAIL", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_USER_DETAIL_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_USER_DETAIL_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_USER_DETAIL_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_USER_DETAIL_RII_USERS_UserId",
                        column: x => x.UserId,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_USER_DISCOUNT_LIMIT",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ErpProductGroupCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SalespersonId = table.Column<long>(type: "bigint", nullable: false),
                    MaxDiscount1 = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    MaxDiscount2 = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    MaxDiscount3 = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_USER_DISCOUNT_LIMIT", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_USER_DISCOUNT_LIMIT_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_USER_DISCOUNT_LIMIT_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_USER_DISCOUNT_LIMIT_RII_USERS_SalespersonId",
                        column: x => x.SalespersonId,
                        principalTable: "RII_USERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_USER_DISCOUNT_LIMIT_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_USER_HIERARCHY",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalespersonId = table.Column<long>(type: "bigint", nullable: false),
                    ManagerId = table.Column<long>(type: "bigint", nullable: true),
                    GeneralManagerId = table.Column<long>(type: "bigint", nullable: true),
                    HierarchyLevel = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_USER_HIERARCHY", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_USER_HIERARCHY_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_USER_HIERARCHY_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_USER_HIERARCHY_RII_USERS_GeneralManagerId",
                        column: x => x.GeneralManagerId,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_USER_HIERARCHY_RII_USERS_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_USER_HIERARCHY_RII_USERS_SalespersonId",
                        column: x => x.SalespersonId,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_USER_HIERARCHY_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_USER_SESSION",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DeviceInfo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_USER_SESSION", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_USER_SESSION_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_USER_SESSION_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_USER_SESSION_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_USER_SESSION_RII_USERS_UserId",
                        column: x => x.UserId,
                        principalTable: "RII_USERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "RII_USER_AUTHORITY",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "DeletedBy", "DeletedDate", "IsDeleted", "Title", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "User", null, null },
                    { 2L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Manager", null, null },
                    { 3L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "Admin", null, null }
                });

            migrationBuilder.InsertData(
                table: "RII_USERS",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "DeletedBy", "DeletedDate", "Email", "FirstName", "IsActive", "IsDeleted", "IsEmailConfirmed", "LastLoginDate", "LastName", "PasswordHash", "PhoneNumber", "RefreshToken", "RefreshTokenExpiryTime", "RoleId", "UpdatedBy", "UpdatedDate", "Username" },
                values: new object[] { 1L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "v3rii@v3rii.com", "Admin", true, false, true, null, "User", "$2a$11$abcdefghijklmnopqrstuuNIZsBQfUYLG05oQWoW6wLHKeQreQYs6", null, null, null, 3L, null, null, "admin@v3rii.com" });

            migrationBuilder.CreateIndex(
                name: "IX_Activity_ActivityDate",
                table: "RII_ACTIVITY",
                column: "ActivityDate");

            migrationBuilder.CreateIndex(
                name: "IX_Activity_ActivityType",
                table: "RII_ACTIVITY",
                column: "ActivityType");

            migrationBuilder.CreateIndex(
                name: "IX_Activity_AssignedUserId",
                table: "RII_ACTIVITY",
                column: "AssignedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Activity_ContactId",
                table: "RII_ACTIVITY",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Activity_IsCompleted",
                table: "RII_ACTIVITY",
                column: "IsCompleted");

            migrationBuilder.CreateIndex(
                name: "IX_Activity_IsDeleted",
                table: "RII_ACTIVITY",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Activity_PotentialCustomerId",
                table: "RII_ACTIVITY",
                column: "PotentialCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Activity_Status",
                table: "RII_ACTIVITY",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Activity_Subject",
                table: "RII_ACTIVITY",
                column: "Subject");

            migrationBuilder.CreateIndex(
                name: "IX_RII_ACTIVITY_ActivityTypeId",
                table: "RII_ACTIVITY",
                column: "ActivityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_ACTIVITY_CreatedBy",
                table: "RII_ACTIVITY",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_ACTIVITY_DeletedBy",
                table: "RII_ACTIVITY",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_ACTIVITY_UpdatedBy",
                table: "RII_ACTIVITY",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityType_CreatedDate",
                table: "RII_ACTIVITY_TYPE",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityType_IsDeleted",
                table: "RII_ACTIVITY_TYPE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityType_Name",
                table: "RII_ACTIVITY_TYPE",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_RII_ACTIVITY_TYPE_CreatedBy",
                table: "RII_ACTIVITY_TYPE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_ACTIVITY_TYPE_DeletedBy",
                table: "RII_ACTIVITY_TYPE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_ACTIVITY_TYPE_UpdatedBy",
                table: "RII_ACTIVITY_TYPE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalAuthority_ApprovalLevel",
                table: "RII_APPROVAL_AUTHORITY",
                column: "ApprovalLevel");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalAuthority_IsActive",
                table: "RII_APPROVAL_AUTHORITY",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalAuthority_IsDeleted",
                table: "RII_APPROVAL_AUTHORITY",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalAuthority_Level_IsActive",
                table: "RII_APPROVAL_AUTHORITY",
                columns: new[] { "ApprovalLevel", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalAuthority_UserId",
                table: "RII_APPROVAL_AUTHORITY",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_APPROVAL_AUTHORITY_CreatedBy",
                table: "RII_APPROVAL_AUTHORITY",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_APPROVAL_AUTHORITY_DeletedBy",
                table: "RII_APPROVAL_AUTHORITY",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_APPROVAL_AUTHORITY_UpdatedBy",
                table: "RII_APPROVAL_AUTHORITY",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalQueue_ApprovalLevel",
                table: "RII_APPROVAL_QUEUE",
                column: "ApprovalLevel");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalQueue_AssignedToUserId",
                table: "RII_APPROVAL_QUEUE",
                column: "AssignedToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalQueue_AssignedToUserId_Status_IsCurrent",
                table: "RII_APPROVAL_QUEUE",
                columns: new[] { "AssignedToUserId", "Status", "IsCurrent" });

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalQueue_IsCurrent",
                table: "RII_APPROVAL_QUEUE",
                column: "IsCurrent");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalQueue_IsDeleted",
                table: "RII_APPROVAL_QUEUE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalQueue_QuotationId",
                table: "RII_APPROVAL_QUEUE",
                column: "QuotationId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalQueue_QuotationId_IsCurrent_Status",
                table: "RII_APPROVAL_QUEUE",
                columns: new[] { "QuotationId", "IsCurrent", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalQueue_QuotationId_SequenceOrder",
                table: "RII_APPROVAL_QUEUE",
                columns: new[] { "QuotationId", "SequenceOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalQueue_QuotationLineId",
                table: "RII_APPROVAL_QUEUE",
                column: "QuotationLineId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalQueue_SequenceOrder",
                table: "RII_APPROVAL_QUEUE",
                column: "SequenceOrder");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalQueue_Status",
                table: "RII_APPROVAL_QUEUE",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_RII_APPROVAL_QUEUE_CreatedBy",
                table: "RII_APPROVAL_QUEUE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_APPROVAL_QUEUE_DeletedBy",
                table: "RII_APPROVAL_QUEUE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_APPROVAL_QUEUE_UpdatedBy",
                table: "RII_APPROVAL_QUEUE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalRule_ApprovalAuthorityId",
                table: "RII_APPROVAL_RULE",
                column: "ApprovalAuthorityId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalRule_AuthorityId_IsActive",
                table: "RII_APPROVAL_RULE",
                columns: new[] { "ApprovalAuthorityId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalRule_IsActive",
                table: "RII_APPROVAL_RULE",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalRule_IsDeleted",
                table: "RII_APPROVAL_RULE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_RII_APPROVAL_RULE_CreatedBy",
                table: "RII_APPROVAL_RULE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_APPROVAL_RULE_DeletedBy",
                table: "RII_APPROVAL_RULE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_APPROVAL_RULE_UpdatedBy",
                table: "RII_APPROVAL_RULE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalTransaction_ActionDate",
                table: "RII_APPROVAL_TRANSACTION",
                column: "ActionDate");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalTransaction_ApprovalLevel",
                table: "RII_APPROVAL_TRANSACTION",
                column: "ApprovalLevel");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalTransaction_ApprovedByUserId",
                table: "RII_APPROVAL_TRANSACTION",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalTransaction_DocumentId",
                table: "RII_APPROVAL_TRANSACTION",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalTransaction_DocumentId_LineId",
                table: "RII_APPROVAL_TRANSACTION",
                columns: new[] { "DocumentId", "LineId" });

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalTransaction_IsDeleted",
                table: "RII_APPROVAL_TRANSACTION",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalTransaction_LineId",
                table: "RII_APPROVAL_TRANSACTION",
                column: "LineId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalTransaction_RequestedAt",
                table: "RII_APPROVAL_TRANSACTION",
                column: "RequestedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalTransaction_Status",
                table: "RII_APPROVAL_TRANSACTION",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_RII_APPROVAL_TRANSACTION_CreatedBy",
                table: "RII_APPROVAL_TRANSACTION",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_APPROVAL_TRANSACTION_DeletedBy",
                table: "RII_APPROVAL_TRANSACTION",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_APPROVAL_TRANSACTION_UpdatedBy",
                table: "RII_APPROVAL_TRANSACTION",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalWorkflow_CustomerTypeId",
                table: "RII_APPROVAL_WORKFLOW",
                column: "CustomerTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalWorkflow_IsDeleted",
                table: "RII_APPROVAL_WORKFLOW",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalWorkflow_UserId",
                table: "RII_APPROVAL_WORKFLOW",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_APPROVAL_WORKFLOW_CreatedBy",
                table: "RII_APPROVAL_WORKFLOW",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_APPROVAL_WORKFLOW_DeletedBy",
                table: "RII_APPROVAL_WORKFLOW",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_APPROVAL_WORKFLOW_UpdatedBy",
                table: "RII_APPROVAL_WORKFLOW",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_City_CountryId",
                table: "RII_CITY",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_City_IsDeleted",
                table: "RII_CITY",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_City_Name",
                table: "RII_CITY",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_RII_CITY_CreatedBy",
                table: "RII_CITY",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_CITY_DeletedBy",
                table: "RII_CITY",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_CITY_UpdatedBy",
                table: "RII_CITY",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_CustomerId",
                table: "RII_CONTACT",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_Email",
                table: "RII_CONTACT",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_IsDeleted",
                table: "RII_CONTACT",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_TitleId",
                table: "RII_CONTACT",
                column: "TitleId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_CONTACT_CountryId",
                table: "RII_CONTACT",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_CONTACT_CreatedBy",
                table: "RII_CONTACT",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_CONTACT_DeletedBy",
                table: "RII_CONTACT",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_CONTACT_UpdatedBy",
                table: "RII_CONTACT",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Country_Code",
                table: "RII_COUNTRY",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Country_IsDeleted",
                table: "RII_COUNTRY",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Country_Name",
                table: "RII_COUNTRY",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_RII_COUNTRY_CreatedBy",
                table: "RII_COUNTRY",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_COUNTRY_DeletedBy",
                table: "RII_COUNTRY",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_COUNTRY_UpdatedBy",
                table: "RII_COUNTRY",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_CityId",
                table: "RII_CUSTOMER",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_CountryId",
                table: "RII_CUSTOMER",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_CustomerCode",
                table: "RII_CUSTOMER",
                column: "CustomerCode",
                unique: true,
                filter: "[CustomerCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_CustomerTypeId",
                table: "RII_CUSTOMER",
                column: "CustomerTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_DistrictId",
                table: "RII_CUSTOMER",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_Email",
                table: "RII_CUSTOMER",
                column: "Email",
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_IsDeleted",
                table: "RII_CUSTOMER",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_TaxNumber",
                table: "RII_CUSTOMER",
                column: "TaxNumber",
                filter: "[TaxNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_TcknNumber",
                table: "RII_CUSTOMER",
                column: "TcknNumber",
                filter: "[TcknNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RII_CUSTOMER_APPROVAL_DATE",
                table: "RII_CUSTOMER",
                column: "APPROVAL_DATE");

            migrationBuilder.CreateIndex(
                name: "IX_RII_CUSTOMER_APPROVAL_STATUS",
                table: "RII_CUSTOMER",
                column: "APPROVAL_STATUS");

            migrationBuilder.CreateIndex(
                name: "IX_RII_CUSTOMER_APPROVED_BY_USER_ID",
                table: "RII_CUSTOMER",
                column: "APPROVED_BY_USER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_RII_CUSTOMER_CreatedBy",
                table: "RII_CUSTOMER",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_CUSTOMER_DeletedBy",
                table: "RII_CUSTOMER",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_CUSTOMER_IS_COMPLETED",
                table: "RII_CUSTOMER",
                column: "IS_COMPLETED");

            migrationBuilder.CreateIndex(
                name: "IX_RII_CUSTOMER_UpdatedBy",
                table: "RII_CUSTOMER",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerType_IsDeleted",
                table: "RII_CUSTOMER_TYPE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerType_Name",
                table: "RII_CUSTOMER_TYPE",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RII_CUSTOMER_TYPE_CreatedBy",
                table: "RII_CUSTOMER_TYPE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_CUSTOMER_TYPE_DeletedBy",
                table: "RII_CUSTOMER_TYPE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_CUSTOMER_TYPE_UpdatedBy",
                table: "RII_CUSTOMER_TYPE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_District_CityId",
                table: "RII_DISTRICT",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_District_ERPCode",
                table: "RII_DISTRICT",
                column: "ERPCode",
                unique: true,
                filter: "[ERPCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_District_IsDeleted",
                table: "RII_DISTRICT",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_District_Name",
                table: "RII_DISTRICT",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_RII_DISTRICT_CreatedBy",
                table: "RII_DISTRICT",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_DISTRICT_DeletedBy",
                table: "RII_DISTRICT",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_DISTRICT_UpdatedBy",
                table: "RII_DISTRICT",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentSerialType_CustomerTypeId",
                table: "RII_DOCUMENT_SERIAL_TYPE",
                column: "CustomerTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentSerialType_IsDeleted",
                table: "RII_DOCUMENT_SERIAL_TYPE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentSerialType_RuleType",
                table: "RII_DOCUMENT_SERIAL_TYPE",
                column: "RuleType");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentSerialType_SalesRepId",
                table: "RII_DOCUMENT_SERIAL_TYPE",
                column: "SalesRepId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_DOCUMENT_SERIAL_TYPE_CreatedBy",
                table: "RII_DOCUMENT_SERIAL_TYPE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_DOCUMENT_SERIAL_TYPE_DeletedBy",
                table: "RII_DOCUMENT_SERIAL_TYPE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_DOCUMENT_SERIAL_TYPE_UpdatedBy",
                table: "RII_DOCUMENT_SERIAL_TYPE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PASSWORD_RESET_REQUEST_CreatedByUserId",
                table: "RII_PASSWORD_RESET_REQUEST",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PASSWORD_RESET_REQUEST_DeletedByUserId",
                table: "RII_PASSWORD_RESET_REQUEST",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PASSWORD_RESET_REQUEST_UpdatedByUserId",
                table: "RII_PASSWORD_RESET_REQUEST",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PASSWORD_RESET_REQUEST_UserId",
                table: "RII_PASSWORD_RESET_REQUEST",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentType_CreatedDate",
                table: "RII_PAYMENT_TYPE",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentType_IsDeleted",
                table: "RII_PAYMENT_TYPE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentType_Name",
                table: "RII_PAYMENT_TYPE",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PAYMENT_TYPE_CreatedBy",
                table: "RII_PAYMENT_TYPE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PAYMENT_TYPE_DeletedBy",
                table: "RII_PAYMENT_TYPE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PAYMENT_TYPE_UpdatedBy",
                table: "RII_PAYMENT_TYPE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PricingRuleHeader_CustomerId",
                table: "RII_PRICING_RULE_HEADER",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_PricingRuleHeader_IsActive",
                table: "RII_PRICING_RULE_HEADER",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_PricingRuleHeader_IsDeleted",
                table: "RII_PRICING_RULE_HEADER",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_PricingRuleHeader_RuleCode",
                table: "RII_PRICING_RULE_HEADER",
                column: "RuleCode");

            migrationBuilder.CreateIndex(
                name: "IX_PricingRuleHeader_RuleType",
                table: "RII_PRICING_RULE_HEADER",
                column: "RuleType");

            migrationBuilder.CreateIndex(
                name: "IX_PricingRuleHeader_ValidFrom",
                table: "RII_PRICING_RULE_HEADER",
                column: "ValidFrom");

            migrationBuilder.CreateIndex(
                name: "IX_PricingRuleHeader_ValidTo",
                table: "RII_PRICING_RULE_HEADER",
                column: "ValidTo");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PRICING_RULE_HEADER_CreatedBy",
                table: "RII_PRICING_RULE_HEADER",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PRICING_RULE_HEADER_DeletedBy",
                table: "RII_PRICING_RULE_HEADER",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PRICING_RULE_HEADER_UpdatedBy",
                table: "RII_PRICING_RULE_HEADER",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PricingRuleLine_IsDeleted",
                table: "RII_PRICING_RULE_LINE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_PricingRuleLine_PricingRuleHeaderId",
                table: "RII_PRICING_RULE_LINE",
                column: "PricingRuleHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_PricingRuleLine_StokCode",
                table: "RII_PRICING_RULE_LINE",
                column: "StokCode");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PRICING_RULE_LINE_CreatedBy",
                table: "RII_PRICING_RULE_LINE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PRICING_RULE_LINE_DeletedBy",
                table: "RII_PRICING_RULE_LINE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PRICING_RULE_LINE_UpdatedBy",
                table: "RII_PRICING_RULE_LINE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PricingRuleSalesman_Header_Salesman_Unique",
                table: "RII_PRICING_RULE_SALESMAN",
                columns: new[] { "PricingRuleHeaderId", "SalesmanId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PricingRuleSalesman_IsDeleted",
                table: "RII_PRICING_RULE_SALESMAN",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_PricingRuleSalesman_PricingRuleHeaderId",
                table: "RII_PRICING_RULE_SALESMAN",
                column: "PricingRuleHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_PricingRuleSalesman_SalesmanId",
                table: "RII_PRICING_RULE_SALESMAN",
                column: "SalesmanId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PRICING_RULE_SALESMAN_CreatedBy",
                table: "RII_PRICING_RULE_SALESMAN",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PRICING_RULE_SALESMAN_DeletedBy",
                table: "RII_PRICING_RULE_SALESMAN",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PRICING_RULE_SALESMAN_UpdatedBy",
                table: "RII_PRICING_RULE_SALESMAN",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPricing_CreatedDate",
                table: "RII_PRODUCT_PRICING",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPricing_ErpGroupCode",
                table: "RII_PRODUCT_PRICING",
                column: "ErpGroupCode");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPricing_ErpProductCode",
                table: "RII_PRODUCT_PRICING",
                column: "ErpProductCode");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPricing_ErpProductCode_ErpGroupCode",
                table: "RII_PRODUCT_PRICING",
                columns: new[] { "ErpProductCode", "ErpGroupCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductPricing_IsDeleted",
                table: "RII_PRODUCT_PRICING",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PRODUCT_PRICING_CreatedBy",
                table: "RII_PRODUCT_PRICING",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PRODUCT_PRICING_DeletedBy",
                table: "RII_PRODUCT_PRICING",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PRODUCT_PRICING_UpdatedBy",
                table: "RII_PRODUCT_PRICING",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPricingGroupBy_ErpGroupCode",
                table: "RII_PRODUCT_PRICING_GROUP_BY",
                column: "ErpGroupCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductPricingGroupBy_IsDeleted",
                table: "RII_PRODUCT_PRICING_GROUP_BY",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PRODUCT_PRICING_GROUP_BY_CreatedBy",
                table: "RII_PRODUCT_PRICING_GROUP_BY",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PRODUCT_PRICING_GROUP_BY_DeletedBy",
                table: "RII_PRODUCT_PRICING_GROUP_BY",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PRODUCT_PRICING_GROUP_BY_UpdatedBy",
                table: "RII_PRODUCT_PRICING_GROUP_BY",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Quotation_ActivityId",
                table: "RII_QUOTATION",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Quotation_ApprovalDate",
                table: "RII_QUOTATION",
                column: "ApprovalDate");

            migrationBuilder.CreateIndex(
                name: "IX_Quotation_ApprovalStatus",
                table: "RII_QUOTATION",
                column: "ApprovalStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Quotation_ApprovedByUserId",
                table: "RII_QUOTATION",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Quotation_ContactId",
                table: "RII_QUOTATION",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Quotation_DeliveryDate",
                table: "RII_QUOTATION",
                column: "DeliveryDate");

            migrationBuilder.CreateIndex(
                name: "IX_Quotation_IsCompleted",
                table: "RII_QUOTATION",
                column: "IsCompleted");

            migrationBuilder.CreateIndex(
                name: "IX_Quotation_IsDeleted",
                table: "RII_QUOTATION",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Quotation_OfferDate",
                table: "RII_QUOTATION",
                column: "OfferDate");

            migrationBuilder.CreateIndex(
                name: "IX_Quotation_OfferNo",
                table: "RII_QUOTATION",
                column: "OfferNo");

            migrationBuilder.CreateIndex(
                name: "IX_Quotation_PaymentTypeId",
                table: "RII_QUOTATION",
                column: "PaymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Quotation_PotentialCustomerId",
                table: "RII_QUOTATION",
                column: "PotentialCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Quotation_RepresentativeId",
                table: "RII_QUOTATION",
                column: "RepresentativeId");

            migrationBuilder.CreateIndex(
                name: "IX_Quotation_ShippingAddressId",
                table: "RII_QUOTATION",
                column: "ShippingAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Quotation_Status",
                table: "RII_QUOTATION",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Quotation_ValidUntil",
                table: "RII_QUOTATION",
                column: "ValidUntil");

            migrationBuilder.CreateIndex(
                name: "IX_Quotation_Year",
                table: "RII_QUOTATION",
                column: "Year");

            migrationBuilder.CreateIndex(
                name: "IX_RII_QUOTATION_CreatedBy",
                table: "RII_QUOTATION",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_QUOTATION_DeletedBy",
                table: "RII_QUOTATION",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_QUOTATION_UpdatedBy",
                table: "RII_QUOTATION",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationExchangeRate_Currency",
                table: "RII_QUOTATION_EXCHANGE_RATE",
                column: "Currency");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationExchangeRate_ExchangeRateDate",
                table: "RII_QUOTATION_EXCHANGE_RATE",
                column: "ExchangeRateDate");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationExchangeRate_IsDeleted",
                table: "RII_QUOTATION_EXCHANGE_RATE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationExchangeRate_IsOfficial",
                table: "RII_QUOTATION_EXCHANGE_RATE",
                column: "IsOfficial");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationExchangeRate_QuotationId",
                table: "RII_QUOTATION_EXCHANGE_RATE",
                column: "QuotationId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_QUOTATION_EXCHANGE_RATE_CreatedBy",
                table: "RII_QUOTATION_EXCHANGE_RATE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_QUOTATION_EXCHANGE_RATE_DeletedBy",
                table: "RII_QUOTATION_EXCHANGE_RATE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_QUOTATION_EXCHANGE_RATE_UpdatedBy",
                table: "RII_QUOTATION_EXCHANGE_RATE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationLine_IsDeleted",
                table: "RII_QUOTATION_LINE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationLine_PricingRuleHeaderId",
                table: "RII_QUOTATION_LINE",
                column: "PricingRuleHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationLine_ProductCode",
                table: "RII_QUOTATION_LINE",
                column: "ProductCode");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationLine_QuotationId",
                table: "RII_QUOTATION_LINE",
                column: "QuotationId");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationLine_RelatedStockId",
                table: "RII_QUOTATION_LINE",
                column: "RelatedStockId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_QUOTATION_LINE_CreatedBy",
                table: "RII_QUOTATION_LINE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_QUOTATION_LINE_DeletedBy",
                table: "RII_QUOTATION_LINE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_QUOTATION_LINE_UpdatedBy",
                table: "RII_QUOTATION_LINE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SHIPPING_ADDRESS_CreatedBy",
                table: "RII_SHIPPING_ADDRESS",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SHIPPING_ADDRESS_DeletedBy",
                table: "RII_SHIPPING_ADDRESS",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SHIPPING_ADDRESS_UpdatedBy",
                table: "RII_SHIPPING_ADDRESS",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ShippingAddress_CityId",
                table: "RII_SHIPPING_ADDRESS",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_ShippingAddress_CountryId",
                table: "RII_SHIPPING_ADDRESS",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_ShippingAddress_CustomerId",
                table: "RII_SHIPPING_ADDRESS",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ShippingAddress_DistrictId",
                table: "RII_SHIPPING_ADDRESS",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_ShippingAddress_IsDeleted",
                table: "RII_SHIPPING_ADDRESS",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_RII_STOCK_CreatedBy",
                table: "RII_STOCK",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_STOCK_DeletedBy",
                table: "RII_STOCK",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_STOCK_UpdatedBy",
                table: "RII_STOCK",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_ErpStockCode",
                table: "RII_STOCK",
                column: "ErpStockCode");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_IsDeleted",
                table: "RII_STOCK",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_StockName",
                table: "RII_STOCK",
                column: "StockName");

            migrationBuilder.CreateIndex(
                name: "IX_RII_STOCK_DETAIL_CreatedBy",
                table: "RII_STOCK_DETAIL",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_STOCK_DETAIL_DeletedBy",
                table: "RII_STOCK_DETAIL",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_STOCK_DETAIL_UpdatedBy",
                table: "RII_STOCK_DETAIL",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StockDetail_IsDeleted",
                table: "RII_STOCK_DETAIL",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_StockDetail_StockId",
                table: "RII_STOCK_DETAIL",
                column: "StockId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RII_STOCK_IMAGE_CreatedBy",
                table: "RII_STOCK_IMAGE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_STOCK_IMAGE_DeletedBy",
                table: "RII_STOCK_IMAGE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_STOCK_IMAGE_UpdatedBy",
                table: "RII_STOCK_IMAGE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StockImage_IsDeleted",
                table: "RII_STOCK_IMAGE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_StockImage_StockId",
                table: "RII_STOCK_IMAGE",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_STOCK_RELATION_CreatedBy",
                table: "RII_STOCK_RELATION",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_STOCK_RELATION_DeletedBy",
                table: "RII_STOCK_RELATION",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_STOCK_RELATION_UpdatedBy",
                table: "RII_STOCK_RELATION",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StockRelation_IsDeleted",
                table: "RII_STOCK_RELATION",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_StockRelation_RelatedStockId",
                table: "RII_STOCK_RELATION",
                column: "RelatedStockId");

            migrationBuilder.CreateIndex(
                name: "IX_StockRelation_StockId",
                table: "RII_STOCK_RELATION",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_StockRelation_StockId_RelatedStockId",
                table: "RII_STOCK_RELATION",
                columns: new[] { "StockId", "RelatedStockId" });

            migrationBuilder.CreateIndex(
                name: "IX_RII_TITLE_CreatedBy",
                table: "RII_TITLE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_TITLE_DeletedBy",
                table: "RII_TITLE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_TITLE_UpdatedBy",
                table: "RII_TITLE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Title_IsDeleted",
                table: "RII_TITLE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Title_TitleName",
                table: "RII_TITLE",
                column: "TitleName");

            migrationBuilder.CreateIndex(
                name: "IX_RII_USER_AUTHORITY_CreatedBy",
                table: "RII_USER_AUTHORITY",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_USER_AUTHORITY_DeletedBy",
                table: "RII_USER_AUTHORITY",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_USER_AUTHORITY_UpdatedBy",
                table: "RII_USER_AUTHORITY",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserAuthority_IsDeleted",
                table: "RII_USER_AUTHORITY",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_UserAuthority_Title",
                table: "RII_USER_AUTHORITY",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RII_USER_DETAIL_CreatedBy",
                table: "RII_USER_DETAIL",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_USER_DETAIL_DeletedBy",
                table: "RII_USER_DETAIL",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_USER_DETAIL_UpdatedBy",
                table: "RII_USER_DETAIL",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserDetail_IsDeleted",
                table: "RII_USER_DETAIL",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_UserDetail_UserId",
                table: "RII_USER_DETAIL",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RII_USER_DISCOUNT_LIMIT_CreatedBy",
                table: "RII_USER_DISCOUNT_LIMIT",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_USER_DISCOUNT_LIMIT_DeletedBy",
                table: "RII_USER_DISCOUNT_LIMIT",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_USER_DISCOUNT_LIMIT_UpdatedBy",
                table: "RII_USER_DISCOUNT_LIMIT",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserDiscountLimit_ErpProductGroupCode",
                table: "RII_USER_DISCOUNT_LIMIT",
                column: "ErpProductGroupCode");

            migrationBuilder.CreateIndex(
                name: "IX_UserDiscountLimit_IsDeleted",
                table: "RII_USER_DISCOUNT_LIMIT",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_UserDiscountLimit_SalespersonId",
                table: "RII_USER_DISCOUNT_LIMIT",
                column: "SalespersonId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDiscountLimit_SalespersonId_ErpProductGroupCode",
                table: "RII_USER_DISCOUNT_LIMIT",
                columns: new[] { "SalespersonId", "ErpProductGroupCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RII_USER_HIERARCHY_CreatedBy",
                table: "RII_USER_HIERARCHY",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_USER_HIERARCHY_DeletedBy",
                table: "RII_USER_HIERARCHY",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_USER_HIERARCHY_UpdatedBy",
                table: "RII_USER_HIERARCHY",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserHierarchy_GeneralManagerId",
                table: "RII_USER_HIERARCHY",
                column: "GeneralManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserHierarchy_IsActive",
                table: "RII_USER_HIERARCHY",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_UserHierarchy_IsDeleted",
                table: "RII_USER_HIERARCHY",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_UserHierarchy_ManagerId",
                table: "RII_USER_HIERARCHY",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserHierarchy_SalespersonId",
                table: "RII_USER_HIERARCHY",
                column: "SalespersonId");

            migrationBuilder.CreateIndex(
                name: "IX_UserHierarchy_SalespersonId_IsActive",
                table: "RII_USER_HIERARCHY",
                columns: new[] { "SalespersonId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_RII_USER_SESSION_CreatedBy",
                table: "RII_USER_SESSION",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_USER_SESSION_DeletedBy",
                table: "RII_USER_SESSION",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_USER_SESSION_UpdatedBy",
                table: "RII_USER_SESSION",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserSession_IsDeleted",
                table: "RII_USER_SESSION",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_UserSession_RevokedAt",
                table: "RII_USER_SESSION",
                column: "RevokedAt");

            migrationBuilder.CreateIndex(
                name: "IX_UserSession_SessionId",
                table: "RII_USER_SESSION",
                column: "SessionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSession_UserId",
                table: "RII_USER_SESSION",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_USERS_CreatedBy",
                table: "RII_USERS",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_USERS_DeletedBy",
                table: "RII_USERS",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_USERS_RoleId",
                table: "RII_USERS",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_USERS_UpdatedBy",
                table: "RII_USERS",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "RII_USERS",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_IsDeleted",
                table: "RII_USERS",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "RII_USERS",
                column: "Username",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RII_ACTIVITY_RII_ACTIVITY_TYPE_ActivityTypeId",
                table: "RII_ACTIVITY",
                column: "ActivityTypeId",
                principalTable: "RII_ACTIVITY_TYPE",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_ACTIVITY_RII_CONTACT_ContactId",
                table: "RII_ACTIVITY",
                column: "ContactId",
                principalTable: "RII_CONTACT",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_ACTIVITY_RII_CUSTOMER_PotentialCustomerId",
                table: "RII_ACTIVITY",
                column: "PotentialCustomerId",
                principalTable: "RII_CUSTOMER",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_ACTIVITY_RII_USERS_AssignedUserId",
                table: "RII_ACTIVITY",
                column: "AssignedUserId",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_ACTIVITY_RII_USERS_CreatedBy",
                table: "RII_ACTIVITY",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_ACTIVITY_RII_USERS_DeletedBy",
                table: "RII_ACTIVITY",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_ACTIVITY_RII_USERS_UpdatedBy",
                table: "RII_ACTIVITY",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_ACTIVITY_TYPE_RII_USERS_CreatedBy",
                table: "RII_ACTIVITY_TYPE",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_ACTIVITY_TYPE_RII_USERS_DeletedBy",
                table: "RII_ACTIVITY_TYPE",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_ACTIVITY_TYPE_RII_USERS_UpdatedBy",
                table: "RII_ACTIVITY_TYPE",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_APPROVAL_AUTHORITY_RII_USERS_CreatedBy",
                table: "RII_APPROVAL_AUTHORITY",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_APPROVAL_AUTHORITY_RII_USERS_DeletedBy",
                table: "RII_APPROVAL_AUTHORITY",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_APPROVAL_AUTHORITY_RII_USERS_UpdatedBy",
                table: "RII_APPROVAL_AUTHORITY",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_APPROVAL_AUTHORITY_RII_USERS_UserId",
                table: "RII_APPROVAL_AUTHORITY",
                column: "UserId",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_APPROVAL_QUEUE_RII_QUOTATION_LINE_QuotationLineId",
                table: "RII_APPROVAL_QUEUE",
                column: "QuotationLineId",
                principalTable: "RII_QUOTATION_LINE",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_APPROVAL_QUEUE_RII_QUOTATION_QuotationId",
                table: "RII_APPROVAL_QUEUE",
                column: "QuotationId",
                principalTable: "RII_QUOTATION",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_APPROVAL_QUEUE_RII_USERS_AssignedToUserId",
                table: "RII_APPROVAL_QUEUE",
                column: "AssignedToUserId",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_APPROVAL_QUEUE_RII_USERS_CreatedBy",
                table: "RII_APPROVAL_QUEUE",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_APPROVAL_QUEUE_RII_USERS_DeletedBy",
                table: "RII_APPROVAL_QUEUE",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_APPROVAL_QUEUE_RII_USERS_UpdatedBy",
                table: "RII_APPROVAL_QUEUE",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_APPROVAL_RULE_RII_USERS_CreatedBy",
                table: "RII_APPROVAL_RULE",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_APPROVAL_RULE_RII_USERS_DeletedBy",
                table: "RII_APPROVAL_RULE",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_APPROVAL_RULE_RII_USERS_UpdatedBy",
                table: "RII_APPROVAL_RULE",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_APPROVAL_TRANSACTION_RII_QUOTATION_DocumentId",
                table: "RII_APPROVAL_TRANSACTION",
                column: "DocumentId",
                principalTable: "RII_QUOTATION",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_APPROVAL_TRANSACTION_RII_QUOTATION_LINE_LineId",
                table: "RII_APPROVAL_TRANSACTION",
                column: "LineId",
                principalTable: "RII_QUOTATION_LINE",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_APPROVAL_TRANSACTION_RII_USERS_ApprovedByUserId",
                table: "RII_APPROVAL_TRANSACTION",
                column: "ApprovedByUserId",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_APPROVAL_TRANSACTION_RII_USERS_CreatedBy",
                table: "RII_APPROVAL_TRANSACTION",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_APPROVAL_TRANSACTION_RII_USERS_DeletedBy",
                table: "RII_APPROVAL_TRANSACTION",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_APPROVAL_TRANSACTION_RII_USERS_UpdatedBy",
                table: "RII_APPROVAL_TRANSACTION",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_APPROVAL_WORKFLOW_RII_CUSTOMER_TYPE_CustomerTypeId",
                table: "RII_APPROVAL_WORKFLOW",
                column: "CustomerTypeId",
                principalTable: "RII_CUSTOMER_TYPE",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_APPROVAL_WORKFLOW_RII_USERS_CreatedBy",
                table: "RII_APPROVAL_WORKFLOW",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_APPROVAL_WORKFLOW_RII_USERS_DeletedBy",
                table: "RII_APPROVAL_WORKFLOW",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_APPROVAL_WORKFLOW_RII_USERS_UpdatedBy",
                table: "RII_APPROVAL_WORKFLOW",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_APPROVAL_WORKFLOW_RII_USERS_UserId",
                table: "RII_APPROVAL_WORKFLOW",
                column: "UserId",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_CITY_RII_COUNTRY_CountryId",
                table: "RII_CITY",
                column: "CountryId",
                principalTable: "RII_COUNTRY",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RII_CITY_RII_USERS_CreatedBy",
                table: "RII_CITY",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_CITY_RII_USERS_DeletedBy",
                table: "RII_CITY",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_CITY_RII_USERS_UpdatedBy",
                table: "RII_CITY",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_CONTACT_RII_COUNTRY_CountryId",
                table: "RII_CONTACT",
                column: "CountryId",
                principalTable: "RII_COUNTRY",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_CONTACT_RII_CUSTOMER_CustomerId",
                table: "RII_CONTACT",
                column: "CustomerId",
                principalTable: "RII_CUSTOMER",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_CONTACT_RII_TITLE_TitleId",
                table: "RII_CONTACT",
                column: "TitleId",
                principalTable: "RII_TITLE",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RII_CONTACT_RII_USERS_CreatedBy",
                table: "RII_CONTACT",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_CONTACT_RII_USERS_DeletedBy",
                table: "RII_CONTACT",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_CONTACT_RII_USERS_UpdatedBy",
                table: "RII_CONTACT",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_COUNTRY_RII_USERS_CreatedBy",
                table: "RII_COUNTRY",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_COUNTRY_RII_USERS_DeletedBy",
                table: "RII_COUNTRY",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_COUNTRY_RII_USERS_UpdatedBy",
                table: "RII_COUNTRY",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_CUSTOMER_RII_CUSTOMER_TYPE_CustomerTypeId",
                table: "RII_CUSTOMER",
                column: "CustomerTypeId",
                principalTable: "RII_CUSTOMER_TYPE",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_RII_CUSTOMER_RII_DISTRICT_DistrictId",
                table: "RII_CUSTOMER",
                column: "DistrictId",
                principalTable: "RII_DISTRICT",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_CUSTOMER_RII_USERS_APPROVED_BY_USER_ID",
                table: "RII_CUSTOMER",
                column: "APPROVED_BY_USER_ID",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_CUSTOMER_RII_USERS_CreatedBy",
                table: "RII_CUSTOMER",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_CUSTOMER_RII_USERS_DeletedBy",
                table: "RII_CUSTOMER",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_CUSTOMER_RII_USERS_UpdatedBy",
                table: "RII_CUSTOMER",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_CUSTOMER_TYPE_RII_USERS_CreatedBy",
                table: "RII_CUSTOMER_TYPE",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_CUSTOMER_TYPE_RII_USERS_DeletedBy",
                table: "RII_CUSTOMER_TYPE",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_CUSTOMER_TYPE_RII_USERS_UpdatedBy",
                table: "RII_CUSTOMER_TYPE",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_DISTRICT_RII_USERS_CreatedBy",
                table: "RII_DISTRICT",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_DISTRICT_RII_USERS_DeletedBy",
                table: "RII_DISTRICT",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_DISTRICT_RII_USERS_UpdatedBy",
                table: "RII_DISTRICT",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_DOCUMENT_SERIAL_TYPE_RII_USERS_CreatedBy",
                table: "RII_DOCUMENT_SERIAL_TYPE",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_DOCUMENT_SERIAL_TYPE_RII_USERS_DeletedBy",
                table: "RII_DOCUMENT_SERIAL_TYPE",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_DOCUMENT_SERIAL_TYPE_RII_USERS_SalesRepId",
                table: "RII_DOCUMENT_SERIAL_TYPE",
                column: "SalesRepId",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_DOCUMENT_SERIAL_TYPE_RII_USERS_UpdatedBy",
                table: "RII_DOCUMENT_SERIAL_TYPE",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PASSWORD_RESET_REQUEST_RII_USERS_CreatedByUserId",
                table: "RII_PASSWORD_RESET_REQUEST",
                column: "CreatedByUserId",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PASSWORD_RESET_REQUEST_RII_USERS_DeletedByUserId",
                table: "RII_PASSWORD_RESET_REQUEST",
                column: "DeletedByUserId",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PASSWORD_RESET_REQUEST_RII_USERS_UpdatedByUserId",
                table: "RII_PASSWORD_RESET_REQUEST",
                column: "UpdatedByUserId",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PASSWORD_RESET_REQUEST_RII_USERS_UserId",
                table: "RII_PASSWORD_RESET_REQUEST",
                column: "UserId",
                principalTable: "RII_USERS",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PAYMENT_TYPE_RII_USERS_CreatedBy",
                table: "RII_PAYMENT_TYPE",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PAYMENT_TYPE_RII_USERS_DeletedBy",
                table: "RII_PAYMENT_TYPE",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PAYMENT_TYPE_RII_USERS_UpdatedBy",
                table: "RII_PAYMENT_TYPE",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PRICING_RULE_HEADER_RII_USERS_CreatedBy",
                table: "RII_PRICING_RULE_HEADER",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PRICING_RULE_HEADER_RII_USERS_DeletedBy",
                table: "RII_PRICING_RULE_HEADER",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PRICING_RULE_HEADER_RII_USERS_UpdatedBy",
                table: "RII_PRICING_RULE_HEADER",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PRICING_RULE_LINE_RII_USERS_CreatedBy",
                table: "RII_PRICING_RULE_LINE",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PRICING_RULE_LINE_RII_USERS_DeletedBy",
                table: "RII_PRICING_RULE_LINE",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PRICING_RULE_LINE_RII_USERS_UpdatedBy",
                table: "RII_PRICING_RULE_LINE",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PRICING_RULE_SALESMAN_RII_USERS_CreatedBy",
                table: "RII_PRICING_RULE_SALESMAN",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PRICING_RULE_SALESMAN_RII_USERS_DeletedBy",
                table: "RII_PRICING_RULE_SALESMAN",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PRICING_RULE_SALESMAN_RII_USERS_SalesmanId",
                table: "RII_PRICING_RULE_SALESMAN",
                column: "SalesmanId",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PRICING_RULE_SALESMAN_RII_USERS_UpdatedBy",
                table: "RII_PRICING_RULE_SALESMAN",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PRODUCT_PRICING_RII_USERS_CreatedBy",
                table: "RII_PRODUCT_PRICING",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PRODUCT_PRICING_RII_USERS_DeletedBy",
                table: "RII_PRODUCT_PRICING",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PRODUCT_PRICING_RII_USERS_UpdatedBy",
                table: "RII_PRODUCT_PRICING",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PRODUCT_PRICING_GROUP_BY_RII_USERS_CreatedBy",
                table: "RII_PRODUCT_PRICING_GROUP_BY",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PRODUCT_PRICING_GROUP_BY_RII_USERS_DeletedBy",
                table: "RII_PRODUCT_PRICING_GROUP_BY",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PRODUCT_PRICING_GROUP_BY_RII_USERS_UpdatedBy",
                table: "RII_PRODUCT_PRICING_GROUP_BY",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_QUOTATION_RII_SHIPPING_ADDRESS_ShippingAddressId",
                table: "RII_QUOTATION",
                column: "ShippingAddressId",
                principalTable: "RII_SHIPPING_ADDRESS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_QUOTATION_RII_USERS_ApprovedByUserId",
                table: "RII_QUOTATION",
                column: "ApprovedByUserId",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_QUOTATION_RII_USERS_CreatedBy",
                table: "RII_QUOTATION",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_QUOTATION_RII_USERS_DeletedBy",
                table: "RII_QUOTATION",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_QUOTATION_RII_USERS_RepresentativeId",
                table: "RII_QUOTATION",
                column: "RepresentativeId",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_QUOTATION_RII_USERS_UpdatedBy",
                table: "RII_QUOTATION",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_QUOTATION_EXCHANGE_RATE_RII_USERS_CreatedBy",
                table: "RII_QUOTATION_EXCHANGE_RATE",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_QUOTATION_EXCHANGE_RATE_RII_USERS_DeletedBy",
                table: "RII_QUOTATION_EXCHANGE_RATE",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_QUOTATION_EXCHANGE_RATE_RII_USERS_UpdatedBy",
                table: "RII_QUOTATION_EXCHANGE_RATE",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_QUOTATION_LINE_RII_STOCK_RelatedStockId",
                table: "RII_QUOTATION_LINE",
                column: "RelatedStockId",
                principalTable: "RII_STOCK",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_RII_QUOTATION_LINE_RII_USERS_CreatedBy",
                table: "RII_QUOTATION_LINE",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_QUOTATION_LINE_RII_USERS_DeletedBy",
                table: "RII_QUOTATION_LINE",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_QUOTATION_LINE_RII_USERS_UpdatedBy",
                table: "RII_QUOTATION_LINE",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_SHIPPING_ADDRESS_RII_USERS_CreatedBy",
                table: "RII_SHIPPING_ADDRESS",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_SHIPPING_ADDRESS_RII_USERS_DeletedBy",
                table: "RII_SHIPPING_ADDRESS",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_SHIPPING_ADDRESS_RII_USERS_UpdatedBy",
                table: "RII_SHIPPING_ADDRESS",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_STOCK_RII_USERS_CreatedBy",
                table: "RII_STOCK",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_STOCK_RII_USERS_DeletedBy",
                table: "RII_STOCK",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_STOCK_RII_USERS_UpdatedBy",
                table: "RII_STOCK",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_STOCK_DETAIL_RII_USERS_CreatedBy",
                table: "RII_STOCK_DETAIL",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_STOCK_DETAIL_RII_USERS_DeletedBy",
                table: "RII_STOCK_DETAIL",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_STOCK_DETAIL_RII_USERS_UpdatedBy",
                table: "RII_STOCK_DETAIL",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_STOCK_IMAGE_RII_USERS_CreatedBy",
                table: "RII_STOCK_IMAGE",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_STOCK_IMAGE_RII_USERS_DeletedBy",
                table: "RII_STOCK_IMAGE",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_STOCK_IMAGE_RII_USERS_UpdatedBy",
                table: "RII_STOCK_IMAGE",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_STOCK_RELATION_RII_USERS_CreatedBy",
                table: "RII_STOCK_RELATION",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_STOCK_RELATION_RII_USERS_DeletedBy",
                table: "RII_STOCK_RELATION",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_STOCK_RELATION_RII_USERS_UpdatedBy",
                table: "RII_STOCK_RELATION",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_TITLE_RII_USERS_CreatedBy",
                table: "RII_TITLE",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_TITLE_RII_USERS_DeletedBy",
                table: "RII_TITLE",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_TITLE_RII_USERS_UpdatedBy",
                table: "RII_TITLE",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_USER_AUTHORITY_RII_USERS_CreatedBy",
                table: "RII_USER_AUTHORITY",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_USER_AUTHORITY_RII_USERS_DeletedBy",
                table: "RII_USER_AUTHORITY",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_USER_AUTHORITY_RII_USERS_UpdatedBy",
                table: "RII_USER_AUTHORITY",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RII_USER_AUTHORITY_RII_USERS_CreatedBy",
                table: "RII_USER_AUTHORITY");

            migrationBuilder.DropForeignKey(
                name: "FK_RII_USER_AUTHORITY_RII_USERS_DeletedBy",
                table: "RII_USER_AUTHORITY");

            migrationBuilder.DropForeignKey(
                name: "FK_RII_USER_AUTHORITY_RII_USERS_UpdatedBy",
                table: "RII_USER_AUTHORITY");

            migrationBuilder.DropTable(
                name: "RII_APPROVAL_QUEUE");

            migrationBuilder.DropTable(
                name: "RII_APPROVAL_RULE");

            migrationBuilder.DropTable(
                name: "RII_APPROVAL_TRANSACTION");

            migrationBuilder.DropTable(
                name: "RII_APPROVAL_WORKFLOW");

            migrationBuilder.DropTable(
                name: "RII_DOCUMENT_SERIAL_TYPE");

            migrationBuilder.DropTable(
                name: "RII_PASSWORD_RESET_REQUEST");

            migrationBuilder.DropTable(
                name: "RII_PRICING_RULE_LINE");

            migrationBuilder.DropTable(
                name: "RII_PRICING_RULE_SALESMAN");

            migrationBuilder.DropTable(
                name: "RII_PRODUCT_PRICING");

            migrationBuilder.DropTable(
                name: "RII_PRODUCT_PRICING_GROUP_BY");

            migrationBuilder.DropTable(
                name: "RII_QUOTATION_EXCHANGE_RATE");

            migrationBuilder.DropTable(
                name: "RII_STOCK_DETAIL");

            migrationBuilder.DropTable(
                name: "RII_STOCK_IMAGE");

            migrationBuilder.DropTable(
                name: "RII_STOCK_RELATION");

            migrationBuilder.DropTable(
                name: "RII_USER_DETAIL");

            migrationBuilder.DropTable(
                name: "RII_USER_DISCOUNT_LIMIT");

            migrationBuilder.DropTable(
                name: "RII_USER_HIERARCHY");

            migrationBuilder.DropTable(
                name: "RII_USER_SESSION");

            migrationBuilder.DropTable(
                name: "RII_APPROVAL_AUTHORITY");

            migrationBuilder.DropTable(
                name: "RII_QUOTATION_LINE");

            migrationBuilder.DropTable(
                name: "RII_PRICING_RULE_HEADER");

            migrationBuilder.DropTable(
                name: "RII_QUOTATION");

            migrationBuilder.DropTable(
                name: "RII_STOCK");

            migrationBuilder.DropTable(
                name: "RII_ACTIVITY");

            migrationBuilder.DropTable(
                name: "RII_PAYMENT_TYPE");

            migrationBuilder.DropTable(
                name: "RII_SHIPPING_ADDRESS");

            migrationBuilder.DropTable(
                name: "RII_ACTIVITY_TYPE");

            migrationBuilder.DropTable(
                name: "RII_CONTACT");

            migrationBuilder.DropTable(
                name: "RII_CUSTOMER");

            migrationBuilder.DropTable(
                name: "RII_TITLE");

            migrationBuilder.DropTable(
                name: "RII_CUSTOMER_TYPE");

            migrationBuilder.DropTable(
                name: "RII_DISTRICT");

            migrationBuilder.DropTable(
                name: "RII_CITY");

            migrationBuilder.DropTable(
                name: "RII_COUNTRY");

            migrationBuilder.DropTable(
                name: "RII_USERS");

            migrationBuilder.DropTable(
                name: "RII_USER_AUTHORITY");
        }
    }
}
