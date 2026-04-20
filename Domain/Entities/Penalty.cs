using System.ComponentModel.DataAnnotations;

namespace DormitoryManagementSystem.Domain.Entities
{
    public class Penalty
    {
        public int PenaltyId { get; set; }

        [Required(ErrorMessage = "Student is required.")]
        [Display(Name = "Student")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Penalty reason is required.")]
        [MaxLength(300)]
        [Display(Name = "Reason")]
        public string Reason { get; set; } = string.Empty;

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0, 100000, ErrorMessage = "Amount must be between 0 and 100,000.")]
        [DataType(DataType.Currency)]
        [Display(Name = "Amount (TL)")]
        public decimal Amount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }

        [MaxLength(20)]
        [Display(Name = "Status")]
        public string Status { get; set; } = "Pending";

        public virtual Student? Student { get; set; }

        public bool IsPending() => Status != "Paid";
    }
}
