using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cms_webapi.Models
{
    [Table("RII_USER_HIERARCHY")]
    public class UserHierarchy : BaseEntity
    {
        [Required]
        public long SalespersonId { get; set; }
        [ForeignKey("SalespersonId")]
        public User Salesperson { get; set; } = null!;
        
        public long? ManagerId { get; set; }
        [ForeignKey("ManagerId")]
        public User? Manager { get; set; }
        
        public long? GeneralManagerId { get; set; }
        [ForeignKey("GeneralManagerId")]
        public User? GeneralManager { get; set; }
        
        public int HierarchyLevel { get; set; } = 1; // 1=Satışçı, 2=Amir, 3=Genel Müdür
        
        public bool IsActive { get; set; } = true;
    }
}
