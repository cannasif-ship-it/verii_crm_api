using cms_webapi.DTOs;
using cms_webapi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace cms_webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DocumentSerialTypeController : ControllerBase
    {
        private readonly IDocumentSerialTypeService _documentSerialTypeService;

        public DocumentSerialTypeController(IDocumentSerialTypeService documentSerialTypeService)
        {
            _documentSerialTypeService = documentSerialTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request)
        {
            var result = await _documentSerialTypeService.GetAllDocumentSerialTypesAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _documentSerialTypeService.GetDocumentSerialTypeByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DocumentSerialTypeCreateDto createDto)
        {
            var result = await _documentSerialTypeService.CreateDocumentSerialTypeAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] DocumentSerialTypeUpdateDto updateDto)
        {
            var result = await _documentSerialTypeService.UpdateDocumentSerialTypeAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _documentSerialTypeService.DeleteDocumentSerialTypeAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
