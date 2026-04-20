using System.Linq.Expressions;
using DormitoryManagementSystem.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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

        public void Update(T entity)
        {
            DetachTrackedEntityWithSameKey(entity);
            _dbSet.Update(entity);
        }

        public void Delete(T entity) => _dbSet.Remove(entity);

        public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
            => predicate == null ? await _dbSet.CountAsync() : await _dbSet.CountAsync(predicate);

        public IQueryable<T> Query() => _dbSet.AsQueryable();

        private void DetachTrackedEntityWithSameKey(T entity)
        {
            var entityType = _context.Model.FindEntityType(typeof(T));
            var primaryKey = entityType?.FindPrimaryKey();
            if (primaryKey == null)
                return;

            var keyProperties = primaryKey.Properties
                .Select(property => property.PropertyInfo)
                .Where(propertyInfo => propertyInfo != null)
                .ToArray();

            if (keyProperties.Length != primaryKey.Properties.Count)
                return;

            var keyValues = keyProperties
                .Select(propertyInfo => propertyInfo!.GetValue(entity))
                .ToArray();

            if (keyValues.Any(value => value == null))
                return;

            EntityEntry<T>? trackedEntry = _context.ChangeTracker
                .Entries<T>()
                .FirstOrDefault(entry =>
                    entry.State != EntityState.Detached &&
                    !ReferenceEquals(entry.Entity, entity) &&
                    KeysMatch(entry.Entity, keyProperties, keyValues));

            if (trackedEntry != null)
                trackedEntry.State = EntityState.Detached;
        }

        private static bool KeysMatch(
            T trackedEntity,
            System.Reflection.PropertyInfo?[] keyProperties,
            object?[] keyValues)
        {
            for (int i = 0; i < keyProperties.Length; i++)
            {
                var trackedValue = keyProperties[i]!.GetValue(trackedEntity);
                if (!Equals(trackedValue, keyValues[i]))
                    return false;
            }

            return true;
        }
    }
}
