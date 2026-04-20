using DormitoryManagementSystem.Business.Services.Interfaces;
using DormitoryManagementSystem.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DormitoryManagementSystem.Web.Controllers
{
    [Authorize(Roles = "SystemAdmin,DormitoryManager,FinanceOfficer,DormitoryStaff")]
    public class ReportController : Controller
    {
        private readonly IPaymentService _paymentService;
        private readonly IMaintenanceService _maintenanceService;
        private readonly IStudentService _studentService;
        private readonly IRoomService _roomService;
        private readonly IHousingService _housingService;
        private readonly INotificationAuditService _auditService;

        public ReportController(
            IPaymentService paymentService,
            IMaintenanceService maintenanceService,
            IStudentService studentService,
            IRoomService roomService,
            IHousingService housingService,
            INotificationAuditService auditService)
        {
            _paymentService = paymentService;
            _maintenanceService = maintenanceService;
            _studentService = studentService;
            _roomService = roomService;
            _housingService = housingService;
            _auditService = auditService;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Report Section 5 - Fig 5.2: Payment & Maintenance Report.
        /// </summary>
        public async Task<IActionResult> PaymentMaintenance()
        {
            var model = new PaymentReportViewModel
            {
                TotalAmount = await _paymentService.GetTotalCollectedAsync() +
                              await _paymentService.GetTotalPendingAsync() +
                              await _paymentService.GetTotalOverdueAsync(),
                CollectedAmount = await _paymentService.GetTotalCollectedAsync(),
                PendingAmount = await _paymentService.GetTotalPendingAsync(),
                PenaltyAmount = await _paymentService.GetTotalOverdueAsync(),
                OpenRequests = await _maintenanceService.GetOpenRequestsAsync()
            };
            return View(model);
        }

        /// <summary>
        /// Report Section 5 - Fig 5.2: Student Report.
        /// </summary>
        public async Task<IActionResult> StudentReport(string? keyword, string? building, int? year, string? status)
        {
            var students = await _studentService.SearchStudentsAsync(keyword, building, year, status);
            var buildings = await _roomService.GetAllBuildingsAsync();

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

        /// <summary>
        /// Housing Module + Reporting intersection: filterable housing records.
        /// </summary>
        public async Task<IActionResult> HousingReport(string? building, string? status, int? year)
        {
            var records = await _housingService.GetHousingReportAsync(building, status, year);
            var buildings = await _roomService.GetAllBuildingsAsync();

            var rows = records.Select(h => new HousingReportRow
            {
                HousingId = h.HousingId,
                StudentNumber = h.Student?.StudentNumber ?? "-",
                StudentName = h.Student?.FullName ?? "-",
                Program = h.Student?.Program ?? "-",
                StudentYear = h.Student?.Year ?? 0,
                BuildingName = h.Room?.Building?.BuildingName ?? "-",
                RoomNumber = h.Room?.RoomNumber ?? "-",
                RoomType = h.Room?.RoomType,
                CheckInDate = h.CheckInDate,
                CheckOutDate = h.CheckOutDate,
                HousingStatus = h.HousingStatus
            }).ToList();

            var model = new HousingReportViewModel
            {
                Rows = rows,
                Buildings = buildings,
                FilterBuilding = building,
                FilterStatus = status,
                FilterYear = year,
                TotalActive = rows.Count(r => r.HousingStatus == "Active"),
                TotalCheckedOut = rows.Count(r => r.HousingStatus == "CheckedOut"),
                TotalTransferred = rows.Count(r => r.HousingStatus == "Transferred")
            };

            return View(model);
        }

        /// <summary>
        /// Audit log report (Admin only).
        /// </summary>
        [Authorize(Roles = "SystemAdmin")]
        public async Task<IActionResult> AuditLog(string? module, DateTime? from, DateTime? to)
        {
            var logs = await _auditService.GetAuditLogsAsync(module, from, to);
            return View(logs);
        }
    }
}
