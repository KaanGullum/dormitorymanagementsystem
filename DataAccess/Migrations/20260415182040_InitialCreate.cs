using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DormitoryManagementSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Buildings",
                columns: table => new
                {
                    BuildingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BuildingName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buildings", x => x.BuildingId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Program = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Active")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.StudentId);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    RoomId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BuildingId = table.Column<int>(type: "int", nullable: false),
                    FloorNo = table.Column<int>(type: "int", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    OccupiedBeds = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, defaultValue: "Available"),
                    RoomType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.RoomId);
                    table.ForeignKey(
                        name: "FK_Rooms_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "BuildingId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    DocumentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.DocumentId);
                    table.ForeignKey(
                        name: "FK_Documents_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaidDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Pending"),
                    Semester = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_Payments_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Penalties",
                columns: table => new
                {
                    PenaltyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Pending")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Penalties", x => x.PenaltyId);
                    table.ForeignKey(
                        name: "FK_Penalties_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HousingRecords",
                columns: table => new
                {
                    HousingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    CheckInDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckOutDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HousingStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Active")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HousingRecords", x => x.HousingId);
                    table.ForeignKey(
                        name: "FK_HousingRecords_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HousingRecords_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceRequests",
                columns: table => new
                {
                    RequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    ReportedByStudentId = table.Column<int>(type: "int", nullable: false),
                    IssueTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Priority = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Medium"),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Open"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AssignedStaffId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceRequests", x => x.RequestId);
                    table.ForeignKey(
                        name: "FK_MaintenanceRequests_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaintenanceRequests_Students_ReportedByStudentId",
                        column: x => x.ReportedByStudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    LogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ModuleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ActionTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.LogId);
                    table.ForeignKey(
                        name: "FK_AuditLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Info"),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Buildings",
                columns: new[] { "BuildingId", "Address", "BuildingName" },
                values: new object[,]
                {
                    { 1, "OSTİM Kampüs, A Blok", "A Blok" },
                    { 2, "OSTİM Kampüs, B Blok", "B Blok" },
                    { 3, "OSTİM Kampüs, C Blok", "C Blok" },
                    { 4, "OSTİM Kampüs, D Blok", "D Blok" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "Description", "RoleName" },
                values: new object[,]
                {
                    { 1, "Sistem Yöneticisi", "SystemAdmin" },
                    { 2, "Yurt Müdürü", "DormitoryManager" },
                    { 3, "Mali İşler Personeli", "FinanceOfficer" },
                    { 4, "Yurt Personeli", "DormitoryStaff" }
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "StudentId", "Email", "FullName", "Phone", "Program", "Status", "StudentNumber", "Year" },
                values: new object[,]
                {
                    { 1, "ayse.yilmaz@student.ostimteknik.edu.tr", "Ayşe Yılmaz", "05551234567", "Computer Engineering", "Active", "2026001", 2 },
                    { 2, "mehmet.kaya@student.ostimteknik.edu.tr", "Mehmet Kaya", "05552345678", "Software Engineering", "Active", "2026012", 1 },
                    { 3, "elif.demir@student.ostimteknik.edu.tr", "Elif Demir", "05553456789", "Industrial Engineering", "Active", "2026033", 3 },
                    { 4, "burak.sahin@student.ostimteknik.edu.tr", "Burak Şahin", "05554567890", "Mechanical Engineering", "Active", "2026060", 4 },
                    { 5, "zeynep.arslan@student.ostimteknik.edu.tr", "Zeynep Arslan", "05555678901", "Civil Engineering", "Left", "2026088", 2 },
                    { 6, "deniz.ozturk@student.ostimteknik.edu.tr", "Deniz Öztürk", "05556789012", "Electrical Engineering", "Active", "2026105", 1 },
                    { 7, "canan.aydin@student.ostimteknik.edu.tr", "Canan Aydın", "05557890123", "Software Engineering", "Active", "2026122", 3 },
                    { 8, "emre.celik@student.ostimteknik.edu.tr", "Emre Çelik", "05558901234", "Computer Engineering", "Active", "2026140", 2 }
                });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "PaymentId", "Amount", "DueDate", "PaidDate", "Semester", "Status", "StudentId" },
                values: new object[,]
                {
                    { 1, 2500m, new DateTime(2025, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "2025-2026 Güz", "Paid", 1 },
                    { 2, 2500m, new DateTime(2025, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 10, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "2025-2026 Güz", "Paid", 2 },
                    { 3, 2500m, new DateTime(2025, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "2025-2026 Güz", "Pending", 3 },
                    { 4, 2500m, new DateTime(2025, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "2025-2026 Güz", "Overdue", 4 },
                    { 5, 2750m, new DateTime(2026, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "2025-2026 Bahar", "Paid", 1 },
                    { 6, 2750m, new DateTime(2026, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "2025-2026 Bahar", "Pending", 2 },
                    { 7, 2750m, new DateTime(2026, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "2025-2026 Bahar", "Paid", 6 },
                    { 8, 2750m, new DateTime(2026, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "2025-2026 Bahar", "Pending", 7 }
                });

            migrationBuilder.InsertData(
                table: "Penalties",
                columns: new[] { "PenaltyId", "Amount", "DueDate", "Reason", "Status", "StudentId" },
                values: new object[,]
                {
                    { 1, 125m, new DateTime(2025, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Geç ödeme gecikme cezası", "Pending", 4 },
                    { 2, 250m, new DateTime(2025, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Yurt kuralları ihlali (gürültü)", "Pending", 3 }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "RoomId", "BuildingId", "Capacity", "FloorNo", "OccupiedBeds", "RoomNumber", "RoomType", "Status" },
                values: new object[,]
                {
                    { 1, 1, 2, 2, 2, "A-203", "Double", "Full" },
                    { 2, 1, 2, 2, 2, "A-204", "Double", "Full" },
                    { 3, 2, 3, 1, 2, "B-113", "Triple", "Available" },
                    { 4, 2, 2, 1, 0, "B-115", "Double", "Available" },
                    { 5, 3, 4, 3, 0, "C-307", "Quad", "Available" },
                    { 6, 3, 2, 3, 0, "C-310", "Double", "Available" },
                    { 7, 4, 3, 1, 0, "D-104", "Triple", "Available" },
                    { 8, 4, 2, 1, 0, "D-109", "Double", "Available" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "FullName", "IsActive", "PasswordHash", "RoleId", "Username" },
                values: new object[] { 1, "Sistem Yöneticisi", true, "PrP+ZrMeO00Q+nC1ytSccRIpSvauTkdqHEBRVdRaoSE=", 1, "admin" });

            migrationBuilder.InsertData(
                table: "HousingRecords",
                columns: new[] { "HousingId", "CheckInDate", "CheckOutDate", "HousingStatus", "RoomId", "StudentId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Active", 1, 1 },
                    { 2, new DateTime(2025, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Active", 1, 2 },
                    { 3, new DateTime(2025, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Active", 2, 3 },
                    { 4, new DateTime(2025, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Active", 2, 4 },
                    { 5, new DateTime(2025, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "CheckedOut", 3, 5 },
                    { 6, new DateTime(2025, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Active", 3, 6 },
                    { 7, new DateTime(2025, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Active", 3, 7 }
                });

            migrationBuilder.InsertData(
                table: "MaintenanceRequests",
                columns: new[] { "RequestId", "AssignedStaffId", "CompletedAt", "CreatedAt", "IssueTitle", "Priority", "ReportedByStudentId", "RoomId", "Status" },
                values: new object[,]
                {
                    { 1, null, null, new DateTime(2025, 10, 8, 10, 30, 0, 0, DateTimeKind.Unspecified), "Musluk sızıntısı var", "High", 1, 1, "Assigned" },
                    { 2, null, null, new DateTime(2025, 10, 15, 14, 15, 0, 0, DateTimeKind.Unspecified), "Isıtıcı çalışmıyor", "Medium", 6, 3, "Open" },
                    { 3, null, new DateTime(2025, 9, 5, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 1, 9, 0, 0, 0, DateTimeKind.Unspecified), "Pencere camı çatlak", "Low", 3, 2, "Closed" }
                });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "NotificationId", "CreatedAt", "IsRead", "Message", "Type", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 4, 1, 9, 0, 0, 0, DateTimeKind.Unspecified), false, "Aylık yurt doluluk raporu hazırlandı.", "Info", 1 },
                    { 2, new DateTime(2026, 4, 10, 14, 0, 0, 0, DateTimeKind.Unspecified), false, "2 adet açık bakım talebi bulunmakta.", "Warning", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_UserId",
                table: "AuditLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_StudentId",
                table: "Documents",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_HousingRecords_RoomId",
                table: "HousingRecords",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_HousingRecords_StudentId",
                table: "HousingRecords",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequests_ReportedByStudentId",
                table: "MaintenanceRequests",
                column: "ReportedByStudentId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequests_RoomId",
                table: "MaintenanceRequests",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_StudentId",
                table: "Payments",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Penalties_StudentId",
                table: "Penalties",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_BuildingId",
                table: "Rooms",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_StudentNumber",
                table: "Students",
                column: "StudentNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "HousingRecords");

            migrationBuilder.DropTable(
                name: "MaintenanceRequests");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Penalties");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Buildings");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
