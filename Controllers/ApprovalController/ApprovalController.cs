using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using cms_webapi.DTOs;
using cms_webapi.Interfaces;

namespace cms_webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ApprovalController : ControllerBase
    {
        private readonly IApprovalService _approvalService;
        private readonly ILocalizationService _localizationService;

        public ApprovalController(IApprovalService approvalService, ILocalizationService localizationService)
        {
            _approvalService = approvalService;
            _localizationService = localizationService;
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingApprovals()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return StatusCode(401, ApiResponse<List<ApprovalQueueGetDto>>.ErrorResult(
                    "Kullanıcı kimliği bulunamadı.",
                    "User identity not found",
                    401));
            }

            var result = await _approvalService.GetPendingApprovals(userId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("quotation/{quotationId}/status")]
        public async Task<IActionResult> GetQuotationApprovalStatus(long quotationId)
        {
            var result = await _approvalService.GetQuotationApprovalStatus(quotationId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("quotation/{quotationId}/can-edit")]
        public async Task<IActionResult> CanEditQuotation(long quotationId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return StatusCode(401, ApiResponse<bool>.ErrorResult(
                    "Kullanıcı kimliği bulunamadı.",
                    "User identity not found",
                    401));
            }

            var canEdit = await _approvalService.CanUserEditQuotation(quotationId, userId);
            return StatusCode(200, ApiResponse<bool>.SuccessResult(canEdit, "Yetki kontrolü tamamlandı"));
        }

        [HttpPost("{queueId}/approve")]
        public async Task<IActionResult> Approve(long queueId, [FromBody] ApprovalNoteDto? dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return StatusCode(401, ApiResponse<bool>.ErrorResult(
                    "Kullanıcı kimliği bulunamadı.",
                    "User identity not found",
                    401));
            }

            var result = await _approvalService.ApproveQuotation(queueId, userId, dto?.Note);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("{queueId}/reject")]
        public async Task<IActionResult> Reject(long queueId, [FromBody] ApprovalNoteDto? dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return StatusCode(401, ApiResponse<bool>.ErrorResult(
                    "Kullanıcı kimliği bulunamadı.",
                    "User identity not found",
                    401));
            }

            var result = await _approvalService.RejectQuotation(queueId, userId, dto?.Note);
            return StatusCode(result.StatusCode, result);
        }
    }
}
