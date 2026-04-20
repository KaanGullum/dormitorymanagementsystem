using System.ComponentModel.DataAnnotations;

namespace DormitoryManagementSystem.Domain.Entities
{
    public class Role
    {
        public int RoleId { get; set; }

        [Required(ErrorMessage = "Role name is required.")]
        [MaxLength(50)]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; } = string.Empty;

        [MaxLength(250)]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
