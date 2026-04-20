using System.ComponentModel.DataAnnotations;

namespace DormitoryManagementSystem.Domain.Entities
{
    public class Payment
    {
        public int PaymentId { get; set; }

        [Required(ErrorMessage = "Student is required.")]
        [Display(Name = "Student")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0, 1000000, ErrorMessage = "Amount must be between 0 and 1,000,000.")]
        [DataType(DataType.Currency)]
        [Display(Name = "Amount (TL)")]
        public decimal Amount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Paid Date")]
        public DateTime? PaidDate { get; set; }

        [MaxLength(20)]
        [Display(Name = "Status")]
        public string Status { get; set; } = "Pending";

        [MaxLength(50)]
        [Display(Name = "Semester")]
        public string? Semester { get; set; }

        public virtual Student? Student { get; set; }

        public bool IsOverdue() => Status != "Paid" && DueDate < DateTime.Now;
        public decimal OutstandingAmount() => Status == "Paid" ? 0m : Amount;
    }
}
