using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using crm_api.DTOs;
using crm_api.Interfaces;

namespace crm_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserSessionController : ControllerBase
    {
        private readonly IUserSessionService _service;
        private readonly ILocalizationService _localizationService;

        public UserSessionController(IUserSessionService service, ILocalizationService localizationService)
        {
            _service = service;
            _localizationService = localizationService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request)
        {
            var result = await _service.GetAllSessionsAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _service.GetSessionByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateUserSessionDto dto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<UserSessionDto>.ErrorResult(
                    _localizationService.GetLocalizedString("UserSessionController.InvalidModelState"),
                    _localizationService.GetLocalizedString("UserSessionController.InvalidModelStateExceptionMessage", ModelState?.ToString() ?? string.Empty),
                    400));
            }
            var result = await _service.CreateSessionAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("{id}/revoke")]
        public async Task<IActionResult> Revoke(long id)
        {
            var result = await _service.RevokeSessionAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _service.DeleteSessionAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
