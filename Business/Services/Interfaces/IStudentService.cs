using DormitoryManagementSystem.Domain.Entities;

namespace DormitoryManagementSystem.Business.Services.Interfaces
{
    /// <summary>
    /// Student and Member Records Module.
    /// </summary>
    public interface IStudentService
    {
        Task<IEnumerable<Student>> GetAllStudentsAsync();
        Task<Student?> GetStudentByIdAsync(int id);
        Task<Student?> GetStudentByNumberAsync(string studentNumber);
        Task CreateStudentAsync(Student student);
        Task UpdateStudentAsync(Student student);
        Task<bool> DeleteStudentAsync(int id);
        Task<IEnumerable<HousingRecord>> ViewHousingHistoryAsync(int studentId);
        Task<IEnumerable<Student>> SearchStudentsAsync(string? keyword, string? building, int? year, string? status);
    }
}
