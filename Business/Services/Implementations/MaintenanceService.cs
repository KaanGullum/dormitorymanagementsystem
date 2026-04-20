using DormitoryManagementSystem.Business.Services.Interfaces;
using DormitoryManagementSystem.DataAccess.Repositories;
using DormitoryManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DormitoryManagementSystem.Business.Services.Implementations
{
    public class MaintenanceService : IMaintenanceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MaintenanceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<MaintenanceRequest>> GetAllRequestsAsync()
            => await _unitOfWork.MaintenanceRequests.Query()
                .Include(m => m.Room).ThenInclude(r => r!.Building)
                .Include(m => m.ReportedByStudent)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();

        public async Task<MaintenanceRequest?> GetRequestByIdAsync(int id)
            => await _unitOfWork.MaintenanceRequests.GetByIdAsync(id);

        public async Task CreateRequestAsync(MaintenanceRequest request)
        {
            request.CreatedAt = DateTime.Now;
            request.Status = "Open";
            await _unitOfWork.MaintenanceRequests.AddAsync(request);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AssignStaffAsync(int requestId, int staffId)
        {
            var request = await _unitOfWork.MaintenanceRequests.GetByIdAsync(requestId);
            if (request == null) throw new InvalidOperationException("Request not found.");

            request.AssignedStaffId = staffId;
            request.Status = "Assigned";
            _unitOfWork.MaintenanceRequests.Update(request);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task CloseRequestAsync(int requestId)
        {
            var request = await _unitOfWork.MaintenanceRequests.GetByIdAsync(requestId);
            if (request == null) throw new InvalidOperationException("Request not found.");

            request.Status = "Closed";
            request.CompletedAt = DateTime.Now;
            _unitOfWork.MaintenanceRequests.Update(request);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateStatusAsync(int requestId, string status)
        {
            var request = await _unitOfWork.MaintenanceRequests.GetByIdAsync(requestId);
            if (request == null) throw new InvalidOperationException("Request not found.");

            request.Status = status;
            if (status == "Closed") request.CompletedAt = DateTime.Now;
            _unitOfWork.MaintenanceRequests.Update(request);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<MaintenanceRequest>> GetOpenRequestsAsync()
            => await _unitOfWork.MaintenanceRequests.Query()
                .Where(m => m.Status != "Closed")
                .Include(m => m.Room)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();

        public async Task<int> GetOpenRequestCountAsync()
            => await _unitOfWork.MaintenanceRequests.CountAsync(m => m.Status != "Closed");
    }
}
