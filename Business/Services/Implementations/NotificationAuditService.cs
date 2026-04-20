using DormitoryManagementSystem.Business.Services.Interfaces;
using DormitoryManagementSystem.DataAccess.Repositories;
using DormitoryManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DormitoryManagementSystem.Business.Services.Implementations
{
    public class NotificationAuditService : INotificationAuditService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotificationAuditService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // ===== Notifications =====
        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId)
            => await _unitOfWork.Notifications.Query()
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

        public async Task CreateNotificationAsync(int userId, string message, string type = "Info")
        {
            var notification = new Notification
            {
                UserId = userId,
                Message = message,
                Type = type,
                IsRead = false,
                CreatedAt = DateTime.Now
            };
            await _unitOfWork.Notifications.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await _unitOfWork.Notifications.GetByIdAsync(notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                _unitOfWork.Notifications.Update(notification);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<int> GetUnreadCountAsync(int userId)
            => await _unitOfWork.Notifications.CountAsync(n => n.UserId == userId && !n.IsRead);

        // ===== Audit Logs =====
        public async Task LogActionAsync(int userId, string action, string moduleName, string? description = null)
        {
            var log = new AuditLog
            {
                UserId = userId,
                Action = action,
                ModuleName = moduleName,
                ActionTime = DateTime.Now,
                Description = description
            };
            await _unitOfWork.AuditLogs.AddAsync(log);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetAuditLogsAsync(
            string? moduleName = null, DateTime? from = null, DateTime? to = null)
        {
            IQueryable<AuditLog> query = _unitOfWork.AuditLogs.Query().Include(a => a.User);

            if (!string.IsNullOrEmpty(moduleName))
                query = query.Where(a => a.ModuleName == moduleName);
            if (from.HasValue)
                query = query.Where(a => a.ActionTime >= from.Value);
            if (to.HasValue)
                query = query.Where(a => a.ActionTime <= to.Value);

            return await query.OrderByDescending(a => a.ActionTime).ToListAsync();
        }

        // ===== Documents =====
        public async Task<IEnumerable<Document>> GetStudentDocumentsAsync(int studentId)
            => await _unitOfWork.Documents.FindAsync(d => d.StudentId == studentId);

        public async Task UploadDocumentAsync(Document document)
        {
            document.UploadedAt = DateTime.Now;
            await _unitOfWork.Documents.AddAsync(document);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteDocumentAsync(int documentId)
        {
            var document = await _unitOfWork.Documents.GetByIdAsync(documentId);
            if (document != null)
            {
                _unitOfWork.Documents.Delete(document);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
