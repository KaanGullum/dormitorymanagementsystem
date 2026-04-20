using System.ComponentModel.DataAnnotations;

namespace DormitoryManagementSystem.Domain.Entities
{
    public class HousingRecord
    {
        public int HousingId { get; set; }

        [Required]
        [Display(Name = "Student")]
        public int StudentId { get; set; }

        [Required]
        [Display(Name = "Room")]
        public int RoomId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Check-in Date")]
        public DateTime CheckInDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Check-out Date")]
        public DateTime? CheckOutDate { get; set; }

        [MaxLength(20)]
        [Display(Name = "Housing Status")]
        public string HousingStatus { get; set; } = "Active";

        public virtual Student? Student { get; set; }
        public virtual Room? Room { get; set; }
    }
}
