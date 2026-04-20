using DormitoryManagementSystem.Domain.Entities;

namespace DormitoryManagementSystem.Web.ViewModels
{
    /// <summary>
    /// Report Section 5 - Fig 5.1: Overview Dashboard
    /// Cards: Active Students, Occupancy Rate, Collected Fees, Open Requests
    /// Charts: Occupancy by Building (bar), Payment Status (donut)
    /// </summary>
    public class DashboardViewModel
    {
        public int ActiveStudents { get; set; }
        public double OccupancyRate { get; set; }
        public decimal CollectedFees { get; set; }
        public int OpenRequests { get; set; }
        public Dictionary<string, double> OccupancyByBuilding { get; set; } = new();
        public Dictionary<string, int> PaymentStatusDistribution { get; set; } = new();
    }

    /// <summary>
    /// Report Section 5 - Fig 5.2: Login Interface
    /// </summary>
    public class LoginViewModel
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? ErrorMessage { get; set; }
    }

    /// <summary>
    /// Report Section 5 - Fig 5.2: Student Report screen.
    /// Filters: Building, Year, Status.
    /// Columns: Student No, Full Name, Program, Year, Room, Building, Status, Check-in
    /// </summary>
    public class StudentReportViewModel
    {
        public IEnumerable<StudentReportRow> Rows { get; set; } = new List<StudentReportRow>();
        public string? SearchKeyword { get; set; }
        public string? FilterBuilding { get; set; }
        public int? FilterYear { get; set; }
        public string? FilterStatus { get; set; }
        public IEnumerable<Building> Buildings { get; set; } = new List<Building>();
    }

    /// <summary>
    /// Row model for the Student Report (Fig 5.2): Student + active HousingRecord joined.
    /// </summary>
    public class StudentReportRow
    {
        public int StudentId { get; set; }
        public string StudentNumber { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Program { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Status { get; set; } = string.Empty;

        public string? RoomNumber { get; set; }
        public string? BuildingName { get; set; }
        public DateTime? CheckInDate { get; set; }
    }

    /// <summary>
    /// Report Section 5 - Fig 5.2: Payment & Maintenance Report screen.
    /// </summary>
    public class PaymentReportViewModel
    {
        public decimal TotalAmount { get; set; }
        public decimal CollectedAmount { get; set; }
        public decimal PendingAmount { get; set; }
        public decimal PenaltyAmount { get; set; }
        public IEnumerable<MaintenanceRequest> OpenRequests { get; set; } = new List<MaintenanceRequest>();
    }

    /// <summary>
    /// Room assignment form.
    /// </summary>
    public class AssignRoomViewModel
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public int SelectedRoomId { get; set; }
        public IEnumerable<Room> AvailableRooms { get; set; } = new List<Room>();
    }

    /// <summary>
    /// Student create/edit form.
    /// </summary>
    public class StudentFormViewModel
    {
        public Student Student { get; set; } = new();
        public bool IsEdit { get; set; }
    }

    /// <summary>
    /// User management form.
    /// </summary>
    public class UserFormViewModel
    {
        public User User { get; set; } = new();
        public IEnumerable<Role> Roles { get; set; } = new List<Role>();
        public bool IsEdit { get; set; }
    }

    /// <summary>
    /// Housing Report (Module 5 + Module 8 intersection).
    /// Filterable list of housing records with summary counters.
    /// </summary>
    public class HousingReportViewModel
    {
        public IEnumerable<HousingReportRow> Rows { get; set; } = new List<HousingReportRow>();
        public IEnumerable<Building> Buildings { get; set; } = new List<Building>();

        public string? FilterBuilding { get; set; }
        public string? FilterStatus { get; set; }
        public int? FilterYear { get; set; }

        public int TotalActive { get; set; }
        public int TotalCheckedOut { get; set; }
        public int TotalTransferred { get; set; }
    }

    /// <summary>
    /// One row of the Housing Report table.
    /// </summary>
    public class HousingReportRow
    {
        public int HousingId { get; set; }
        public string StudentNumber { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public string Program { get; set; } = string.Empty;
        public int StudentYear { get; set; }
        public string BuildingName { get; set; } = string.Empty;
        public string RoomNumber { get; set; } = string.Empty;
        public string? RoomType { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public string HousingStatus { get; set; } = string.Empty;
    }

    /// <summary>
    /// Penalty create/edit form.
    /// </summary>
    public class PenaltyFormViewModel
    {
        public Penalty Penalty { get; set; } = new();
        public IEnumerable<Student> Students { get; set; } = new List<Student>();
    }
}
