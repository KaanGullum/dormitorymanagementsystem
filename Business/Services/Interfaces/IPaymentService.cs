using DormitoryManagementSystem.Domain.Entities;

namespace DormitoryManagementSystem.Business.Services.Interfaces
{
    /// <summary>
    /// Fee, Payment, and Penalty Management Module.
    /// </summary>
    public interface IPaymentService
    {
        Task<IEnumerable<Payment>> GetAllPaymentsAsync();
        Task<Payment?> GetPaymentByIdAsync(int id);
        Task<IEnumerable<Payment>> GetPaymentsByStudentAsync(int studentId);
        Task CreatePaymentAsync(Payment payment);
        Task RecordPaymentAsync(int paymentId);
        Task<decimal> CalculateBalanceAsync(int studentId);
        Task<IEnumerable<Payment>> GetOverduePaymentsAsync();

        // Penalty
        Task<IEnumerable<Penalty>> GetPenaltiesByStudentAsync(int studentId);
        Task CreatePenaltyAsync(Penalty penalty);
        Task<decimal> GetTotalCollectedAsync();
        Task<decimal> GetTotalPendingAsync();
        Task<decimal> GetTotalOverdueAsync();
    }
}
