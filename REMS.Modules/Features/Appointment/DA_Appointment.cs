
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using REMS.Database.AppDbContextModels;
using REMS.Shared;
using static Azure.Core.HttpHeader;

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

        public async Task<Result<string>> UpdateAppointment(int id, AppointmentRequestModel requestModel)
        {
            try
            {
                var appointres = await _db.Appointments.Where(x => x.AppointmentId == id).FirstOrDefaultAsync();
                if (appointres is null)
                    return Result<string>.Error("Appointment Not Found");

                var agent = await _db.Agents.AsNoTracking()
                    .Where(x => x.AgentId == requestModel.AgentId).FirstOrDefaultAsync();
                if (agent is null)
                    return Result<string>.Error("Agent Not Found");

                var client = await _db.Clients.AsNoTracking()
                    .Where(x => x.ClientId == requestModel.ClientId).FirstOrDefaultAsync();
                if (client is null)
                    return Result<string>.Error("Client Not Found");

                var property = await _db.Properties.AsNoTracking()
                    .Where(x => x.PropertyId == requestModel.PropertyId)
                    .FirstOrDefaultAsync();
                if (property is null)
                    return Result<string>.Error("Property Not Found");
                appointres.AgentId = requestModel.AgentId;
                appointres.ClientId = requestModel.ClientId;
                appointres.PropertyId = requestModel.PropertyId;
                appointres.AppointmentDate = requestModel.AppointmentDate;
                appointres.AppointmentTime = TimeSpan.Parse(requestModel.AppointmentTime!);
                appointres.Status = requestModel.Status;
                appointres.Notes = requestModel.Notes;
                _db.Appointments.Update(appointres);
                await _db.SaveChangesAsync();
                return Result<string>.Success("Update Appointment Success");
            }
            catch (Exception ex)
            {
                return Result<string>.Error(ex);
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

        public async Task<Result<AppointmentDetail>> GetAppointDetails(int id)
        {
            Result<AppointmentDetail> model = null;
            try
            {
                var query = await (from _app in _db.Appointments
                                   join _age in _db.Agents on _app.AgentId equals _age.AgentId
                                   join _cli in _db.Clients on _app.ClientId equals _cli.ClientId
                                   join _pro in _db.Properties on _app.PropertyId equals _pro.PropertyId
                                   where _app.AppointmentId == id
                                   select new AppointmentDetail
                                   {
                                       AgentName = _age.AgencyName,
                                       ClientName = _cli.FirstName + " " + _cli.LastName,
                                       AppointmentDate = _app.AppointmentDate.Year + "-" + _app.AppointmentDate.Month + "-" + _app.AppointmentDate.Day,
                                       AppointmentTime = _app.AppointmentTime.Hours + "-" + _app.AppointmentTime.Minutes,
                                       Status = _app.Status,
                                       Note = _app.Notes,
                                       Address = _pro.Address,
                                       City = _pro.City,
                                       State = _pro.State,
                                       Price = _pro.Price,
                                       Size = _pro.Size,
                                       NumberOfBedrooms = _pro.NumberOfBedrooms,
                                       NumberOfBathrooms = _pro.NumberOfBathrooms,
                                   }).FirstOrDefaultAsync();
                if(query is null)
                    return Result<AppointmentDetail>.Error("Appointment Not Found");
                model = Result<AppointmentDetail>.Success(query);
            }
            catch (Exception ex)
            {
                model = Result<AppointmentDetail>.Error(ex);
            }
            return model;
        }
    }
}
