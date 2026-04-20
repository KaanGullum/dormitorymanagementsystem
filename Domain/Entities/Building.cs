using System.ComponentModel.DataAnnotations;

namespace DormitoryManagementSystem.Domain.Entities
{
    public class Building
    {
        public int BuildingId { get; set; }

        [Required(ErrorMessage = "Building name is required.")]
        [MaxLength(100)]
        [Display(Name = "Building Name")]
        public string BuildingName { get; set; } = string.Empty;

        [MaxLength(300)]
        [Display(Name = "Address")]
        public string? Address { get; set; }

        public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
    }
}
