using DormitoryManagementSystem.Business.Services.Interfaces;
using DormitoryManagementSystem.Domain.Entities;
using DormitoryManagementSystem.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DormitoryManagementSystem.Web.Controllers
{
    [Authorize]
    public class RoomController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly IHousingService _housingService;
        private readonly IStudentService _studentService;
        private readonly INotificationAuditService _auditService;

        public RoomController(
            IRoomService roomService,
            IHousingService housingService,
            IStudentService studentService,
            INotificationAuditService auditService)
        {
            _roomService = roomService;
            _housingService = housingService;
            _studentService = studentService;
            _auditService = auditService;
        }

        public async Task<IActionResult> Index(int? buildingId)
        {
            var rooms = buildingId.HasValue
                ? await _roomService.GetRoomsByBuildingAsync(buildingId.Value)
                : await _roomService.GetAllRoomsAsync();

            ViewBag.Buildings = await _roomService.GetAllBuildingsAsync();
            ViewBag.SelectedBuilding = buildingId;
            return View(rooms);
        }

        [HttpGet]
        [Authorize(Roles = "SystemAdmin,DormitoryManager")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Buildings = await _roomService.GetAllBuildingsAsync();
            return View(new Room());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SystemAdmin,DormitoryManager")]
        public async Task<IActionResult> Create(Room room)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Buildings = await _roomService.GetAllBuildingsAsync();
                return View(room);
            }

            if (await _roomService.RoomExistsInBuildingAsync(room.BuildingId, room.RoomNumber))
            {
                ModelState.AddModelError("RoomNumber", "A room with this number already exists in the selected building.");
                ViewBag.Buildings = await _roomService.GetAllBuildingsAsync();
                return View(room);
            }

            await _roomService.CreateRoomAsync(room);

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            await _auditService.LogActionAsync(userId, "Create", "Room",
                $"Room created: {room.RoomNumber}");

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "SystemAdmin,DormitoryManager,DormitoryStaff,FinanceOfficer")]
        public async Task<IActionResult> AssignStudent(int studentId)
        {
            var student = await _studentService.GetStudentByIdAsync(studentId);
            if (student == null) return NotFound();

            var availableRooms = await _roomService.GetAvailableRoomsAsync();
            var model = new AssignRoomViewModel
            {
                StudentId = studentId,
                StudentName = student.FullName,
                AvailableRooms = availableRooms
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SystemAdmin,DormitoryManager,DormitoryStaff,FinanceOfficer")]
        public async Task<IActionResult> AssignStudent(AssignRoomViewModel model)
        {
            try
            {
                await _housingService.CheckInAsync(model.StudentId, model.SelectedRoomId);

                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
                await _auditService.LogActionAsync(userId, "Create", "Housing",
                    $"Student {model.StudentId} placed in room {model.SelectedRoomId}.");

                return RedirectToAction("Index", "Student");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                model.AvailableRooms = await _roomService.GetAvailableRoomsAsync();
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SystemAdmin,DormitoryManager,DormitoryStaff,FinanceOfficer")]
        public async Task<IActionResult> CheckOut(int housingId)
        {
            try
            {
                await _housingService.CheckOutAsync(housingId);

                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
                await _auditService.LogActionAsync(userId, "Update", "Housing",
                    $"Checked out: HousingId={housingId}");

                return RedirectToAction("Index", "Student");
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
