using REMS.Models.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Models.Appointment
{
    public class AppointmentListResponseModel
    {
        public MessageResponseModel? messageResponse { get; set; }

        public PageSettingModel? pageSetting { get; set; }

        public List<AppointmentModel>? lstAppointment { get; set; }
    }
}
