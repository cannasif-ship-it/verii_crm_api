using crm_api.DTOs;
using crm_api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace crm_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CustomerTypeController : ControllerBase
    {
        private readonly ICustomerTypeService _customerTypeService;

        public CustomerTypeController(ICustomerTypeService customerTypeService)
        {
            _customerTypeService = customerTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request)
        {
            var result = await _customerTypeService.GetAllCustomerTypesAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _customerTypeService.GetCustomerTypeByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CustomerTypeCreateDto customerTypeCreateDto)
        {
            var result = await _customerTypeService.CreateCustomerTypeAsync(customerTypeCreateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] CustomerTypeUpdateDto customerTypeUpdateDto)
        {
            var result = await _customerTypeService.UpdateCustomerTypeAsync(id, customerTypeUpdateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _customerTypeService.DeleteCustomerTypeAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
