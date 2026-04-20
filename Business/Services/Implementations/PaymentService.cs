using DormitoryManagementSystem.Business.Services.Interfaces;
using DormitoryManagementSystem.DataAccess.Repositories;
using DormitoryManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DormitoryManagementSystem.Business.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
            => await _unitOfWork.Payments.Query().Include(p => p.Student).ToListAsync();

        public async Task<Payment?> GetPaymentByIdAsync(int id)
            => await _unitOfWork.Payments.GetByIdAsync(id);

        public async Task<IEnumerable<Payment>> GetPaymentsByStudentAsync(int studentId)
            => await _unitOfWork.Payments.FindAsync(p => p.StudentId == studentId);

        public async Task CreatePaymentAsync(Payment payment)
        {
            await _unitOfWork.Payments.AddAsync(payment);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RecordPaymentAsync(int paymentId)
        {
            var payment = await _unitOfWork.Payments.GetByIdAsync(paymentId);
            if (payment == null)
                throw new InvalidOperationException("Payment record not found.");

            payment.PaidDate = DateTime.Now;
            payment.Status = "Paid";
            _unitOfWork.Payments.Update(payment);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<decimal> CalculateBalanceAsync(int studentId)
        {
            var payments = await _unitOfWork.Payments.FindAsync(p => p.StudentId == studentId);
            var penalties = await _unitOfWork.Penalties.FindAsync(p => p.StudentId == studentId);

            var totalDue = payments.Where(p => p.Status != "Paid").Sum(p => p.Amount);
            var totalPenalties = penalties.Where(p => p.Status != "Paid").Sum(p => p.Amount);

            return totalDue + totalPenalties;
        }

        public async Task<IEnumerable<Payment>> GetOverduePaymentsAsync()
            => await _unitOfWork.Payments.Query()
                .Include(p => p.Student)
                .Where(p => p.Status == "Overdue" || (p.Status == "Pending" && p.DueDate < DateTime.Now))
                .ToListAsync();

        public async Task<IEnumerable<Penalty>> GetPenaltiesByStudentAsync(int studentId)
            => await _unitOfWork.Penalties.FindAsync(p => p.StudentId == studentId);

        public async Task CreatePenaltyAsync(Penalty penalty)
        {
            await _unitOfWork.Penalties.AddAsync(penalty);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<decimal> GetTotalCollectedAsync()
        {
            var paid = await _unitOfWork.Payments.FindAsync(p => p.Status == "Paid");
            return paid.Sum(p => p.Amount);
        }

        public async Task<decimal> GetTotalPendingAsync()
        {
            var pending = await _unitOfWork.Payments.FindAsync(p => p.Status == "Pending");
            return pending.Sum(p => p.Amount);
        }

        public async Task<decimal> GetTotalOverdueAsync()
        {
            var overdue = await _unitOfWork.Payments.FindAsync(p => p.Status == "Overdue");
            return overdue.Sum(p => p.Amount);
        }
    }
}
