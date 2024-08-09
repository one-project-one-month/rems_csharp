using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Models.Appointment
{
    public class AppointmentModel
    {
        public int AppointmentId { get; set; }

        public int? ClientId { get; set; }

        public int? PropertyId { get; set; }

        public DateTime AppointmentDate { get; set; }

        public string? AppointmentTime { get; set; }

        public string Status { get; set; } = null!;

        public string? Notes { get; set; }
    }
}
