using System.ComponentModel.DataAnnotations;

namespace DormitoryManagementSystem.Domain.Entities
{
    public class Student
    {
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Student number is required.")]
        [MaxLength(20)]
        [Display(Name = "Student No")]
        public string StudentNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Full name is required.")]
        [MaxLength(100)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        [MaxLength(100)]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Enter a valid phone number.")]
        [MaxLength(20)]
        [Display(Name = "Phone")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Program is required.")]
        [MaxLength(100)]
        [Display(Name = "Program")]
        public string Program { get; set; } = string.Empty;

        [Range(1, 6, ErrorMessage = "Year must be between 1 and 6.")]
        [Display(Name = "Year")]
        public int Year { get; set; }

        [MaxLength(20)]
        [Display(Name = "Status")]
        public string Status { get; set; } = "Active";

        public virtual ICollection<HousingRecord> HousingRecords { get; set; } = new List<HousingRecord>();
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public virtual ICollection<Penalty> Penalties { get; set; } = new List<Penalty>();
        public virtual ICollection<MaintenanceRequest> MaintenanceRequests { get; set; } = new List<MaintenanceRequest>();
        public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

        public bool IsActive() => Status == "Active";
        public int ActiveHousingCount() => HousingRecords.Count(h => h.HousingStatus == "Active");
    }
}
