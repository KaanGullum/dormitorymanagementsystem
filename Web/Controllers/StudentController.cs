using DormitoryManagementSystem.Business.Services.Interfaces;
using DormitoryManagementSystem.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DormitoryManagementSystem.Web.Controllers
{
    [Authorize(Roles = "SystemAdmin,DormitoryManager,FinanceOfficer,DormitoryStaff")]
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly IRoomService _roomService;
        private readonly IHousingService _housingService;
        private readonly INotificationAuditService _auditService;

        public StudentController(
            IStudentService studentService,
            IRoomService roomService,
            IHousingService housingService,
            INotificationAuditService auditService)
        {
            _studentService = studentService;
            _roomService = roomService;
            _housingService = housingService;
            _auditService = auditService;
        }

        /// <summary>
        /// Report Section 5 - Fig 5.2 (Page 10): Student Report list.
        /// Columns: Student No, Full Name, Program, Year, Room, Building, Status, Check-in
        /// </summary>
        public async Task<IActionResult> Index(string? keyword, string? building, int? year, string? status)
        {
            var students = await _studentService.SearchStudentsAsync(keyword, building, year, status);
            var buildings = await _roomService.GetAllBuildingsAsync();

            // Active housing info for Room + Building + Check-in columns
            var activeHousings = await _housingService.GetActiveRecordsAsync();
            var housingByStudent = activeHousings
                .GroupBy(h => h.StudentId)
                .ToDictionary(g => g.Key, g => g.First());

            var rows = students.Select(s =>
            {
                housingByStudent.TryGetValue(s.StudentId, out var housing);
                return new StudentReportRow
                {
                    StudentId = s.StudentId,
                    StudentNumber = s.StudentNumber,
                    FullName = s.FullName,
                    Program = s.Program,
                    Year = s.Year,
                    Status = s.Status,
                    RoomNumber = housing?.Room?.RoomNumber,
                    BuildingName = housing?.Room?.Building?.BuildingName,
                    CheckInDate = housing?.CheckInDate
                };
            }).ToList();

            var model = new StudentReportViewModel
            {
                Rows = rows,
                SearchKeyword = keyword,
                FilterBuilding = building,
                FilterYear = year,
                FilterStatus = status,
                Buildings = buildings
            };

            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            if (student == null) return NotFound();

            ViewBag.HousingHistory = await _studentService.ViewHousingHistoryAsync(id);
            return View(student);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new StudentFormViewModel { IsEdit = false });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var existingStudent = await _studentService.GetStudentByNumberAsync(model.Student.StudentNumber);
            if (existingStudent != null)
            {
                ModelState.AddModelError("Student.StudentNumber", "A student with this number already exists.");
                return View(model);
            }

            await _studentService.CreateStudentAsync(model.Student);

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            await _auditService.LogActionAsync(userId, "Create", "Student",
                $"Student created: {model.Student.FullName}");

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            if (student == null) return NotFound();

            return View(new StudentFormViewModel { Student = student, IsEdit = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(StudentFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var existingStudent = await _studentService.GetStudentByNumberAsync(model.Student.StudentNumber);
            if (existingStudent != null && existingStudent.StudentId != model.Student.StudentId)
            {
                ModelState.AddModelError("Student.StudentNumber", "A student with this number already exists.");
                return View(model);
            }

            await _studentService.UpdateStudentAsync(model.Student);

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            await _auditService.LogActionAsync(userId, "Update", "Student",
                $"Student updated: {model.Student.FullName}");

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            if (student == null) return NotFound();

            var result = await _studentService.DeleteStudentAsync(id);
            if (!result)
            {
                TempData["Error"] = "Failed to delete student. The student may not exist.";
                return RedirectToAction(nameof(Index));
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            await _auditService.LogActionAsync(userId, "Delete", "Student",
                $"Student deleted: {student.FullName}");

            TempData["Success"] = "Student deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
