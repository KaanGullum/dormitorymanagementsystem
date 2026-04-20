using System.ComponentModel.DataAnnotations;

namespace DormitoryManagementSystem.Domain.Entities
{
    public class Room
    {
        public int RoomId { get; set; }

        [Required(ErrorMessage = "Room number is required.")]
        [MaxLength(20)]
        [Display(Name = "Room No")]
        public string RoomNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Building is required.")]
        [Display(Name = "Building")]
        public int BuildingId { get; set; }

        [Range(0, 20, ErrorMessage = "Floor must be between 0 and 20.")]
        [Display(Name = "Floor")]
        public int FloorNo { get; set; }

        [Range(1, 10, ErrorMessage = "Capacity must be between 1 and 10.")]
        [Display(Name = "Capacity")]
        public int Capacity { get; set; }

        [Range(0, 10)]
        [Display(Name = "Occupied Beds")]
        public int OccupiedBeds { get; set; }

        [MaxLength(30)]
        [Display(Name = "Status")]
        public string Status { get; set; } = "Available";

        [MaxLength(30)]
        [Display(Name = "Room Type")]
        public string? RoomType { get; set; }

        public virtual Building? Building { get; set; }
        public virtual ICollection<HousingRecord> HousingRecords { get; set; } = new List<HousingRecord>();
        public virtual ICollection<MaintenanceRequest> MaintenanceRequests { get; set; } = new List<MaintenanceRequest>();

        public int GetAvailableBeds() => Capacity - OccupiedBeds;
        public bool HasFreeBed() => OccupiedBeds < Capacity;
        public bool IsFull() => OccupiedBeds >= Capacity;
    }
}
