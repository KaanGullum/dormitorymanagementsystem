using System.Linq.Expressions;
using DormitoryManagementSystem.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace DormitoryManagementSystem.DataAccess.Repositories
{
    /// <summary>
    /// Data Access Layer - Repository pattern implementation with EF Core
    /// for persistent storage.
    /// </summary>
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DormitoryDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(DormitoryDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
            => await _dbSet.Where(predicate).ToListAsync();

        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public void Update(T entity) => _dbSet.Update(entity);

        public void Delete(T entity) => _dbSet.Remove(entity);

        public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
            => predicate == null ? await _dbSet.CountAsync() : await _dbSet.CountAsync(predicate);

        public IQueryable<T> Query() => _dbSet.AsQueryable();
    }
}
