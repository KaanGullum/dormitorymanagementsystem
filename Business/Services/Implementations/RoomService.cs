using DormitoryManagementSystem.Business.Services.Interfaces;
using DormitoryManagementSystem.DataAccess.Repositories;
using DormitoryManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DormitoryManagementSystem.Business.Services.Implementations
{
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoomService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Room>> GetAllRoomsAsync()
            => await _unitOfWork.Rooms.Query().Include(r => r.Building).ToListAsync();

        public async Task<Room?> GetRoomByIdAsync(int id)
            => await _unitOfWork.Rooms.GetByIdAsync(id);

        public async Task<IEnumerable<Room>> GetRoomsByBuildingAsync(int buildingId)
            => await _unitOfWork.Rooms.FindAsync(r => r.BuildingId == buildingId);

        public async Task<bool> RoomExistsInBuildingAsync(int buildingId, string roomNumber)
            => await _unitOfWork.Rooms.CountAsync(r => r.BuildingId == buildingId && r.RoomNumber == roomNumber) > 0;

        public async Task<IEnumerable<Room>> GetAvailableRoomsAsync()
            => await _unitOfWork.Rooms.FindAsync(r => r.OccupiedBeds < r.Capacity && r.Status == "Available");

        public async Task CreateRoomAsync(Room room)
        {
            await _unitOfWork.Rooms.AddAsync(room);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateRoomAsync(Room room)
        {
            _unitOfWork.Rooms.Update(room);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AssignStudentToRoomAsync(int roomId, int studentId)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(roomId);
            if (room == null || room.OccupiedBeds >= room.Capacity)
                throw new InvalidOperationException("Room is full or not found.");

            room.OccupiedBeds++;
            if (room.OccupiedBeds >= room.Capacity)
                room.Status = "Full";

            _unitOfWork.Rooms.Update(room);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<int> GetAvailableBedsAsync(int roomId)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(roomId);
            return room?.GetAvailableBeds() ?? 0;
        }

        public async Task<IEnumerable<Building>> GetAllBuildingsAsync()
            => await _unitOfWork.Buildings.GetAllAsync();

        /// <summary>
        /// Dashboard: Occupancy by Building chart data.
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
    }
}
