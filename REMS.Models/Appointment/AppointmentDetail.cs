using REMS.Models.Custom;

namespace REMS.Models.Appointment;

public class AppointmentDetail
{
    public int? AppointmentId { get; set; }
    public string? AgentName { get; set; }
    public string? ClientName { get; set; }
    public string? AppointmentDate { get; set; }
    public string? AppointmentTime { get; set; }
    public string? AgentPhoneNumber { get; set; }
    public string? Status { get; set; }
    public string? Note { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public decimal? Price { get; set; }
    public decimal? Size { get; set; }
    public int? NumberOfBedrooms { get; set; }
    public int? NumberOfBathrooms { get; set; }
}

public class AppointmentDetailList
{
    public PageSettingModel? pageSetting { get; set; }
    public List<AppointmentDetail> appointmentDetails { get; set; }
}