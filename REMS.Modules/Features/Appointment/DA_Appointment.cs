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

        public async Task<Result<string>> CreateAppointmentAsync(AppointmentRequestModel requestModel)
        {
            Result<string> response = null;
            try
            {
                var agent = await _db.Agents
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync(x => x.AgentId == requestModel.AgentId);
                if (agent is null)
                {
                    return Result<string>.Error("Agent Not Found");
                }
                var client = await _db.Clients
                                      .AsNoTracking()
                                      .FirstOrDefaultAsync(x => x.ClientId == requestModel.ClientId);
                if (client is null)
                {
                    return Result<string>.Error("Client Not Found");
                }
                var property = await _db.Properties
                                      .AsNoTracking()
                                      .FirstOrDefaultAsync(x => x.PropertyId == requestModel.PropertyId);
                if (property is null)
                {
                    return Result<string>.Error("Property not found.");
                }
                await _db.Appointments.AddAsync(requestModel.ChangeAppointment());
                int result = await _db.SaveChangesAsync();
                response = result > 0
                    ? Result<string>.Success("Successfully Save")
                    : Result<string>.Error("Saving Fail");
            }
            catch (Exception ex)
            {
                response = Result<string>.Error(ex);
            }
            return response;
        }

        public async Task<Result<string>> DeleteAppointmentAsync(int id)
        {
            Result<string> response = null;
            try
            {
                var appointment = await _db.Appointments
                                         .AsNoTracking()
                                         .FirstOrDefaultAsync(x => x.AppointmentId == id);
                if (appointment is null)
                   return Result<string>.Error("Appointment Not Found.");
                _db.Appointments.Remove(appointment);
                _db.Entry(appointment).State = EntityState.Deleted;
                int result = await _db.SaveChangesAsync();
                response = result > 0
                    ? Result<string>.Success("Successfully Delete")
                    : Result<string>.Error("Deleting Fail");
            }
            catch (Exception ex)
            {
               response= Result<string>.Error(ex);
            }
            return response;
        }

        public async Task<Result<AppointmentListResponseModel>> GetAppointmentByAgentIdAsync(int id, int pageNo, int pageSize)
        {
            Result<AppointmentListResponseModel> response = null;
            AppointmentListResponseModel appointment = new AppointmentListResponseModel();
            try
            {
                var query = _db.Appointments
                                .AsNoTracking()
                                .Where(x => x.AgentId == id)
                                .Select(n => new AppointmentModel
                                {
                                    AppointmentId = n.AppointmentId,
                                    AgentId = n.AgentId,
                                    ClientId = n.ClientId,
                                    PropertyId = n.PropertyId,
                                    AppointmentDate = n.AppointmentDate,
                                    AppointmentTime = n.AppointmentTime.ToString(),
                                    Status = n.Status,
                                    Notes = n.Notes
                                });
                var appointmentList = await query.Pagination(pageNo, pageSize).ToListAsync();
                if (appointmentList is null || appointmentList.Count == 0)
                {
                    return Result<AppointmentListResponseModel>.Error("No Data Found");
                }
                int totalCount = await query.CountAsync();
                int pageCount = totalCount / pageSize;
                if (totalCount % pageSize != 0)
                {
                    pageCount++;
                }
                appointment.pageSetting = new PageSettingModel(pageNo, pageSize, pageCount, totalCount);
                appointment.lstAppointment = appointmentList;
                response=Result<AppointmentListResponseModel>.Success(appointment);
            }
            catch (Exception ex)
            {
                response = Result<AppointmentListResponseModel>.Error(ex);
            }
            return response;
        }
    }
}
