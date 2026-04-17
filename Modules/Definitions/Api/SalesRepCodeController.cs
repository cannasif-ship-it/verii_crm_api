using crm_api.Modules.Definitions.Application.Dtos;
using crm_api.Modules.Definitions.Application.Services;
using Hangfire;
using Infrastructure.BackgroundJobs.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace crm_api.Modules.Definitions.Api
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SalesRepCodeController : ControllerBase
    {
        private readonly ISalesRepCodeService _salesRepCodeService;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public SalesRepCodeController(
            ISalesRepCodeService salesRepCodeService,
            IBackgroundJobClient backgroundJobClient)
        {
            _salesRepCodeService = salesRepCodeService;
            _backgroundJobClient = backgroundJobClient;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request)
        {
            var result = await _salesRepCodeService.GetAllSalesRepCodesAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _salesRepCodeService.GetSalesRepCodeByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SalesRepCodeCreateDto dto)
        {
            var result = await _salesRepCodeService.CreateSalesRepCodeAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] SalesRepCodeUpdateDto dto)
        {
            var result = await _salesRepCodeService.UpdateSalesRepCodeAsync(id, dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _salesRepCodeService.DeleteSalesRepCodeAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("sync")]
        public IActionResult Sync()
        {
            var jobId = _backgroundJobClient.Enqueue<ISalesRepCodeSyncJob>(job => job.ExecuteAsync());

            return Ok(new ApiResponse<SalesRepCodeSyncTriggerResponseDto>
            {
                Success = true,
                Message = "Sales rep code sync queued successfully.",
                Data = new SalesRepCodeSyncTriggerResponseDto
                {
                    JobId = jobId,
                    Queue = "default",
                    EnqueuedAtUtc = DateTime.UtcNow
                },
                StatusCode = StatusCodes.Status200OK
            });
        }
    }
}
