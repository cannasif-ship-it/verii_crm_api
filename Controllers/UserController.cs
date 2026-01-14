using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using cms_webapi.DTOs;
using cms_webapi.Interfaces;

namespace cms_webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly ILocalizationService _localizationService;

        public UserController(IUserService service, ILocalizationService localizationService)
        {
            _service = service;
            _localizationService = localizationService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request)
        {
            var result = await _service.GetAllUsersAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _service.GetUserByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateUserDto dto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<UserDto>.ErrorResult(
                    _localizationService.GetLocalizedString("UserController.InvalidModelState"),
                    _localizationService.GetLocalizedString("UserController.InvalidModelStateExceptionMessage", ModelState?.ToString() ?? string.Empty),
                    400));
            }
            var result = await _service.CreateUserAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] UpdateUserDto dto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<UserDto>.ErrorResult(
                    _localizationService.GetLocalizedString("UserController.InvalidModelState"),
                    _localizationService.GetLocalizedString("UserController.InvalidModelStateExceptionMessage", ModelState?.ToString() ?? string.Empty),
                    400));
            }
            var result = await _service.UpdateUserAsync(id, dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _service.DeleteUserAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}