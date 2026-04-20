using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DormitoryManagementSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class TranslateSeedDataToEnglish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Buildings",
                keyColumn: "BuildingId",
                keyValue: 1,
                columns: new[] { "Address", "BuildingName" },
                values: new object[] { "OSTIM Campus, Block A", "Block A" });

            migrationBuilder.UpdateData(
                table: "Buildings",
                keyColumn: "BuildingId",
                keyValue: 2,
                columns: new[] { "Address", "BuildingName" },
                values: new object[] { "OSTIM Campus, Block B", "Block B" });

            migrationBuilder.UpdateData(
                table: "Buildings",
                keyColumn: "BuildingId",
                keyValue: 3,
                columns: new[] { "Address", "BuildingName" },
                values: new object[] { "OSTIM Campus, Block C", "Block C" });

            migrationBuilder.UpdateData(
                table: "Buildings",
                keyColumn: "BuildingId",
                keyValue: 4,
                columns: new[] { "Address", "BuildingName" },
                values: new object[] { "OSTIM Campus, Block D", "Block D" });

            migrationBuilder.UpdateData(
                table: "MaintenanceRequests",
                keyColumn: "RequestId",
                keyValue: 1,
                column: "IssueTitle",
                value: "Tap leakage");

            migrationBuilder.UpdateData(
                table: "MaintenanceRequests",
                keyColumn: "RequestId",
                keyValue: 2,
                column: "IssueTitle",
                value: "Heater not working");

            migrationBuilder.UpdateData(
                table: "MaintenanceRequests",
                keyColumn: "RequestId",
                keyValue: 3,
                column: "IssueTitle",
                value: "Window glass cracked");

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 1,
                column: "Message",
                value: "Monthly dormitory occupancy report is ready.");

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 2,
                column: "Message",
                value: "There are 2 open maintenance requests.");

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 1,
                column: "Semester",
                value: "2025-2026 Fall");

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 2,
                column: "Semester",
                value: "2025-2026 Fall");

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 3,
                column: "Semester",
                value: "2025-2026 Fall");

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 4,
                column: "Semester",
                value: "2025-2026 Fall");

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 5,
                column: "Semester",
                value: "2025-2026 Spring");

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 6,
                column: "Semester",
                value: "2025-2026 Spring");

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 7,
                column: "Semester",
                value: "2025-2026 Spring");

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 8,
                column: "Semester",
                value: "2025-2026 Spring");

            migrationBuilder.UpdateData(
                table: "Penalties",
                keyColumn: "PenaltyId",
                keyValue: 1,
                column: "Reason",
                value: "Late payment fee");

            migrationBuilder.UpdateData(
                table: "Penalties",
                keyColumn: "PenaltyId",
                keyValue: 2,
                column: "Reason",
                value: "Dormitory rules violation (noise)");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                column: "Description",
                value: "System Administrator");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                column: "Description",
                value: "Dormitory Manager");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                column: "Description",
                value: "Finance Officer");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 4,
                column: "Description",
                value: "Dormitory Staff");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 5,
                column: "Description",
                value: "Student User");

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 1,
                column: "FullName",
                value: "Ayse Yilmaz");

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 4,
                column: "FullName",
                value: "Burak Sahin");

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 6,
                column: "FullName",
                value: "Deniz Ozturk");

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 7,
                column: "FullName",
                value: "Canan Aydin");

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 8,
                column: "FullName",
                value: "Emre Celik");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "FullName",
                value: "System Administrator");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                column: "FullName",
                value: "Student User");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Buildings",
                keyColumn: "BuildingId",
                keyValue: 1,
                columns: new[] { "Address", "BuildingName" },
                values: new object[] { "OSTİM Kampüs, A Blok", "A Blok" });

            migrationBuilder.UpdateData(
                table: "Buildings",
                keyColumn: "BuildingId",
                keyValue: 2,
                columns: new[] { "Address", "BuildingName" },
                values: new object[] { "OSTİM Kampüs, B Blok", "B Blok" });

            migrationBuilder.UpdateData(
                table: "Buildings",
                keyColumn: "BuildingId",
                keyValue: 3,
                columns: new[] { "Address", "BuildingName" },
                values: new object[] { "OSTİM Kampüs, C Blok", "C Blok" });

            migrationBuilder.UpdateData(
                table: "Buildings",
                keyColumn: "BuildingId",
                keyValue: 4,
                columns: new[] { "Address", "BuildingName" },
                values: new object[] { "OSTİM Kampüs, D Blok", "D Blok" });

            migrationBuilder.UpdateData(
                table: "MaintenanceRequests",
                keyColumn: "RequestId",
                keyValue: 1,
                column: "IssueTitle",
                value: "Musluk sızıntısı var");

            migrationBuilder.UpdateData(
                table: "MaintenanceRequests",
                keyColumn: "RequestId",
                keyValue: 2,
                column: "IssueTitle",
                value: "Isıtıcı çalışmıyor");

            migrationBuilder.UpdateData(
                table: "MaintenanceRequests",
                keyColumn: "RequestId",
                keyValue: 3,
                column: "IssueTitle",
                value: "Pencere camı çatlak");

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 1,
                column: "Message",
                value: "Aylık yurt doluluk raporu hazırlandı.");

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 2,
                column: "Message",
                value: "2 adet açık bakım talebi bulunmakta.");

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 1,
                column: "Semester",
                value: "2025-2026 Güz");

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 2,
                column: "Semester",
                value: "2025-2026 Güz");

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 3,
                column: "Semester",
                value: "2025-2026 Güz");

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 4,
                column: "Semester",
                value: "2025-2026 Güz");

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 5,
                column: "Semester",
                value: "2025-2026 Bahar");

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 6,
                column: "Semester",
                value: "2025-2026 Bahar");

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 7,
                column: "Semester",
                value: "2025-2026 Bahar");

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 8,
                column: "Semester",
                value: "2025-2026 Bahar");

            migrationBuilder.UpdateData(
                table: "Penalties",
                keyColumn: "PenaltyId",
                keyValue: 1,
                column: "Reason",
                value: "Geç ödeme gecikme cezası");

            migrationBuilder.UpdateData(
                table: "Penalties",
                keyColumn: "PenaltyId",
                keyValue: 2,
                column: "Reason",
                value: "Yurt kuralları ihlali (gürültü)");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                column: "Description",
                value: "Sistem Yöneticisi");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                column: "Description",
                value: "Yurt Müdürü");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                column: "Description",
                value: "Mali İşler Personeli");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 4,
                column: "Description",
                value: "Yurt Personeli");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 5,
                column: "Description",
                value: "Öğrenci Kullanıcı");

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 1,
                column: "FullName",
                value: "Ayşe Yılmaz");

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 4,
                column: "FullName",
                value: "Burak Şahin");

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 6,
                column: "FullName",
                value: "Deniz Öztürk");

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 7,
                column: "FullName",
                value: "Canan Aydın");

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 8,
                column: "FullName",
                value: "Emre Çelik");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "FullName",
                value: "Sistem Yöneticisi");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                column: "FullName",
                value: "Öğrenci Kullanıcı");
        }
    }
}
