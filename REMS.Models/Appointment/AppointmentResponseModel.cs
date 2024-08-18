using REMS.Database.AppDbContextModels;
using REMS.Models.Agent;
using REMS.Models.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Models.Appointment
{
    public class AppointmentResponseModel
    {
        public AppointmentModel? Appointment { get; set; }
        public ClientModel? Client { get; set; }
        public AgentDto? Agent { get; set; }

    }
}
