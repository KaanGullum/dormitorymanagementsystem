namespace DormitoryManagementSystem.Business.Services.Interfaces
{
    /// <summary>
    /// Reporting and Dashboard Module.
    /// Provides occupancy summaries, payment charts, overdue balances,
    /// open-request statistics, and exportable report views for managers.
    /// </summary>
    public interface IDashboardService
    {
        Task<int> GetActiveStudentCountAsync();
        Task<double> GetOccupancyRateAsync();
        Task<decimal> GetCollectedFeesAsync();
        Task<int> GetOpenRequestCountAsync();
        Task<Dictionary<string, double>> GetOccupancyByBuildingAsync();
        Task<Dictionary<string, int>> GetPaymentStatusDistributionAsync();
    }
}
