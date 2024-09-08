using REMS.Models.Custom;

namespace REMS.Models.Appointment;

public class AppointmentListResponseModel
{
    public PageSettingModel? pageSetting { get; set; }

    public List<AppointmentModel>? lstAppointment { get; set; }
}