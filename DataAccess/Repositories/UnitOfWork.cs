using DormitoryManagementSystem.DataAccess.Context;
using DormitoryManagementSystem.Domain.Entities;

namespace DormitoryManagementSystem.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DormitoryDbContext _context;

        private IRepository<Role>? _roles;
        private IRepository<User>? _users;
        private IRepository<Student>? _students;
        private IRepository<Building>? _buildings;
        private IRepository<Room>? _rooms;
        private IRepository<HousingRecord>? _housingRecords;
        private IRepository<Payment>? _payments;
        private IRepository<Penalty>? _penalties;
        private IRepository<MaintenanceRequest>? _maintenanceRequests;
        private IRepository<Document>? _documents;
        private IRepository<Notification>? _notifications;
        private IRepository<AuditLog>? _auditLogs;

        public UnitOfWork(DormitoryDbContext context)
        {
            _context = context;
        }

        public IRepository<Role> Roles => _roles ??= new Repository<Role>(_context);
        public IRepository<User> Users => _users ??= new Repository<User>(_context);
        public IRepository<Student> Students => _students ??= new Repository<Student>(_context);
        public IRepository<Building> Buildings => _buildings ??= new Repository<Building>(_context);
        public IRepository<Room> Rooms => _rooms ??= new Repository<Room>(_context);
        public IRepository<HousingRecord> HousingRecords => _housingRecords ??= new Repository<HousingRecord>(_context);
        public IRepository<Payment> Payments => _payments ??= new Repository<Payment>(_context);
        public IRepository<Penalty> Penalties => _penalties ??= new Repository<Penalty>(_context);
        public IRepository<MaintenanceRequest> MaintenanceRequests => _maintenanceRequests ??= new Repository<MaintenanceRequest>(_context);
        public IRepository<Document> Documents => _documents ??= new Repository<Document>(_context);
        public IRepository<Notification> Notifications => _notifications ??= new Repository<Notification>(_context);
        public IRepository<AuditLog> AuditLogs => _auditLogs ??= new Repository<AuditLog>(_context);

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}
