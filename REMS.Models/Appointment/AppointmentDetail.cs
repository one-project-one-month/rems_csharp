
namespace REMS.Models.Appointment;


public partial class AppointmentDetail
{
    public string? AgentName { get; set; }
    public string? ClientName { get; set; }
    public string? AppointmentDate { get; set; }
    public string? AppointmentTime { get; set; }
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

public partial class AppointmentDetailList
{
    public int TotalCount { get; set; }
    public int PageCount { get; set; }
    public int PageNo { get; set; }
    public int PageSize { get; set; }
    public bool IsEndOfPage => PageNo >= PageCount;
    public List<AppointmentDetail> appointmentDetails { get; set; }
}
