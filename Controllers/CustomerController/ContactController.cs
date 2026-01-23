using crm_api.DTOs;
using crm_api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace crm_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;
        private readonly ILocalizationService _localizationService;

        public ContactController(IContactService contactService, ILocalizationService localizationService)
        {
            _contactService = contactService;
            _localizationService = localizationService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request)
        {
            var result = await _contactService.GetAllContactsAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _contactService.GetContactByIdAsync(id);           
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateContactDto createContactDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<object>.ErrorResult(_localizationService.GetLocalizedString("ValidationError"), "ValidationFailed", 400));
            }

            var result = await _contactService.CreateContactAsync(createContactDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] UpdateContactDto updateContactDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<object>.ErrorResult(_localizationService.GetLocalizedString("ValidationError"), "ValidationFailed", 400));
            }

            var result = await _contactService.UpdateContactAsync(id, updateContactDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _contactService.DeleteContactAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
