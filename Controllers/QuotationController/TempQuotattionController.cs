using crm_api.DTOs;
using crm_api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace crm_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TempQuotattionController : ControllerBase
    {
        private readonly ITempQuotattionService _tempQuotattionService;

        public TempQuotattionController(ITempQuotattionService tempQuotattionService)
        {
            _tempQuotattionService = tempQuotattionService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request)
        {
            var result = await _tempQuotattionService.GetAllAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _tempQuotattionService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TempQuotattionCreateDto dto)
        {
            var result = await _tempQuotattionService.CreateAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] TempQuotattionUpdateDto dto)
        {
            var result = await _tempQuotattionService.UpdateAsync(id, dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("{id}/set-approved")]
        public async Task<IActionResult> SetApproved(long id)
        {
            var result = await _tempQuotattionService.SetApprovedAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _tempQuotattionService.DeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{tempQuotattionId}/lines")]
        public async Task<IActionResult> GetLinesByHeaderId(long tempQuotattionId)
        {
            var result = await _tempQuotattionService.GetLinesByHeaderIdAsync(tempQuotattionId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("lines/{lineId}")]
        public async Task<IActionResult> GetLineById(long lineId)
        {
            var result = await _tempQuotattionService.GetLineByIdAsync(lineId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("lines")]
        public async Task<IActionResult> CreateLine([FromBody] TempQuotattionLineCreateDto dto)
        {
            var result = await _tempQuotattionService.CreateLineAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("lines/bulk")]
        public async Task<IActionResult> CreateLines([FromBody] List<TempQuotattionLineCreateDto> dtos)
        {
            var result = await _tempQuotattionService.CreateLinesAsync(dtos);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("lines/{lineId}")]
        public async Task<IActionResult> UpdateLine(long lineId, [FromBody] TempQuotattionLineUpdateDto dto)
        {
            var result = await _tempQuotattionService.UpdateLineAsync(lineId, dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("lines/{lineId}")]
        public async Task<IActionResult> DeleteLine(long lineId)
        {
            var result = await _tempQuotattionService.DeleteLineAsync(lineId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{tempQuotattionId}/exchange-lines")]
        public async Task<IActionResult> GetExchangeLinesByHeaderId(long tempQuotattionId)
        {
            var result = await _tempQuotattionService.GetExchangeLinesByHeaderIdAsync(tempQuotattionId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("exchange-lines/{exchangeLineId}")]
        public async Task<IActionResult> GetExchangeLineById(long exchangeLineId)
        {
            var result = await _tempQuotattionService.GetExchangeLineByIdAsync(exchangeLineId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("exchange-lines")]
        public async Task<IActionResult> CreateExchangeLine([FromBody] TempQuotattionExchangeLineCreateDto dto)
        {
            var result = await _tempQuotattionService.CreateExchangeLineAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("exchange-lines/{exchangeLineId}")]
        public async Task<IActionResult> UpdateExchangeLine(long exchangeLineId, [FromBody] TempQuotattionExchangeLineUpdateDto dto)
        {
            var result = await _tempQuotattionService.UpdateExchangeLineAsync(exchangeLineId, dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("exchange-lines/{exchangeLineId}")]
        public async Task<IActionResult> DeleteExchangeLine(long exchangeLineId)
        {
            var result = await _tempQuotattionService.DeleteExchangeLineAsync(exchangeLineId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
