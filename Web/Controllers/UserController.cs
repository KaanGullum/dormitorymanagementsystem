using DormitoryManagementSystem.Business.Services.Interfaces;
using DormitoryManagementSystem.Domain.Entities;
using DormitoryManagementSystem.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DormitoryManagementSystem.Web.Controllers
{
    [Authorize(Roles = "SystemAdmin")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly INotificationAuditService _auditService;

        public UserController(IUserService userService, INotificationAuditService auditService)
        {
            _userService = userService;
            _auditService = auditService;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var roles = await _userService.GetAllRolesAsync();
            return View(new UserFormViewModel { Roles = roles, IsEdit = false });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Roles = await _userService.GetAllRolesAsync();
                return View(model);
            }

            var existingUser = await _userService.GetUserByUsernameAsync(model.User.Username);
            if (existingUser != null)
            {
                ModelState.AddModelError("User.Username", "A user with this username already exists.");
                model.Roles = await _userService.GetAllRolesAsync();
                return View(model);
            }

            // SHA256 hash of default password (production should use BCrypt)
            model.User.PasswordHash = Convert.ToBase64String(
                System.Security.Cryptography.SHA256.HashData(
                    System.Text.Encoding.UTF8.GetBytes("DefaultPassword123!")));

            await _userService.CreateUserAsync(model.User);

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            await _auditService.LogActionAsync(userId, "Create", "User",
                $"User created: {model.User.FullName}");

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            var roles = await _userService.GetAllRolesAsync();
            return View(new UserFormViewModel { User = user, Roles = roles, IsEdit = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Roles = await _userService.GetAllRolesAsync();
                return View(model);
            }

            var existingUser = await _userService.GetUserByUsernameAsync(model.User.Username);
            if (existingUser != null && existingUser.UserId != model.User.UserId)
            {
                ModelState.AddModelError("User.Username", "A user with this username already exists.");
                model.Roles = await _userService.GetAllRolesAsync();
                return View(model);
            }

            await _userService.UpdateUserAsync(model.User);

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            await _auditService.LogActionAsync(userId, "Update", "User",
                $"User updated: {model.User.FullName}");

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            await _userService.ToggleUserStatusAsync(id);

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            await _auditService.LogActionAsync(userId, "Update", "User",
                $"User status toggled: UserId={id}");

            return RedirectToAction(nameof(Index));
        }
    }
}
