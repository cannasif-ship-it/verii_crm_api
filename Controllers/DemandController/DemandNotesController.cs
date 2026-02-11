using crm_api.DTOs;
using crm_api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace crm_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DemandNotesController : ControllerBase
    {
        private readonly IDemandNotesService _demandNotesService;

        public DemandNotesController(IDemandNotesService demandNotesService)
        {
            _demandNotesService = demandNotesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDemandNotes([FromQuery] PagedRequest request)
        {
            var result = await _demandNotesService.GetAllDemandNotesAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDemandNotesById(long id)
        {
            var result = await _demandNotesService.GetDemandNotesByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("by-demand/{demandId}")]
        public async Task<IActionResult> GetByDemandId(long demandId)
        {
            var result = await _demandNotesService.GetNotesByDemandIdAsync(demandId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDemandNotes([FromBody] CreateDemandNotesDto request)
        {
            var result = await _demandNotesService.CreateDemandNotesAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDemandNotes(long id, [FromBody] UpdateDemandNotesDto request)
        {
            var result = await _demandNotesService.UpdateDemandNotesAsync(id, request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("by-demand/{demandId}/notes-list")]
        public async Task<IActionResult> UpdateNotesListByDemandId(long demandId, [FromBody] UpdateDemandNotesListDto request)
        {
            var result = await _demandNotesService.UpdateNotesListByDemandIdAsync(demandId, request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDemandNotes(long id)
        {
            var result = await _demandNotesService.DeleteDemandNotesAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
