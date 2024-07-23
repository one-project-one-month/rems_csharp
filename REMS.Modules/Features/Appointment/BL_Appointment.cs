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

        public async Task<Result<string>> CreateAppointmentAsync(AppointmentRequestModel requestModel)
        {
            var checkAppointment = CheckAppointmentValue(requestModel);
            if (checkAppointment is not null)
            {
                return checkAppointment;
            }
            return await _daAppointment.CreateAppointmentAsync(requestModel);
        }

        public async Task<Result<string>> DeleteAppointmentAsync(int id)
        {
            var response = await _daAppointment.DeleteAppointmentAsync(id);
            return response;
        }

        public async Task<Result<AppointmentListResponseModel>> GetAppointmentByAgentIdAsync(int id, int pageNo, int pageSize)
        {
            var checkpageSetting=CheckPageNoandPageSize(pageNo, pageSize);
            if(checkpageSetting is not null)
            {
                return checkpageSetting;
            }
            return await _daAppointment.GetAppointmentByAgentIdAsync(id, pageNo, pageSize);
        }

        private Result<string> CheckAppointmentValue(AppointmentRequestModel requestModel)
        {
            TimeSpan time;
            if (requestModel is null)
            {
                return Result<string>.Error("Model is null");
            }
            if (requestModel.Status is null)
            {
                Result<string>.Error( "Please Add Status.");
            }
            if (requestModel.AppointmentTime is null)
            {
                Result<string>.Error( "Please Add Appointment Time.");
            }
            if (!TimeSpan.TryParse(requestModel.AppointmentTime, out time))
            {
                Result<string>.Error( "Invalid Appointment Time.");
            }
            return default;
        }

        private Result<AppointmentListResponseModel> CheckPageNoandPageSize(int pageNo, int pageSize)
        {
            AppointmentListResponseModel response = new AppointmentListResponseModel();
            if (pageNo <= 0)
            {
                return Result<AppointmentListResponseModel>.Error("PageNo must be greater than 0");
            }
            if (pageSize <= 0)
            {
                return Result<AppointmentListResponseModel>.Error("PageSize must be greater than 0");
            }
            return default;
        }
    }
}
