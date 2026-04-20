# Dormitory Management System (DMS)

**OSTIM Technical University - SENG321 - 2026 Spring Semester**

Prepared by: Ulas Sarp Kaya (220201035), Emirhan Yildiz (220201016), Kaan Gullu (220201007)

A layered ASP.NET Core MVC application that digitalises dormitory operations:
students, rooms, housing check-in/out, payments, penalties, maintenance
requests, reports, notifications and audit logs.

---

## How to Run (3 steps)

### 1. Prerequisites

- .NET 8 SDK: <https://dotnet.microsoft.com/download/dotnet/8.0>
- SQL Server LocalDB (installed with Visual Studio, or via "SQL Server Express LocalDB" installer)
- (Optional, only if you want to run `dotnet ef` manually) EF Core CLI:
  ```
  dotnet tool install --global dotnet-ef --version 8.0.0
  ```

### 2. Clone and run

```
git clone https://github.com/emirhan2823/dormitory-management-system.git
cd dormitory-management-system/Web
dotnet run
```

That is all. The application will:

1. Create the `DormitoryManagementDB` database on `(localdb)\mssqllocaldb` automatically
   (via `db.Database.Migrate()` in `Program.cs`).
2. Apply every migration in `DataAccess/Migrations/` - which also **seeds all demo data**:
   5 roles, 2 users, 4 buildings, 8 rooms, 8 students, 7 housing records,
   8 payments, 2 penalties, 3 maintenance requests, 2 notifications.
3. Start listening on the URLs printed in the console (typically
   <https://localhost:7xxx> or <http://localhost:5xxx>).

> The database and all demo records live inside the migration files in the
> repository, so you do not need a `.mdf` backup. Cloning the repo and running
> `dotnet run` is enough for a live demo.

### 3. Log in

| Role | Username | Password | What you can do |
|------|----------|----------|-----------------|
| System Admin | `admin` | `Admin123!` | Every module, including Users and Audit Logs |
| Student | `student` | `Student123!` | Limited view: Dashboard welcome + Rooms + Maintenance |

---

## Screens / Modules

| Sidebar item | URL | Purpose |
|--------------|-----|---------|
| Dashboard | `/Home/Index` | 4 KPI cards + Occupancy by Building + Payment Status donut |
| Students | `/Student/Index` | Student Report with Room, Building, Check-in, filters (Fig 5.2) |
| Rooms | `/Room/Index` | Room list, occupancy, room type; Assign Student flow |
| Payments | `/Payment/Index` | Payment list, Overdue view, student-level payments |
| Maintenance | `/Maintenance/Index` | Maintenance requests, create/close, priority |
| Reports | `/Report/Index` | Student Report, Housing Report, Payment & Maintenance Report |
| Settings | `/Settings/Index` | Values from `appsettings.json` (semester, capacity, etc.) |
| Users (admin) | `/User/Index` | CRUD users and roles |
| Audit Logs (admin) | `/Report/AuditLog` | Every Create/Update/Delete/Login/Logout action |

---

## Architecture (4-layer, MVC)

```
Domain/                    POCO entities (12), Data Annotations, pure domain methods
DataAccess/                EF Core DbContext, Repository<T>, IUnitOfWork, Migrations
Business/                  Service interfaces + implementations (Auth, User, Student,
                           Room, Housing, Payment, Penalty, Maintenance, Dashboard,
                           NotificationAudit)
Web/                       ASP.NET Core MVC: Controllers, Views, ViewModels,
                           ViewComponents, wwwroot (Bootstrap 5.3 + Chart.js)
```

- **Presentation** - Razor Views + Bootstrap 5.3 + Chart.js
- **Business** - Service classes, DI-registered in `Program.cs`
- **Data Access** - EF Core 8 + Repository + UnitOfWork patterns
- **Domain** - Plain entities; FKs/unique indexes defined via Fluent API
- **Auth** - Cookie authentication with role claims
  (`SystemAdmin`, `DormitoryManager`, `FinanceOfficer`, `DormitoryStaff`, `Student`)

---

## Database

- Provider: SQL Server LocalDB (`(localdb)\mssqllocaldb`)
- Database name: `DormitoryManagementDB`
- Connection string: `Web/appsettings.json` -> `DefaultConnection`
- Schema: 12 tables in 3NF
  (Roles, Users, Students, Buildings, Rooms, HousingRecords, Payments,
  Penalties, MaintenanceRequests, Documents, Notifications, AuditLogs)
- Migrations:
  1. `InitialCreate` - all tables + initial seed
  2. `AddStudentRoleAndUser` - Student role and user
  3. `TranslateSeedDataToEnglish` - seed data normalized to ASCII / English

### Reset database (if you need a clean state)

```
sqlcmd -S "(localdb)\mssqllocaldb" -Q "DROP DATABASE DormitoryManagementDB"
cd dormitory-management-system/Web
dotnet run
```

On start-up the database will be recreated from migrations and re-seeded.

---

## Demo tip for presentation

1. `git clone https://github.com/emirhan2823/dormitory-management-system.git`
2. `cd dormitory-management-system/Web && dotnet run`
3. Open the printed URL, log in as **admin / Admin123!**
4. Walk through the Dashboard, then Students -> Reports -> Audit Logs.
5. Log out, log back in as **student / Student123!** to show the restricted
   student view (Dashboard welcome, Rooms, Maintenance only).
