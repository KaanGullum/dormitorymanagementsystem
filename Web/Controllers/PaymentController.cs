using DormitoryManagementSystem.Business.Services.Interfaces;
using DormitoryManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DormitoryManagementSystem.Web.Controllers
{
    [Authorize(Roles = "SystemAdmin,DormitoryManager,FinanceOfficer,DormitoryStaff")]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;
        private readonly IStudentService _studentService;
        private readonly INotificationAuditService _auditService;

        public PaymentController(
            IPaymentService paymentService,
            IStudentService studentService,
            INotificationAuditService auditService)
        {
            _paymentService = paymentService;
            _studentService = studentService;
            _auditService = auditService;
        }

        public async Task<IActionResult> Index()
        {
            var payments = await _paymentService.GetAllPaymentsAsync();
            return View(payments);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int? studentId)
        {
            ViewBag.Students = await _studentService.GetAllStudentsAsync();
            var payment = new Payment();
            if (studentId.HasValue) payment.StudentId = studentId.Value;
            return View(payment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Payment payment)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Students = await _studentService.GetAllStudentsAsync();
                return View(payment);
            }

            payment.Status = "Pending";
            await _paymentService.CreatePaymentAsync(payment);

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            await _auditService.LogActionAsync(userId, "Create", "Payment",
                $"Payment record created: {payment.Amount:C} - Student {payment.StudentId}");

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RecordPayment(int paymentId)
        {
            try
            {
                await _paymentService.RecordPaymentAsync(paymentId);

                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
                await _auditService.LogActionAsync(userId, "Update", "Payment",
                    $"Payment approved: PaymentId={paymentId}");

                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> StudentPayments(int studentId)
        {
            var payments = await _paymentService.GetPaymentsByStudentAsync(studentId);
            var balance = await _paymentService.CalculateBalanceAsync(studentId);
            var student = await _studentService.GetStudentByIdAsync(studentId);

            ViewBag.Student = student;
            ViewBag.Balance = balance;
            ViewBag.Penalties = await _paymentService.GetPenaltiesByStudentAsync(studentId);
            return View(payments);
        }

        [Authorize(Roles = "SystemAdmin,FinanceOfficer,DormitoryManager")]
        public async Task<IActionResult> Overdue()
        {
            var overduePayments = await _paymentService.GetOverduePaymentsAsync();
            return View(overduePayments);
        }
    }
}
