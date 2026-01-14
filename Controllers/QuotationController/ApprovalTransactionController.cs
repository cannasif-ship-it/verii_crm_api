using cms_webapi.DTOs;
using cms_webapi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace cms_webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ApprovalTransactionController : ControllerBase
    {
        private readonly IApprovalTransactionService _approvalTransactionService;

        public ApprovalTransactionController(IApprovalTransactionService approvalTransactionService)
        {
            _approvalTransactionService = approvalTransactionService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request)
        {
            var result = await _approvalTransactionService.GetAllApprovalTransactionsAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _approvalTransactionService.GetApprovalTransactionByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("document/{documentId}")]
        public async Task<IActionResult> GetByDocumentId(long documentId)
        {
            var result = await _approvalTransactionService.GetApprovalTransactionsByDocumentIdAsync(documentId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("line/{lineId}")]
        public async Task<IActionResult> GetByLineId(long lineId)
        {
            var result = await _approvalTransactionService.GetApprovalTransactionsByLineIdAsync(lineId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ApprovalTransactionCreateDto createDto)
        {
            var result = await _approvalTransactionService.CreateApprovalTransactionAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] ApprovalTransactionUpdateDto updateDto)
        {
            var result = await _approvalTransactionService.UpdateApprovalTransactionAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _approvalTransactionService.DeleteApprovalTransactionAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
