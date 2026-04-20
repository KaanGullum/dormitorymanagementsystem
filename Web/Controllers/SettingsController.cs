using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DormitoryManagementSystem.Web.Controllers
{
    [Authorize(Roles = "SystemAdmin")]
    public class SettingsController : Controller
    {
        private readonly IConfiguration _configuration;

        public SettingsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            // Configuration values read from appsettings.json
            ViewBag.DefaultSemester = _configuration["DormitorySettings:DefaultSemester"];
            ViewBag.MaxCapacity = _configuration["DormitorySettings:MaxCapacityPerRoom"];
            ViewBag.PaymentDueDays = _configuration["DormitorySettings:PaymentDueDays"];
            ViewBag.PenaltyRate = _configuration["DormitorySettings:OverduePenaltyRate"];
            ViewBag.NotificationEnabled = _configuration["DormitorySettings:NotificationEnabled"];
            ViewBag.BackupPath = _configuration["DormitorySettings:BackupPath"];
            return View();
        }
    }
}
