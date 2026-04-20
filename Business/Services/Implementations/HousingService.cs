using DormitoryManagementSystem.Business.Services.Interfaces;
using DormitoryManagementSystem.DataAccess.Repositories;
using DormitoryManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DormitoryManagementSystem.Business.Services.Implementations
{
    public class HousingService : IHousingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HousingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<HousingRecord>> GetAllRecordsAsync()
            => await _unitOfWork.HousingRecords.Query()
                .Include(h => h.Student)
                .Include(h => h.Room).ThenInclude(r => r!.Building)
                .ToListAsync();

        public async Task<HousingRecord?> GetRecordByIdAsync(int id)
            => await _unitOfWork.HousingRecords.GetByIdAsync(id);

        public async Task CheckInAsync(int studentId, int roomId)
        {
            var existing = await _unitOfWork.HousingRecords
                .FindAsync(h => h.StudentId == studentId && h.HousingStatus == "Active");
            if (existing.Any())
                throw new InvalidOperationException("Student already has an active housing record.");

            var room = await _unitOfWork.Rooms.GetByIdAsync(roomId);
            if (room == null || room.OccupiedBeds >= room.Capacity)
                throw new InvalidOperationException("Room is not available.");

            var record = new HousingRecord
            {
                StudentId = studentId,
                RoomId = roomId,
                CheckInDate = DateTime.Now,
                HousingStatus = "Active"
            };

            room.OccupiedBeds++;
            if (room.OccupiedBeds >= room.Capacity)
                room.Status = "Full";

            await _unitOfWork.HousingRecords.AddAsync(record);
            _unitOfWork.Rooms.Update(room);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task CheckOutAsync(int housingId)
        {
            var record = await _unitOfWork.HousingRecords.GetByIdAsync(housingId);
            if (record == null || record.HousingStatus != "Active")
                throw new InvalidOperationException("Active record not found.");

            record.CheckOutDate = DateTime.Now;
            record.HousingStatus = "CheckedOut";

            var room = await _unitOfWork.Rooms.GetByIdAsync(record.RoomId);
            if (room != null)
            {
                room.OccupiedBeds = Math.Max(0, room.OccupiedBeds - 1);
                if (room.OccupiedBeds < room.Capacity)
                    room.Status = "Available";
                _unitOfWork.Rooms.Update(room);
            }

            _unitOfWork.HousingRecords.Update(record);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task TransferRoomAsync(int housingId, int newRoomId)
        {
            var record = await _unitOfWork.HousingRecords.GetByIdAsync(housingId);
            if (record == null || record.HousingStatus != "Active")
                throw new InvalidOperationException("Active record not found.");

            var oldRoom = await _unitOfWork.Rooms.GetByIdAsync(record.RoomId);
            if (oldRoom != null)
            {
                oldRoom.OccupiedBeds = Math.Max(0, oldRoom.OccupiedBeds - 1);
                if (oldRoom.OccupiedBeds < oldRoom.Capacity) oldRoom.Status = "Available";
                _unitOfWork.Rooms.Update(oldRoom);
            }

            record.CheckOutDate = DateTime.Now;
            record.HousingStatus = "Transferred";
            _unitOfWork.HousingRecords.Update(record);

            var newRoom = await _unitOfWork.Rooms.GetByIdAsync(newRoomId);
            if (newRoom == null || newRoom.OccupiedBeds >= newRoom.Capacity)
                throw new InvalidOperationException("New room is not available.");

            newRoom.OccupiedBeds++;
            if (newRoom.OccupiedBeds >= newRoom.Capacity) newRoom.Status = "Full";
            _unitOfWork.Rooms.Update(newRoom);

            var newRecord = new HousingRecord
            {
                StudentId = record.StudentId,
                RoomId = newRoomId,
                CheckInDate = DateTime.Now,
                HousingStatus = "Active"
            };
            await _unitOfWork.HousingRecords.AddAsync(newRecord);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<HousingRecord>> GetActiveRecordsAsync()
            => await _unitOfWork.HousingRecords.Query()
                .Where(h => h.HousingStatus == "Active")
                .Include(h => h.Student)
                .Include(h => h.Room).ThenInclude(r => r!.Building)
                .ToListAsync();

        public async Task<HousingRecord?> GetActiveRecordByStudentAsync(int studentId)
        {
            var records = await _unitOfWork.HousingRecords
                .FindAsync(h => h.StudentId == studentId && h.HousingStatus == "Active");
            return records.FirstOrDefault();
        }

        /// <summary>
        /// Housing Report: filterable list.
        /// </summary>
        public async Task<IEnumerable<HousingRecord>> GetHousingReportAsync(
            string? buildingName = null,
            string? housingStatus = null,
            int? studentYear = null)
        {
            IQueryable<HousingRecord> query = _unitOfWork.HousingRecords.Query()
                .Include(h => h.Student)
                .Include(h => h.Room).ThenInclude(r => r!.Building);

            if (!string.IsNullOrWhiteSpace(buildingName))
                query = query.Where(h => h.Room!.Building!.BuildingName == buildingName);

            if (!string.IsNullOrWhiteSpace(housingStatus))
                query = query.Where(h => h.HousingStatus == housingStatus);

            if (studentYear.HasValue)
                query = query.Where(h => h.Student!.Year == studentYear.Value);

            return await query
                .OrderByDescending(h => h.HousingStatus == "Active")
                .ThenByDescending(h => h.CheckInDate)
                .ToListAsync();
        }
    }
}
