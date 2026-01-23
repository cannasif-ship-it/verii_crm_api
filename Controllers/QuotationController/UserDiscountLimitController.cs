using crm_api.DTOs;
using crm_api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace crm_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserDiscountLimitController : ControllerBase
    {
        private readonly IUserDiscountLimitService _userDiscountLimitService;
        private readonly ILocalizationService _localizationService;

        public UserDiscountLimitController(IUserDiscountLimitService userDiscountLimitService, ILocalizationService localizationService)
        {
            _userDiscountLimitService = userDiscountLimitService;
            _localizationService = localizationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PagedRequest request)
        {
            var result = await _userDiscountLimitService.GetAllAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _userDiscountLimitService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("salesperson/{salespersonId}")]
        public async Task<ActionResult<IEnumerable<UserDiscountLimitDto>>> GetBySalespersonId(long salespersonId)
        {
            var result = await _userDiscountLimitService.GetBySalespersonIdAsync(salespersonId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("group/{erpProductGroupCode}")]
        public async Task<IActionResult> GetByErpProductGroupCode(string erpProductGroupCode)
        {
            var result = await _userDiscountLimitService.GetByErpProductGroupCodeAsync(erpProductGroupCode);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("salesperson/{salespersonId}/group/{erpProductGroupCode}")]
        public async Task<ActionResult<UserDiscountLimitDto>> GetBySalespersonAndGroup(long salespersonId, string erpProductGroupCode)
        {
            var result = await _userDiscountLimitService.GetBySalespersonAndGroupAsync(salespersonId, erpProductGroupCode);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDiscountLimitDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<UserDiscountLimitDto>.ErrorResult(
                    _localizationService.GetLocalizedString("UserDiscountLimitController.InvalidModelState"),
                    _localizationService.GetLocalizedString("UserDiscountLimitController.InvalidModelStateExceptionMessage", ModelState?.ToString() ?? string.Empty),
                    400));
            }

            // Check if combination already exists
            var existsResult = await _userDiscountLimitService.ExistsBySalespersonAndGroupAsync(createDto.SalespersonId, createDto.ErpProductGroupCode);
            if (existsResult.Success && existsResult.Data)
            {
                var message = _localizationService.GetLocalizedString("UserDiscountLimitService.AlreadyExists", createDto.SalespersonId, createDto.ErpProductGroupCode);
                return Conflict(new { message });
            }

            var result = await _userDiscountLimitService.CreateAsync(createDto);
            if (result.Success)
            {
                return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateUserDiscountLimitDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<UserDiscountLimitDto>.ErrorResult(
                    _localizationService.GetLocalizedString("UserDiscountLimitController.InvalidModelState"),
                    _localizationService.GetLocalizedString("UserDiscountLimitController.InvalidModelStateExceptionMessage", ModelState?.ToString() ?? string.Empty),
                    400));
            }

            var result = await _userDiscountLimitService.UpdateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _userDiscountLimitService.DeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("exists/{id}")]
        public async Task<IActionResult> Exists(long id)
        {
            var result = await _userDiscountLimitService.ExistsAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("exists/salesperson/{salespersonId}/group/{erpProductGroupCode}")]
        public async Task<ActionResult<bool>> ExistsBySalespersonAndGroup(long salespersonId, string erpProductGroupCode)
        {
            var result = await _userDiscountLimitService.ExistsBySalespersonAndGroupAsync(salespersonId, erpProductGroupCode);
            return StatusCode(result.StatusCode, result);
        }
    }
}
