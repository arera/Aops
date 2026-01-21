using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Domain.Enums
{
    public enum UserRole
    {
        Admin = 1,
        Manager = 2,
        customer = 3,
        Guest = 4
    }
    public enum VehicleDocumentType
    {
        Registration,
        Insurance,
        PollutionCertificate,
        FitnessCertificate,
        Permit
    }

    public enum TicketStatus
    {
        New = 0,            // Ticket created, not yet assigned
        Assigned = 1,       // Assigned to a team/member
        InProgress = 2,     // Work is ongoing
        OnHold = 3,         // Waiting on customer/vendor/another team
        Escalated = 4,      // Escalated to higher priority/next level support
        AwaitingApproval = 5, // Pending approval from manager/customer
        Resolved = 6,       // Fixed, waiting for confirmation
        Reopened = 7,       // Reported as not resolved, reopened for work
        Closed = 8,         // Final state, cannot be modified
        Cancelled = 9,      // Withdrawn or invalid
        Duplicate = 10      // Marked as duplicate of another ticket
    }

    public enum TicketCategorys
    {
        Vehicle =1,
        Manpower = 2,
    }
    public enum TicketPriority
    {
        Low = 1,
        Medium = 2,
        High = 3,
        Critical = 4
    }
    public enum VehicleTicketIssueType
    {
        Breakdown = 1,
        EngineIssue = 2,
        BatteryProblem = 3,
        TyreOrWheelIssue = 4,
        FuelLeakage = 5,
        ElectricalFailure = 6,
        RoutineMaintenance = 7,
        AccidentDamage = 8,
        Other = 9
    }

    public enum ManpowerTicketIssueType
    {
        AttendanceIssue = 1,
        SkillMismatch = 2,
        SafetyConcern = 3,
        Misconduct = 4,
        TrainingRequired = 5,
        PerformanceIssue = 6,
        HealthOrMedical = 7,
        PayrollOrCompensation = 8,
        Other = 9
    }


    public enum ExpenseType
    {
        Medicine = 1,
        Food = 2,
        Fuel = 3,
        Accommodation = 4,
        Maintenance = 5,
        Miscellaneous = 6
    }

    public enum Designation
    {
        Pilot =1,
        Nurse =2,
        Doctor =3,
        Others =4,
    }
    public enum EmploymentType
    {
        Permanent = 1,
        Contract = 2,
        Others = 3,
    }
}
