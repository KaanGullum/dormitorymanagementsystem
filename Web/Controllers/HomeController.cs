using DormitoryManagementSystem.Business.Services.Interfaces;
using DormitoryManagementSystem.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DormitoryManagementSystem.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public HomeController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        /// <summary>
        /// Report Section 5 - Fig 5.1: Overview Dashboard.
        /// Users in the Student role do not see admin data; StudentHome view is returned instead.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Student"))
            {
                return View("StudentHome");
            }

            var model = new DashboardViewModel
            {
                ActiveStudents = await _dashboardService.GetActiveStudentCountAsync(),
                OccupancyRate = await _dashboardService.GetOccupancyRateAsync(),
                CollectedFees = await _dashboardService.GetCollectedFeesAsync(),
                OpenRequests = await _dashboardService.GetOpenRequestCountAsync(),
                OccupancyByBuilding = await _dashboardService.GetOccupancyByBuildingAsync(),
                PaymentStatusDistribution = await _dashboardService.GetPaymentStatusDistributionAsync()
            };

            return View(model);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
