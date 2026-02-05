using System.ComponentModel.DataAnnotations;

namespace crm_api.DTOs.PowerBi
{
    public class UserPowerBIGroupGetDto : BaseEntityDto
    {
        public long UserId { get; set; }
        public string? UserName { get; set; }

        public long GroupId { get; set; }
        public string? GroupName { get; set; }

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }

    public class CreateUserPowerBIGroupDto
    {
        [Required]
        public long UserId { get; set; }

        [Required]
        public long GroupId { get; set; }
    }

    public class UpdateUserPowerBIGroupDto
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public long UserId { get; set; }

        [Required]
        public long GroupId { get; set; }
    }
}
