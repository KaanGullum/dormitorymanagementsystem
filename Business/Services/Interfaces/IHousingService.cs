using DormitoryManagementSystem.Domain.Entities;

namespace DormitoryManagementSystem.Business.Services.Interfaces
{
    /// <summary>
    /// Housing and Check-in/Check-out Module.
    /// Maintains residence periods, active housing records, contract information,
    /// room changes, and entry/exit transactions.
    /// </summary>
    public interface IHousingService
    {
        Task<IEnumerable<HousingRecord>> GetAllRecordsAsync();
        Task<HousingRecord?> GetRecordByIdAsync(int id);
        Task CheckInAsync(int studentId, int roomId);
        Task CheckOutAsync(int housingId);
        Task TransferRoomAsync(int housingId, int newRoomId);
        Task<IEnumerable<HousingRecord>> GetActiveRecordsAsync();
        Task<HousingRecord?> GetActiveRecordByStudentAsync(int studentId);

        /// <summary>
        /// Housing Report filterable list. Returns all if filters are null.
        /// </summary>
        Task<IEnumerable<HousingRecord>> GetHousingReportAsync(
            string? buildingName = null,
            string? housingStatus = null,
            int? studentYear = null);
    }
}
