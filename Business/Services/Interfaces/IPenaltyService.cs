using DormitoryManagementSystem.Domain.Entities;

namespace DormitoryManagementSystem.Business.Services.Interfaces
{
    /// <summary>
    /// Penalty Management (second half of Module 6: Fee, Payment, and Penalty).
    /// Dedicated CRUD for penalty records, independent from IPaymentService.
    /// </summary>
    public interface IPenaltyService
    {
        Task<IEnumerable<Penalty>> GetAllAsync();
        Task<Penalty?> GetByIdAsync(int id);
        Task CreateAsync(Penalty penalty);
        Task MarkAsPaidAsync(int penaltyId);
        Task DeleteAsync(int penaltyId);
        Task<decimal> GetTotalPendingAmountAsync();
    }
}
