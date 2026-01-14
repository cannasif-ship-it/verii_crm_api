using cms_webapi.DTOs;
using cms_webapi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace cms_webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ShippingAddressController : ControllerBase
    {
        private readonly IShippingAddressService _shippingAddressService;

        public ShippingAddressController(IShippingAddressService shippingAddressService)
        {
            _shippingAddressService = shippingAddressService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request)
        {
            var result = await _shippingAddressService.GetAllShippingAddressesAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _shippingAddressService.GetShippingAddressByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetByCustomerId(long customerId)
        {
            var result = await _shippingAddressService.GetShippingAddressesByCustomerIdAsync(customerId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateShippingAddressDto createShippingAddressDto)
        {
            var result = await _shippingAddressService.CreateShippingAddressAsync(createShippingAddressDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] UpdateShippingAddressDto updateShippingAddressDto)
        {
            var result = await _shippingAddressService.UpdateShippingAddressAsync(id, updateShippingAddressDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _shippingAddressService.DeleteShippingAddressAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
