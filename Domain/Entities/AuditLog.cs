using System.ComponentModel.DataAnnotations;

namespace DormitoryManagementSystem.Domain.Entities
{
    public class AuditLog
    {
        public int LogId { get; set; }

        [Required]
        [Display(Name = "User")]
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Action")]
        public string Action { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [Display(Name = "Module")]
        public string ModuleName { get; set; } = string.Empty;

        [DataType(DataType.DateTime)]
        [Display(Name = "Action Time")]
        public DateTime ActionTime { get; set; } = DateTime.Now;

        [MaxLength(1000)]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        public virtual User? User { get; set; }
    }
}
