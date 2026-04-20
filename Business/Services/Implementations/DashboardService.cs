using DormitoryManagementSystem.Business.Services.Interfaces;
using DormitoryManagementSystem.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DormitoryManagementSystem.Business.Services.Implementations
{
    /// <summary>
    /// Reporting and Dashboard Module.
    /// Overview Dashboard indicators: Active Students, Occupancy Rate, Collected Fees, Open Requests.
    /// </summary>
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> GetActiveStudentCountAsync()
            => await _unitOfWork.Students.CountAsync(s => s.Status == "Active");

        public async Task<double> GetOccupancyRateAsync()
        {
            var rooms = await _unitOfWork.Rooms.GetAllAsync();
            var roomList = rooms.ToList();

            var totalCapacity = roomList.Sum(r => r.Capacity);
            var totalOccupied = roomList.Sum(r => r.OccupiedBeds);

            return totalCapacity > 0 ? Math.Round((double)totalOccupied / totalCapacity * 100, 1) : 0;
        }

        public async Task<decimal> GetCollectedFeesAsync()
        {
            var paidPayments = await _unitOfWork.Payments.FindAsync(p => p.Status == "Paid");
            return paidPayments.Sum(p => p.Amount);
        }

        public async Task<int> GetOpenRequestCountAsync()
            => await _unitOfWork.MaintenanceRequests.CountAsync(m => m.Status != "Closed");

        /// <summary>
        /// Dashboard: "Occupancy by Building" bar chart (A, B, C, D).
        /// </summary>
        public async Task<Dictionary<string, double>> GetOccupancyByBuildingAsync()
        {
            var buildings = await _unitOfWork.Buildings.Query()
                .Include(b => b.Rooms)
                .ToListAsync();

            var result = new Dictionary<string, double>();
            foreach (var building in buildings)
            {
                var totalCapacity = building.Rooms.Sum(r => r.Capacity);
                var totalOccupied = building.Rooms.Sum(r => r.OccupiedBeds);
                var rate = totalCapacity > 0 ? (double)totalOccupied / totalCapacity * 100 : 0;
                result[building.BuildingName] = Math.Round(rate, 1);
            }
            return result;
        }

        /// <summary>
        /// Dashboard: "Payment Status" donut chart (Paid, Pending, Overdue).
        /// </summary>
        public async Task<Dictionary<string, int>> GetPaymentStatusDistributionAsync()
        {
            var payments = await _unitOfWork.Payments.GetAllAsync();
            var paymentList = payments.ToList();

            return new Dictionary<string, int>
            {
                ["Paid"] = paymentList.Count(p => p.Status == "Paid"),
                ["Pending"] = paymentList.Count(p => p.Status == "Pending"),
                ["Overdue"] = paymentList.Count(p => p.Status == "Overdue")
            };
        }
    }
}
