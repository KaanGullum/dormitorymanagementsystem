using DormitoryManagementSystem.Business.Services.Interfaces;
using DormitoryManagementSystem.DataAccess.Repositories;
using DormitoryManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DormitoryManagementSystem.Business.Services.Implementations
{
    public class PenaltyService : IPenaltyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PenaltyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Penalty>> GetAllAsync()
            => await _unitOfWork.Penalties.Query()
                .Include(p => p.Student)
                .OrderByDescending(p => p.DueDate)
                .ToListAsync();

        public async Task<Penalty?> GetByIdAsync(int id)
            => await _unitOfWork.Penalties.GetByIdAsync(id);

        public async Task CreateAsync(Penalty penalty)
        {
            penalty.Status = string.IsNullOrWhiteSpace(penalty.Status) ? "Pending" : penalty.Status;
            await _unitOfWork.Penalties.AddAsync(penalty);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task MarkAsPaidAsync(int penaltyId)
        {
            var penalty = await _unitOfWork.Penalties.GetByIdAsync(penaltyId);
            if (penalty == null)
                throw new InvalidOperationException("Penalty record not found.");
            if (penalty.Status == "Paid")
                throw new InvalidOperationException("This penalty is already paid.");

            penalty.Status = "Paid";
            _unitOfWork.Penalties.Update(penalty);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int penaltyId)
        {
            var penalty = await _unitOfWork.Penalties.GetByIdAsync(penaltyId);
            if (penalty == null) return;
            _unitOfWork.Penalties.Delete(penalty);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<decimal> GetTotalPendingAmountAsync()
            => await _unitOfWork.Penalties.Query()
                .Where(p => p.Status != "Paid")
                .SumAsync(p => p.Amount);
    }
}
