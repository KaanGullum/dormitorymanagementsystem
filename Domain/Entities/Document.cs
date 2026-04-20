using System.ComponentModel.DataAnnotations;

namespace DormitoryManagementSystem.Domain.Entities
{
    public class Document
    {
        public int DocumentId { get; set; }

        [Required]
        [Display(Name = "Student")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "File name is required.")]
        [MaxLength(200)]
        [Display(Name = "File Name")]
        public string FileName { get; set; } = string.Empty;

        [MaxLength(50)]
        [Display(Name = "File Type")]
        public string? FileType { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Uploaded At")]
        public DateTime UploadedAt { get; set; } = DateTime.Now;

        [MaxLength(500)]
        [Display(Name = "File Path")]
        public string? FilePath { get; set; }

        public virtual Student? Student { get; set; }
    }
}
