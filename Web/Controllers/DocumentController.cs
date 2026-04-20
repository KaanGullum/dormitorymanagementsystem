using DormitoryManagementSystem.Business.Services.Interfaces;
using DormitoryManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DormitoryManagementSystem.Web.Controllers
{
    [Authorize(Roles = "SystemAdmin,DormitoryManager,FinanceOfficer,DormitoryStaff")]
    public class DocumentController : Controller
    {
        private readonly INotificationAuditService _service;
        private readonly IStudentService _studentService;
        private readonly IWebHostEnvironment _env;

        // Security: allowed file extensions
        private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".png", ".jpg", ".jpeg", ".txt"
        };

        // Security: max file size 5 MB
        private const long MaxFileSize = 5 * 1024 * 1024;

        public DocumentController(
            INotificationAuditService service,
            IStudentService studentService,
            IWebHostEnvironment env)
        {
            _service = service;
            _studentService = studentService;
            _env = env;
        }

        /// <summary>
        /// Lists documents for a student. If studentId is empty, lists all students.
        /// </summary>
        public async Task<IActionResult> Index(int? studentId)
        {
            if (!studentId.HasValue)
            {
                var students = await _studentService.GetAllStudentsAsync();
                ViewBag.Students = students;
                return View("SelectStudent");
            }

            var student = await _studentService.GetStudentByIdAsync(studentId.Value);
            if (student == null) return NotFound();

            ViewBag.Student = student;
            var documents = await _service.GetStudentDocumentsAsync(studentId.Value);
            return View(documents);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(int studentId, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["Error"] = "No file selected.";
                return RedirectToAction(nameof(Index), new { studentId });
            }

            if (file.Length > MaxFileSize)
            {
                TempData["Error"] = "File size cannot exceed 5 MB.";
                return RedirectToAction(nameof(Index), new { studentId });
            }

            var extension = Path.GetExtension(file.FileName);
            if (!AllowedExtensions.Contains(extension))
            {
                TempData["Error"] = $"'{extension}' extension is not allowed. Allowed: {string.Join(", ", AllowedExtensions)}";
                return RedirectToAction(nameof(Index), new { studentId });
            }

            // Create upload folder
            var uploadsRoot = Path.Combine(_env.WebRootPath, "uploads", "documents");
            Directory.CreateDirectory(uploadsRoot);

            // Make file name unique: {guid}_{originalname}
            var safeName = $"{Guid.NewGuid():N}_{Path.GetFileName(file.FileName)}";
            var fullPath = Path.Combine(uploadsRoot, safeName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var doc = new Document
            {
                StudentId = studentId,
                FileName = file.FileName,
                FileType = file.ContentType,
                FilePath = Path.Combine("uploads", "documents", safeName).Replace('\\', '/'),
                UploadedAt = DateTime.Now
            };

            await _service.UploadDocumentAsync(doc);

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            await _service.LogActionAsync(userId, "Create", "Document",
                $"Document uploaded: {file.FileName} - Student {studentId}");

            TempData["Success"] = "Document uploaded successfully.";
            return RedirectToAction(nameof(Index), new { studentId });
        }

        public async Task<IActionResult> Download(int id)
        {
            var doc = (await _service.GetStudentDocumentsAsync(0))
                .Concat(await GetAllDocumentsAsync())
                .FirstOrDefault(d => d.DocumentId == id);

            if (doc == null || string.IsNullOrEmpty(doc.FilePath)) return NotFound();

            var fullPath = Path.Combine(_env.WebRootPath, doc.FilePath);
            if (!System.IO.File.Exists(fullPath)) return NotFound();

            var bytes = await System.IO.File.ReadAllBytesAsync(fullPath);
            return File(bytes, doc.FileType ?? "application/octet-stream", doc.FileName);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SystemAdmin,DormitoryManager")]
        public async Task<IActionResult> Delete(int id, int studentId)
        {
            var docs = await _service.GetStudentDocumentsAsync(studentId);
            var doc = docs.FirstOrDefault(d => d.DocumentId == id);
            if (doc != null && !string.IsNullOrEmpty(doc.FilePath))
            {
                var fullPath = Path.Combine(_env.WebRootPath, doc.FilePath);
                if (System.IO.File.Exists(fullPath))
                {
                    try { System.IO.File.Delete(fullPath); } catch { /* ignore */ }
                }
            }

            await _service.DeleteDocumentAsync(id);

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            await _service.LogActionAsync(userId, "Delete", "Document",
                $"Document deleted: DocumentId={id}");

            TempData["Success"] = "Document deleted.";
            return RedirectToAction(nameof(Index), new { studentId });
        }

        // Helper: fetch documents for all students (for Download lookup)
        private async Task<IEnumerable<Document>> GetAllDocumentsAsync()
        {
            var students = await _studentService.GetAllStudentsAsync();
            var all = new List<Document>();
            foreach (var s in students)
            {
                var docs = await _service.GetStudentDocumentsAsync(s.StudentId);
                all.AddRange(docs);
            }
            return all;
        }
    }
}
