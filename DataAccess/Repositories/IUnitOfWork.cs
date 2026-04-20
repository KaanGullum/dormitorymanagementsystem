using DormitoryManagementSystem.Domain.Entities;

namespace DormitoryManagementSystem.DataAccess.Repositories
{
    /// <summary>
    /// Repository pattern with Entity Framework Core: persistent storage,
    /// queries, and transaction handling. UnitOfWork groups multiple
    /// repositories under a single SaveChangesAsync transaction.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Role> Roles { get; }
        IRepository<User> Users { get; }
        IRepository<Student> Students { get; }
        IRepository<Building> Buildings { get; }
        IRepository<Room> Rooms { get; }
        IRepository<HousingRecord> HousingRecords { get; }
        IRepository<Payment> Payments { get; }
        IRepository<Penalty> Penalties { get; }
        IRepository<MaintenanceRequest> MaintenanceRequests { get; }
        IRepository<Document> Documents { get; }
        IRepository<Notification> Notifications { get; }
        IRepository<AuditLog> AuditLogs { get; }

        Task<int> SaveChangesAsync();
    }
}
