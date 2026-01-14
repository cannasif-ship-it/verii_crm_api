using System.ComponentModel.DataAnnotations;
using cms_webapi.Models;

namespace cms_webapi.DTOs
{
    public class UserHierarchyGetDto : BaseEntityDto
    {
        public long SalespersonId { get; set; }
        public string? SalespersonFullName { get; set; }
        public long? ManagerId { get; set; }
        public string? ManagerFullName { get; set; }
        public long? GeneralManagerId { get; set; }
        public string? GeneralManagerFullName { get; set; }
        public int HierarchyLevel { get; set; }
        public bool IsActive { get; set; }
    }

    public class UserHierarchyCreateDto
    {
        [Required]
        public long SalespersonId { get; set; }

        public long? ManagerId { get; set; }

        public long? GeneralManagerId { get; set; }

        public int HierarchyLevel { get; set; } = 1;

        public bool IsActive { get; set; } = true;
    }

    public class UserHierarchyUpdateDto
    {
        [Required]
        public long SalespersonId { get; set; }

        public long? ManagerId { get; set; }

        public long? GeneralManagerId { get; set; }

        public int HierarchyLevel { get; set; }

        public bool IsActive { get; set; }
    }
}
