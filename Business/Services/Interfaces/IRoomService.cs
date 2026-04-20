using DormitoryManagementSystem.Domain.Entities;

namespace DormitoryManagementSystem.Business.Services.Interfaces
{
    /// <summary>
    /// Room Allocation and Occupancy Module.
    /// Tracks buildings, floors, rooms, capacity, occupied beds, available beds, room type, and occupancy status.
    /// </summary>
    public interface IRoomService
    {
        Task<IEnumerable<Room>> GetAllRoomsAsync();
        Task<Room?> GetRoomByIdAsync(int id);
        Task<IEnumerable<Room>> GetRoomsByBuildingAsync(int buildingId);
        Task<bool> RoomExistsInBuildingAsync(int buildingId, string roomNumber);
        Task<IEnumerable<Room>> GetAvailableRoomsAsync();
        Task CreateRoomAsync(Room room);
        Task UpdateRoomAsync(Room room);
        Task AssignStudentToRoomAsync(int roomId, int studentId);
        Task<int> GetAvailableBedsAsync(int roomId);
        Task<IEnumerable<Building>> GetAllBuildingsAsync();
        Task<Dictionary<string, double>> GetOccupancyByBuildingAsync();
    }
}
