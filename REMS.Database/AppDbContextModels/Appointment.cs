namespace REMS.Database.AppDbContextModels;

public class Appointment
{
    public int AppointmentId { get; set; }

    public int? ClientId { get; set; }

    public int? PropertyId { get; set; }

    public DateTime AppointmentDate { get; set; }

    public TimeSpan AppointmentTime { get; set; }

    public string Status { get; set; } = null!;

    public string? Notes { get; set; }

    public virtual Client? Client { get; set; }

    public virtual Property? Property { get; set; }
}