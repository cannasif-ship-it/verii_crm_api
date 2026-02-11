using crm_api.DTOs;
using crm_api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace crm_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class QuotationNotesController : ControllerBase
    {
        private readonly IQuotationNotesService _quotationNotesService;

        public QuotationNotesController(IQuotationNotesService quotationNotesService)
        {
            _quotationNotesService = quotationNotesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetQuotationNotes([FromQuery] PagedRequest request)
        {
            var result = await _quotationNotesService.GetAllQuotationNotesAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuotationNotesById(long id)
        {
            var result = await _quotationNotesService.GetQuotationNotesByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("by-quotation/{quotationId}")]
        public async Task<IActionResult> GetByQuotationId(long quotationId)
        {
            var result = await _quotationNotesService.GetNotesByQuotationIdAsync(quotationId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuotationNotes([FromBody] CreateQuotationNotesDto createQuotationNotesDto)
        {
            var result = await _quotationNotesService.CreateQuotationNotesAsync(createQuotationNotesDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuotationNotes(long id, [FromBody] UpdateQuotationNotesDto updateQuotationNotesDto)
        {
            var result = await _quotationNotesService.UpdateQuotationNotesAsync(id, updateQuotationNotesDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("by-quotation/{quotationId}/notes-list")]
        public async Task<IActionResult> UpdateNotesListByQuotationId(long quotationId, [FromBody] UpdateQuotationNotesListDto request)
        {
            var result = await _quotationNotesService.UpdateNotesListByQuotationIdAsync(quotationId, request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuotationNotes(long id)
        {
            var result = await _quotationNotesService.DeleteQuotationNotesAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
