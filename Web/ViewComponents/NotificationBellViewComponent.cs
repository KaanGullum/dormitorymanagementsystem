using DormitoryManagementSystem.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DormitoryManagementSystem.Web.ViewComponents
{
    /// <summary>
    /// ViewComponent that renders an unread-count badge for the "Notifications" menu.
    /// </summary>
    public class NotificationBellViewComponent : ViewComponent
    {
        private readonly INotificationAuditService _service;

        public NotificationBellViewComponent(INotificationAuditService service)
        {
            _service = service;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userIdClaim = (User as ClaimsPrincipal)?.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                return View(0);

            var unread = await _service.GetUnreadCountAsync(userId);
            return View(unread);
        }
    }
}
