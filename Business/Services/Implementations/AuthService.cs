using DormitoryManagementSystem.Business.Services.Interfaces;
using DormitoryManagementSystem.DataAccess.Repositories;
using DormitoryManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DormitoryManagementSystem.Business.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<User?> LoginAsync(string username, string password)
        {
            var user = _unitOfWork.Users.Query()
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Username == username && u.IsActive);

            if (user == null) return Task.FromResult<User?>(null);

            var hash = HashPassword(password);
            if (user.PasswordHash != hash) return Task.FromResult<User?>(null);

            return Task.FromResult<User?>(user);
        }

        public async Task LogoutAsync(int userId)
        {
            // Session invalidation is handled via ASP.NET session
            await Task.CompletedTask;
        }

        public async Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null) return false;

            var oldHash = HashPassword(oldPassword);
            if (user.PasswordHash != oldHash) return false;

            user.PasswordHash = HashPassword(newPassword);
            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ValidateSessionAsync(int userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            return user != null && user.IsActive;
        }

        /// <summary>
        /// Simple SHA256 hash. Production should use BCrypt.
        /// </summary>
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
