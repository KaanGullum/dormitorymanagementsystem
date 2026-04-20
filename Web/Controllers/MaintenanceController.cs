using DormitoryManagementSystem.Business.Services.Interfaces;
using DormitoryManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DormitoryManagementSystem.Web.Controllers
{
    [Authorize]
    public class MaintenanceController : Controller
    {
        private readonly IMaintenanceService _maintenanceService;
        private readonly IRoomService _roomService;
        private readonly IStudentService _studentService;
        private readonly INotificationAuditService _auditService;

        public MaintenanceController(
            IMaintenanceService maintenanceService,
            IRoomService roomService,
            IStudentService studentService,
            INotificationAuditService auditService)
        {
            _maintenanceService = maintenanceService;
            _roomService = roomService;
            _studentService = studentService;
            _auditService = auditService;
        }

        public async Task<IActionResult> Index()
        {
            var requests = await _maintenanceService.GetAllRequestsAsync();
            return View(requests);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Rooms = await _roomService.GetAllRoomsAsync();
            ViewBag.Students = await _studentService.GetAllStudentsAsync();
            return View(new MaintenanceRequest());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MaintenanceRequest request)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Rooms = await _roomService.GetAllRoomsAsync();
                ViewBag.Students = await _studentService.GetAllStudentsAsync();
                return View(request);
            }

            await _maintenanceService.CreateRequestAsync(request);

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            await _auditService.LogActionAsync(userId, "Create", "Maintenance",
                $"Maintenance request created: {request.IssueTitle}");

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignStaff(int requestId, int staffId)
        {
            await _maintenanceService.AssignStaffAsync(requestId, staffId);

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            await _auditService.LogActionAsync(userId, "Update", "Maintenance",
                $"Staff assigned: RequestId={requestId}, StaffId={staffId}");

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Close(int requestId)
        {
            await _maintenanceService.CloseRequestAsync(requestId);

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            await _auditService.LogActionAsync(userId, "Update", "Maintenance",
                $"Maintenance request closed: RequestId={requestId}");

            return RedirectToAction(nameof(Index));
        }
    }
}
