using REMS.Models.Appointment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Modules.Features.Appointment
{
    public class BL_Appointment
    {
        private readonly DA_Appointment _daAppointment;

        public BL_Appointment(DA_Appointment daAppointment)
        {
            _daAppointment = daAppointment;
        }

        public async Task<MessageResponseModel> CreateAppointmentAsync(AppointmentRequestModel requestModel)
        {
            var response = CheckAppointmentValue(requestModel);
            if(response is not null)
            {
                return response;
            }
            return await _daAppointment.CreateAppointmentAsync(requestModel);
        }

        private MessageResponseModel CheckAppointmentValue(AppointmentRequestModel requestModel)
        {
            if (requestModel is null)
            {
                return new MessageResponseModel(false, "Model is null");
            }
            if(requestModel.Status is null)
            {
                return new MessageResponseModel(false, "Please Add Status");
            }
            return default;
        }
    }
}
