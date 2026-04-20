using DormitoryManagementSystem.Business.Services.Interfaces;
using DormitoryManagementSystem.Domain.Entities;
using DormitoryManagementSystem.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DormitoryManagementSystem.Web.Controllers
{
    [Authorize(Roles = "SystemAdmin,FinanceOfficer,DormitoryManager")]
    public class PenaltyController : Controller
    {
        private readonly IPenaltyService _penaltyService;
        private readonly IStudentService _studentService;
        private readonly INotificationAuditService _auditService;

        public PenaltyController(
            IPenaltyService penaltyService,
            IStudentService studentService,
            INotificationAuditService auditService)
        {
            _penaltyService = penaltyService;
            _studentService = studentService;
            _auditService = auditService;
        }

        public async Task<IActionResult> Index()
        {
            var penalties = await _penaltyService.GetAllAsync();
            ViewBag.TotalPending = await _penaltyService.GetTotalPendingAmountAsync();
            return View(penalties);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new PenaltyFormViewModel
            {
                Students = await _studentService.GetAllStudentsAsync(),
                Penalty = new Penalty { DueDate = DateTime.Now.AddDays(30) }
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PenaltyFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Students = await _studentService.GetAllStudentsAsync();
                return View(model);
            }

            await _penaltyService.CreateAsync(model.Penalty);

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            await _auditService.LogActionAsync(userId, "Create", "Penalty",
                $"Penalty created: {model.Penalty.Amount:C} - Student {model.Penalty.StudentId} - {model.Penalty.Reason}");

            TempData["Success"] = "Penalty record created.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkPaid(int id)
        {
            try
            {
                await _penaltyService.MarkAsPaidAsync(id);

                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
                await _auditService.LogActionAsync(userId, "Update", "Penalty",
                    $"Penalty marked as paid: PenaltyId={id}");

                TempData["Success"] = "Penalty marked as paid.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SystemAdmin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _penaltyService.DeleteAsync(id);

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            await _auditService.LogActionAsync(userId, "Delete", "Penalty",
                $"Penalty deleted: PenaltyId={id}");

            TempData["Success"] = "Penalty record deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}
