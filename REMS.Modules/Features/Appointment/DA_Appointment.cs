using REMS.Models.Appointment;
using REMS.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Modules.Features.Appointment
{
    public class DA_Appointment
    {
        private readonly AppDbContext _db;

        public DA_Appointment(AppDbContext db) => _db = db;

        public async Task<MessageResponseModel> CreateAppointmentAsync(AppointmentRequestModel requestModel)
        {
            try
            {
                var agent = await _db.Agents
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync(x => x.AgentId == requestModel.AgentId);
                if (agent is null)
                {
                    return new MessageResponseModel(false, "Agent Not Found");
                }
                var client = await _db.Clients
                                      .AsNoTracking()
                                      .FirstOrDefaultAsync(x => x.ClientId == requestModel.ClientId);
                if (client is null)
                {
                    return new MessageResponseModel(false, "Client Not Found");
                }
                var property = await _db.Properties
                                      .AsNoTracking()
                                      .FirstOrDefaultAsync(x => x.PropertyId == requestModel.PropertyId);
                if (property is null)
                {
                    return new MessageResponseModel(false, "Property Not Found");
                }
                await _db.Appointments.AddAsync(requestModel.ChangeAppointment());
                int result = await _db.SaveChangesAsync();
                var response = result > 0
                    ? new MessageResponseModel(true, "Appoinment Create Successfully.")
                    : new MessageResponseModel(false, "Invalid.");
                return response;
            }
            catch (Exception ex)
            {
                return new MessageResponseModel(false, ex);
            }
        }

        public async Task<MessageResponseModel> DeleteAppointmentAsync(int id)
        {
            try
            {
                var appointment = await _db.Appointments
                                         .AsNoTracking()
                                         .FirstOrDefaultAsync(x => x.AppointmentId == id);
                if (appointment is null)
                    return new MessageResponseModel(false, "Appointment Not Found.");
                _db.Appointments.Remove(appointment);
                _db.Entry(appointment).State = EntityState.Deleted;
                int result = await _db.SaveChangesAsync();
                var response = result > 0
                    ? new MessageResponseModel(true, "Successfully Delete")
                    : new MessageResponseModel(false, "Deleting Fail");
                return response;
            }
            catch (Exception ex)
            {
                return new MessageResponseModel(false, ex);
            }
        }

        public async Task<AppointmentListResponseModel> GetAppointmentByAgentIdAsync(int id, int pageNo, int pageSize)
        {
            AppointmentListResponseModel response = new AppointmentListResponseModel();
            try
            {
                var query = _db.Appointments
                                .AsNoTracking()
                                .Where(x => x.AgentId == id)
                                .Select(n=>new AppointmentModel
                                {
                                    AppointmentId = n.AppointmentId,
                                    AgentId=n.AgentId,
                                    ClientId=n.ClientId,
                                    PropertyId=n.PropertyId,
                                    AppointmentDate = n.AppointmentDate,
                                    AppointmentTime=n.AppointmentTime.ToString(),
                                    Status=n.Status,
                                    Notes=n.Notes
                                });
                var appointmentList = await query.Pagination(pageNo, pageSize).ToListAsync();
                if(appointmentList is null || appointmentList.Count == 0)
                {
                    response.messageResponse = new MessageResponseModel(false, "No Data Found.");
                    return response;
                }
                int totalCount = await query.CountAsync();
                int pageCount = totalCount / pageSize;
                if (totalCount % pageSize != 0)
                {
                    pageCount++;
                }
                response.messageResponse = new MessageResponseModel(true, "Success");
                response.pageSetting = new PageSettingModel(pageNo, pageSize, pageCount, totalCount);
                response.lstAppointment = appointmentList;
            }
            catch (Exception ex)
            {
                response.messageResponse=new MessageResponseModel(false, ex);
            }
            return response;
        }
    }
}
