using DormitoryManagementSystem.Business.Services.Interfaces;
using DormitoryManagementSystem.DataAccess.Repositories;
using DormitoryManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DormitoryManagementSystem.Business.Services.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StudentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
            => await _unitOfWork.Students.GetAllAsync();

        public async Task<Student?> GetStudentByIdAsync(int id)
            => await _unitOfWork.Students.GetByIdAsync(id);

        public async Task<Student?> GetStudentByNumberAsync(string studentNumber)
            => await _unitOfWork.Students.Query()
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.StudentNumber == studentNumber);

        public async Task CreateStudentAsync(Student student)
        {
            await _unitOfWork.Students.AddAsync(student);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateStudentAsync(Student student)
        {
            _unitOfWork.Students.Update(student);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await _unitOfWork.Students.GetByIdAsync(id);
            if (student == null) return false;

            // 1. Handle housing records — check out active ones and update room occupancy
            var housingRecords = await _unitOfWork.HousingRecords
                .FindAsync(h => h.StudentId == id);
            foreach (var housing in housingRecords)
            {
                // If still checked in, update the room's current occupancy
                if (housing.CheckOutDate == null)
                {
                    var room = await _unitOfWork.Rooms.GetByIdAsync(housing.RoomId);
                    if (room != null && room.OccupiedBeds > 0)
                    {
                        room.OccupiedBeds--;
                        if (room.OccupiedBeds == 0) room.Status = "Available";
                        _unitOfWork.Rooms.Update(room);
                    }
                }
                _unitOfWork.HousingRecords.Delete(housing);
            }

            // 2. Remove payments
            var payments = await _unitOfWork.Payments.FindAsync(p => p.StudentId == id);
            foreach (var payment in payments)
                _unitOfWork.Payments.Delete(payment);

            // 3. Remove penalties
            var penalties = await _unitOfWork.Penalties.FindAsync(p => p.StudentId == id);
            foreach (var penalty in penalties)
                _unitOfWork.Penalties.Delete(penalty);

            // 4. Remove maintenance requests
            var maintenanceRequests = await _unitOfWork.MaintenanceRequests
                .FindAsync(m => m.ReportedByStudentId == id);
            foreach (var request in maintenanceRequests)
                _unitOfWork.MaintenanceRequests.Delete(request);

            // 5. Remove documents
            var documents = await _unitOfWork.Documents.FindAsync(d => d.StudentId == id);
            foreach (var document in documents)
                _unitOfWork.Documents.Delete(document);

            // 6. Finally delete the student
            _unitOfWork.Students.Delete(student);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<HousingRecord>> ViewHousingHistoryAsync(int studentId)
            => await _unitOfWork.HousingRecords.FindAsync(h => h.StudentId == studentId);

        /// <summary>
        /// Student Report filtering: search by keyword and filter by building, year, status.
        /// </summary>
        public async Task<IEnumerable<Student>> SearchStudentsAsync(
            string? keyword, string? building, int? year, string? status)
        {
            var query = _unitOfWork.Students.Query();

            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(s => s.FullName.Contains(keyword) || s.StudentNumber.Contains(keyword));

            if (year.HasValue)
                query = query.Where(s => s.Year == year.Value);

            if (!string.IsNullOrEmpty(status))
                query = query.Where(s => s.Status == status);

            return await query.ToListAsync();
        }
    }
}
