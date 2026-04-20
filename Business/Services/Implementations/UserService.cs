using DormitoryManagementSystem.Business.Services.Interfaces;
using DormitoryManagementSystem.DataAccess.Repositories;
using DormitoryManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DormitoryManagementSystem.Business.Services.Implementations
{
    /// <summary>
    /// User and Role Management Module.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
            => await _unitOfWork.Users.GetAllAsync();

        public async Task<User?> GetUserByIdAsync(int id)
            => await _unitOfWork.Users.GetByIdAsync(id);

        public async Task<User?> GetUserByUsernameAsync(string username)
            => await _unitOfWork.Users.Query()
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Username == username);

        public async Task CreateUserAsync(User user)
        {
            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ToggleUserStatusAsync(int userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user != null)
            {
                user.IsActive = !user.IsActive;
                _unitOfWork.Users.Update(user);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
            => await _unitOfWork.Roles.GetAllAsync();

        public async Task AssignRoleAsync(int userId, int roleId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user != null)
            {
                user.RoleId = roleId;
                _unitOfWork.Users.Update(user);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
