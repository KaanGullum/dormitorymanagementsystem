using DormitoryManagementSystem.Domain.Entities;

namespace DormitoryManagementSystem.Business.Services.Interfaces
{
    /// <summary>
    /// Document, Notification, and Audit Module.
    /// Stores files and documents, delivers internal notifications, and keeps audit logs
    /// of important create, update, delete, and approval actions.
    /// </summary>
    public interface INotificationAuditService
    {
        // Notifications
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId);
        Task CreateNotificationAsync(int userId, string message, string type = "Info");
        Task MarkAsReadAsync(int notificationId);
        Task<int> GetUnreadCountAsync(int userId);

        // Audit Logs
        Task LogActionAsync(int userId, string action, string moduleName, string? description = null);
        Task<IEnumerable<AuditLog>> GetAuditLogsAsync(string? moduleName = null, DateTime? from = null, DateTime? to = null);

        // Documents
        Task<IEnumerable<Document>> GetStudentDocumentsAsync(int studentId);
        Task UploadDocumentAsync(Document document);
        Task DeleteDocumentAsync(int documentId);
    }
}
