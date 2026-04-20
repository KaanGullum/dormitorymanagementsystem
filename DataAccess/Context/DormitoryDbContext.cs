using DormitoryManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DormitoryManagementSystem.DataAccess.Context
{
    public class DormitoryDbContext : DbContext
    {
        public DormitoryDbContext(DbContextOptions<DormitoryDbContext> options) : base(options) { }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<HousingRecord> HousingRecords { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Penalty> Penalties { get; set; }
        public DbSet<MaintenanceRequest> MaintenanceRequests { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ===== Role =====
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.RoleId);
                entity.Property(e => e.RoleName).IsRequired().HasMaxLength(50);
            });

            // ===== User =====
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.Username).IsUnique();
                entity.Property(e => e.PasswordHash).IsRequired();

                entity.HasOne(e => e.Role)
                      .WithMany(r => r.Users)
                      .HasForeignKey(e => e.RoleId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ===== Student =====
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.StudentId);
                entity.Property(e => e.StudentNumber).IsRequired().HasMaxLength(20);
                entity.HasIndex(e => e.StudentNumber).IsUnique();
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Program).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Status).HasMaxLength(20).HasDefaultValue("Active");
            });

            // ===== Building =====
            modelBuilder.Entity<Building>(entity =>
            {
                entity.HasKey(e => e.BuildingId);
                entity.Property(e => e.BuildingName).IsRequired().HasMaxLength(100);
            });

            // ===== Room =====
            modelBuilder.Entity<Room>(entity =>
            {
                entity.HasKey(e => e.RoomId);
                entity.Property(e => e.RoomNumber).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Status).HasMaxLength(30).HasDefaultValue("Available");
                entity.Property(e => e.RoomType).HasMaxLength(30);

                entity.HasOne(e => e.Building)
                      .WithMany(b => b.Rooms)
                      .HasForeignKey(e => e.BuildingId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ===== HousingRecord =====
            modelBuilder.Entity<HousingRecord>(entity =>
            {
                entity.HasKey(e => e.HousingId);
                entity.Property(e => e.HousingStatus).HasMaxLength(20).HasDefaultValue("Active");

                entity.HasOne(e => e.Student)
                      .WithMany(s => s.HousingRecords)
                      .HasForeignKey(e => e.StudentId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Room)
                      .WithMany(r => r.HousingRecords)
                      .HasForeignKey(e => e.RoomId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ===== Payment =====
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.PaymentId);
                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Status).HasMaxLength(20).HasDefaultValue("Pending");
                entity.Property(e => e.Semester).HasMaxLength(50);

                entity.HasOne(e => e.Student)
                      .WithMany(s => s.Payments)
                      .HasForeignKey(e => e.StudentId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ===== Penalty =====
            modelBuilder.Entity<Penalty>(entity =>
            {
                entity.HasKey(e => e.PenaltyId);
                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Reason).IsRequired().HasMaxLength(300);
                entity.Property(e => e.Status).HasMaxLength(20).HasDefaultValue("Pending");

                entity.HasOne(e => e.Student)
                      .WithMany(s => s.Penalties)
                      .HasForeignKey(e => e.StudentId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ===== MaintenanceRequest =====
            modelBuilder.Entity<MaintenanceRequest>(entity =>
            {
                entity.HasKey(e => e.RequestId);
                entity.Property(e => e.IssueTitle).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Priority).HasMaxLength(20).HasDefaultValue("Medium");
                entity.Property(e => e.Status).HasMaxLength(20).HasDefaultValue("Open");

                entity.HasOne(e => e.Room)
                      .WithMany(r => r.MaintenanceRequests)
                      .HasForeignKey(e => e.RoomId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.ReportedByStudent)
                      .WithMany(s => s.MaintenanceRequests)
                      .HasForeignKey(e => e.ReportedByStudentId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ===== Document =====
            modelBuilder.Entity<Document>(entity =>
            {
                entity.HasKey(e => e.DocumentId);
                entity.Property(e => e.FileName).IsRequired().HasMaxLength(200);

                entity.HasOne(e => e.Student)
                      .WithMany(s => s.Documents)
                      .HasForeignKey(e => e.StudentId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ===== Notification =====
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.NotificationId);
                entity.Property(e => e.Message).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Type).HasMaxLength(20).HasDefaultValue("Info");

                entity.HasOne(e => e.User)
                      .WithMany(u => u.Notifications)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ===== AuditLog =====
            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.HasKey(e => e.LogId);
                entity.Property(e => e.Action).IsRequired().HasMaxLength(50);
                entity.Property(e => e.ModuleName).IsRequired().HasMaxLength(100);

                entity.HasOne(e => e.User)
                      .WithMany(u => u.AuditLogs)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            SeedData(modelBuilder);
        }

        /// <summary>
        /// Demo seed data. Note: DateTime.Now cannot be used (migrations must be deterministic)
        /// so fixed dates are used.
        /// </summary>
        private void SeedData(ModelBuilder modelBuilder)
        {
            // ===== Roles =====
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, RoleName = "SystemAdmin", Description = "System Administrator" },
                new Role { RoleId = 2, RoleName = "DormitoryManager", Description = "Dormitory Manager" },
                new Role { RoleId = 3, RoleName = "FinanceOfficer", Description = "Finance Officer" },
                new Role { RoleId = 4, RoleName = "DormitoryStaff", Description = "Dormitory Staff" },
                new Role { RoleId = 5, RoleName = "Student", Description = "Student User" }
            );

            // ===== Default Users =====
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    FullName = "System Administrator",
                    Username = "admin",
                    // Password: Admin123! (SHA256 hash)
                    PasswordHash = "PrP+ZrMeO00Q+nC1ytSccRIpSvauTkdqHEBRVdRaoSE=",
                    RoleId = 1,
                    IsActive = true
                },
                new User
                {
                    UserId = 2,
                    FullName = "Student User",
                    Username = "student",
                    // Password: Student123! (SHA256 hash)
                    PasswordHash = "yyQzBz7bJcp38ERdzb1+fFaBlu7xzVTacijd0ickDi4=",
                    RoleId = 5,
                    IsActive = true
                }
            );

            // ===== Buildings =====
            modelBuilder.Entity<Building>().HasData(
                new Building { BuildingId = 1, BuildingName = "Block A", Address = "OSTIM Campus, Block A" },
                new Building { BuildingId = 2, BuildingName = "Block B", Address = "OSTIM Campus, Block B" },
                new Building { BuildingId = 3, BuildingName = "Block C", Address = "OSTIM Campus, Block C" },
                new Building { BuildingId = 4, BuildingName = "Block D", Address = "OSTIM Campus, Block D" }
            );


        }
    }
}
