using DormitoryManagementSystem.Domain.Entities;

namespace DormitoryManagementSystem.Business.Services.Interfaces
{
    /// <summary>
    /// User and Role Management Module.
    /// Manages staff accounts, role definitions, permission assignments, activation status.
    /// </summary>
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByUsernameAsync(string username);
        Task CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task ToggleUserStatusAsync(int userId);
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task AssignRoleAsync(int userId, int roleId);
    }
}
