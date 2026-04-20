using System.ComponentModel.DataAnnotations;

namespace DormitoryManagementSystem.Domain.Entities
{
    public class Notification
    {
        public int NotificationId { get; set; }

        [Required]
        [Display(Name = "User")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Message is required.")]
        [MaxLength(500)]
        [Display(Name = "Message")]
        public string Message { get; set; } = string.Empty;

        [MaxLength(20)]
        [Display(Name = "Type")]
        public string Type { get; set; } = "Info";

        [Display(Name = "Read")]
        public bool IsRead { get; set; } = false;

        [DataType(DataType.DateTime)]
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual User? User { get; set; }
    }
}
