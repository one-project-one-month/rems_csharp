using Azure;
using REMS.Models.Appointment;
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

        public async Task<Result<AppointmentResponseModel>> CreateAppointmentAsync(AppointmentRequestModel requestModel)
        {
            Result<AppointmentResponseModel> response;
            try
            {
                var client = await _db.Clients
                                      .AsNoTracking()
                                      .FirstOrDefaultAsync(x => x.ClientId == requestModel.ClientId);
                if (client is null)
                {
                    return Result<AppointmentResponseModel>.Error("Client Not Found");
                }
                var property = await _db.Properties
                                      .AsNoTracking()
                                      .FirstOrDefaultAsync(x => x.PropertyId == requestModel.PropertyId);
                if (property is null)
                {
                    return Result<AppointmentResponseModel>.Error("Property Not Found");
                }
                var appointment = requestModel.ChangeAppointment();
                await _db.Appointments.AddAsync(appointment);
                int result = await _db.SaveChangesAsync();
                if (result < 0)
                {
                    return Result<AppointmentResponseModel>.Error("Invalid");
                }
                var appointmentResponse = new AppointmentResponseModel
                {
                    Appointment = appointment.Change()
                };
                response = Result<AppointmentResponseModel>.Success(appointmentResponse);
            }
            catch (Exception ex)
            {
                response = Result<AppointmentResponseModel>.Error(ex);
            }
            return response;
        }

        public async Task<Result<object>> DeleteAppointmentAsync(int id)
        {
            Result<object> response = null;
            try
            {
                var appointment = await _db.Appointments
                                         .AsNoTracking()
                                         .FirstOrDefaultAsync(x => x.AppointmentId == id);
                if (appointment is null)
                    return Result<object>.Error("Appointment Not Found.");
                _db.Appointments.Remove(appointment);
                _db.Entry(appointment).State = EntityState.Deleted;
                int result = await _db.SaveChangesAsync();
                response = result > 0
                    ? Result<object>.Success(null, "Successfully Delete")
                    : Result<object>.Error("Deleting Fail");
            }
            catch (Exception ex)
            {
                response = Result<object>.Error(ex);
            }
            return response;
        }

        public async Task<Result<AppointmentListResponseModel>> GetAppointmentByClientIdAsync(int id, int pageNo, int pageSize)
        {
            Result<AppointmentListResponseModel> response = null;
            try
            {
                var query = _db.Appointments
                                .AsNoTracking()
                                .Where(x => x.ClientId == id)
                                .Select(n => new AppointmentModel
                                {
                                    AppointmentId = n.AppointmentId,
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
                    return Result<AppointmentListResponseModel>.Error("No Data Found.");
                }
                int totalCount = await query.CountAsync();
                int pageCount = totalCount / pageSize;
                if (totalCount % pageSize != 0)
                {
                    pageCount++;
                }
                var appointmentResponse = new AppointmentListResponseModel
                {
                    pageSetting = new PageSettingModel(pageNo, pageSize, pageCount, totalCount),
                    lstAppointment = appointmentList
                };
                response = Result<AppointmentListResponseModel>.Success(appointmentResponse);
            }
            catch (Exception ex)
            {
                response = Result<AppointmentListResponseModel>.Error(ex);
            }
            return response;
        }

        public async Task<Result<AppointmentResponseModel>> UpdateAppointmentAsync(int id, AppointmentRequestModel requestModel)
        {
            Result<AppointmentResponseModel> response = null;
            try
            {
                if (requestModel is null)
                    return Result<AppointmentResponseModel>.Error("Request Model is null");

                var appointment = await _db.Appointments
                                         .AsNoTracking()
                                         .FirstOrDefaultAsync(x => x.PropertyId == id); ;
                if (appointment is null)
                    return Result<AppointmentResponseModel>.Error("appointment not found");

                if (!string.IsNullOrWhiteSpace(requestModel.AppointmentDate.ToString()))
                {
                    appointment.AppointmentDate = requestModel.AppointmentDate;
                }
                if (!string.IsNullOrWhiteSpace(requestModel.AppointmentTime))
                {
                    appointment.AppointmentTime = TimeSpan.Parse(requestModel.AppointmentTime);
                }
                if (!string.IsNullOrWhiteSpace(requestModel.Status))
                {
                    appointment.Status = requestModel.Status;
                }
                if (!string.IsNullOrWhiteSpace(requestModel.Notes))
                {
                    appointment.Notes = requestModel.Notes;
                }
                _db.Entry(appointment).State = EntityState.Modified;
                int result = await _db.SaveChangesAsync();
                if (result < 0)
                {
                    return Result<AppointmentResponseModel>.Error("Fail to update appointment");
                }
                var appointmentResponse = new AppointmentResponseModel
                {
                    Appointment = appointment.Change()
                };
                response = Result<AppointmentResponseModel>.Success(appointmentResponse, "Appointment Update Successfully");
            }
            catch (Exception ex)
            {
                response = Result<AppointmentResponseModel>.Error(ex);
            }
            return response;
        }
    }
}
