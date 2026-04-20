using DormitoryManagementSystem.Business.Services.Implementations;
using DormitoryManagementSystem.Business.Services.Interfaces;
using DormitoryManagementSystem.DataAccess.Context;
using DormitoryManagementSystem.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ===== Database Context =====
builder.Services.AddDbContext<DormitoryDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("DormitoryManagementSystem.DataAccess")
    ));

// ===== Dependency Injection =====
// Data Access Layer
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Business Layer - Service Registrations
builder.Services.AddScoped<IAuthService, AuthService>();           // Module 1
builder.Services.AddScoped<IUserService, UserService>();           // Module 2
builder.Services.AddScoped<IStudentService, StudentService>();     // Module 3
builder.Services.AddScoped<IRoomService, RoomService>();           // Module 4
builder.Services.AddScoped<IHousingService, HousingService>();     // Module 5
builder.Services.AddScoped<IPaymentService, PaymentService>();     // Module 6
builder.Services.AddScoped<IPenaltyService, PenaltyService>();     // Module 6 (penalty management)
builder.Services.AddScoped<IMaintenanceService, MaintenanceService>(); // Module 7
builder.Services.AddScoped<IDashboardService, DashboardService>(); // Module 8
builder.Services.AddScoped<INotificationAuditService, NotificationAuditService>(); // Module 9
// Module 10 (System Settings) is managed via appsettings.json

// ===== Authentication =====
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

builder.Services.AddAuthorization();

// ===== MVC =====
builder.Services.AddControllersWithViews();

// Session support
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Apply EF Core migrations to update the database.
// Migrate() applies all pending migrations and is ready for schema changes.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DormitoryDbContext>();
    db.Database.Migrate();
}

// ===== Middleware Pipeline =====
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// ===== Route Mapping =====
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
