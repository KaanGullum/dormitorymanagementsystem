using DormitoryManagementSystem.Domain.Entities;

namespace DormitoryManagementSystem.Business.Services.Interfaces
{
    /// <summary>
    /// Authentication and Authorization Module.
    /// Handles secure login, password management, session control, and role-based authorization.
    /// </summary>
    public interface IAuthService
    {
        Task<User?> LoginAsync(string username, string password);
        Task LogoutAsync(int userId);
        Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword);
        Task<bool> ValidateSessionAsync(int userId);
    }
}
