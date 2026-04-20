using System.Linq.Expressions;

namespace DormitoryManagementSystem.DataAccess.Repositories
{
    /// <summary>
    /// Data Access Layer - Repository pattern with Entity Framework Core.
    /// Generic repository interface: common CRUD operations for every entity.
    /// </summary>
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
        IQueryable<T> Query();
    }
}
