using System.ComponentModel.DataAnnotations;

namespace DormitoryManagementSystem.Domain.Entities
{
    public class MaintenanceRequest
    {
        public int RequestId { get; set; }

        [Required]
        [Display(Name = "Room")]
        public int RoomId { get; set; }

        [Required]
        [Display(Name = "Reported By Student")]
        public int ReportedByStudentId { get; set; }

        [Required(ErrorMessage = "Issue title is required.")]
        [MaxLength(200)]
        [Display(Name = "Issue Title")]
        public string IssueTitle { get; set; } = string.Empty;

        [MaxLength(20)]
        [Display(Name = "Priority")]
        public string Priority { get; set; } = "Medium";

        [MaxLength(20)]
        [Display(Name = "Status")]
        public string Status { get; set; } = "Open";

        [DataType(DataType.DateTime)]
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        [Display(Name = "Completed At")]
        public DateTime? CompletedAt { get; set; }

        [Display(Name = "Assigned Staff")]
        public int? AssignedStaffId { get; set; }

        public virtual Room? Room { get; set; }
        public virtual Student? ReportedByStudent { get; set; }

        public bool IsClosed() => Status == "Closed";
        public bool IsOpen() => Status != "Closed";
    }
}
