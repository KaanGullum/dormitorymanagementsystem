using DormitoryManagementSystem.Domain.Entities;

namespace DormitoryManagementSystem.Business.Services.Interfaces
{
    /// <summary>
    /// Maintenance Request Module.
    /// Collects room-related technical issues, priority levels, status changes,
    /// assigned staff, and completion history to support internal service quality.
    /// </summary>
    public interface IMaintenanceService
    {
        Task<IEnumerable<MaintenanceRequest>> GetAllRequestsAsync();
        Task<MaintenanceRequest?> GetRequestByIdAsync(int id);
        Task CreateRequestAsync(MaintenanceRequest request);
        Task AssignStaffAsync(int requestId, int staffId);
        Task CloseRequestAsync(int requestId);
        Task UpdateStatusAsync(int requestId, string status);
        Task<IEnumerable<MaintenanceRequest>> GetOpenRequestsAsync();
        Task<int> GetOpenRequestCountAsync();
    }
}
