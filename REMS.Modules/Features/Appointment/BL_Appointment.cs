using REMS.Models.Appointment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
            if (response is not null)
            {
                return response;
            }
            return await _daAppointment.CreateAppointmentAsync(requestModel);
        }

        public async Task<MessageResponseModel> DeleteAppointmentAsync(int id)
        {
            var response = await _daAppointment.DeleteAppointmentAsync(id);
            return response;
        }

        public async Task<AppointmentListResponseModel> GetAppointmentByAgentIdAsync(int id, int pageNo, int pageSize)
        {
            var response=CheckPageNoandPageSize(pageNo, pageSize);
            if(response is not null)
            {
                return response;
            }
            return await _daAppointment.GetAppointmentByAgentIdAsync(id, pageNo, pageSize);
        }

        private MessageResponseModel CheckAppointmentValue(AppointmentRequestModel requestModel)
        {
            TimeSpan time;
            if (requestModel is null)
            {
                return new MessageResponseModel(false, "Model is null.");
            }
            if (requestModel.Status is null)
            {
                return new MessageResponseModel(false, "Please Add Status.");
            }
            if (requestModel.AppointmentTime is null)
            {
                return new MessageResponseModel(false, "Please Add Appointment Time.");
            }
            if (!TimeSpan.TryParse(requestModel.AppointmentTime, out time))
            {
                return new MessageResponseModel(false, "Invalid Appointment Time.");
            }
            return default;
        }

        private AppointmentListResponseModel CheckPageNoandPageSize(int pageNo, int pageSize)
        {
            AppointmentListResponseModel response = new AppointmentListResponseModel();
            if (pageNo <= 0)
            {
                response.messageResponse = new MessageResponseModel(false, "PageNo must be positive number");
                return response;
            }
            if (pageSize <= 0)
            {
                response.messageResponse = new MessageResponseModel(false, "pageSize must be positive number");
                return response;
            }
            return default;
        }
    }
}
