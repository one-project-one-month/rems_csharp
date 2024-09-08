namespace REMS.Modules.Features.Appointment;

public class DA_Appointment
{
    private readonly AppDbContext _db;

    public DA_Appointment(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Result<AppointmentResponseModel>> CreateAppointmentAsync(AppointmentRequestModel requestModel)
    {
        Result<AppointmentResponseModel> response;
        try
        {
            var client = await _db.Clients
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ClientId == requestModel.ClientId);
            if (client is null) return Result<AppointmentResponseModel>.Error("Client Not Found");
            var property = await _db.Properties
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.PropertyId == requestModel.PropertyId);
            if (property is null) return Result<AppointmentResponseModel>.Error("Property Not Found");
            var appointment = requestModel.ChangeAppointment();
            await _db.Appointments.AddAsync(appointment);
            var result = await _db.SaveChangesAsync();
            if (result < 0) return Result<AppointmentResponseModel>.Error("Invalid");
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
            var result = await _db.SaveChangesAsync();
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

    public async Task<Result<AppointmentDetailList>> GetAppointmentByPropertyIdAsycn(int propertyId, int pageNo,
        int pageSize)
    {
        Result<AppointmentDetailList> response = null;
        try
        {
            var query = await (from _app in _db.Appointments
                               join _cli in _db.Clients on _app.ClientId equals _cli.ClientId
                               join _pro in _db.Properties on _app.PropertyId equals _pro.PropertyId
                               join _age in _db.Agents on _pro.AgentId equals _age.AgentId
                               where _app.PropertyId == propertyId
                               select new AppointmentDetail
                               {
                                   AgentName = _age.AgencyName,
                                   ClientName = _cli.FirstName + " " + _cli.LastName,
                                   AppointmentDate = _app.AppointmentDate.ToString("yyyy-MM-dd"),
                                   AppointmentTime = _app.AppointmentTime.ToString(),
                                   Status = _app.Status,
                                   Note = _app.Notes,
                                   Address = _pro.Address,
                                   City = _pro.City,
                                   State = _pro.State,
                                   Price = _pro.Price,
                                   Size = _pro.Size,
                                   NumberOfBedrooms = _pro.NumberOfBedrooms,
                                   NumberOfBathrooms = _pro.NumberOfBathrooms
                               }).ToListAsync();
            var appointmentList = query
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize).ToList();
            if (appointmentList is null || appointmentList.Count < 0)
                return Result<AppointmentDetailList>.Error("No Data Found.");
            var totalCount = query.Count();
            var pageCount = totalCount / pageSize;
            if (totalCount % pageSize != 0) pageCount++;
            var appointmentResponse = new AppointmentDetailList
            {
                pageSetting = new PageSettingModel(pageNo, pageSize, pageCount, totalCount),
                appointmentDetails = appointmentList
            };
            response = Result<AppointmentDetailList>.Success(appointmentResponse);
        }
        catch (Exception ex)
        {
            response = Result<AppointmentDetailList>.Error(ex);
        }

        return response;
    }

    public async Task<Result<AppointmentResponseModel>> UpdateAppointmentAsync(int id,
        AppointmentRequestModel requestModel)
    {
        Result<AppointmentResponseModel> response = null;
        try
        {
            if (requestModel is null)
                return Result<AppointmentResponseModel>.Error("Request Model is null");

            var appointment = await _db.Appointments
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.AppointmentId == id);
            if (appointment is null)
                return Result<AppointmentResponseModel>.Error("appointment not found");

            if (requestModel.AppointmentDate != DateTime.MinValue)
                appointment.AppointmentDate = requestModel.AppointmentDate;
            if (!string.IsNullOrWhiteSpace(requestModel.AppointmentTime))
                appointment.AppointmentTime = TimeSpan.Parse(requestModel.AppointmentTime);
            if (!string.IsNullOrWhiteSpace(requestModel.Status)) appointment.Status = requestModel.Status;
            if (!string.IsNullOrWhiteSpace(requestModel.Notes)) appointment.Notes = requestModel.Notes;
            _db.Entry(appointment).State = EntityState.Modified;
            var result = await _db.SaveChangesAsync();
            if (result < 0) return Result<AppointmentResponseModel>.Error("Fail to update appointment");
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

    public async Task<Result<AppointmentDetailList>> GetAppointmentByClientId(int clientId, int pageNo, int pageSize)
    {
        Result<AppointmentDetailList> model = null;
        try
        {
            var query = await (from _app in _db.Appointments
                               join _cli in _db.Clients on _app.ClientId equals _cli.ClientId
                               join _pro in _db.Properties on _app.PropertyId equals _pro.PropertyId
                               join _age in _db.Agents on _pro.AgentId equals _age.AgentId
                               join _user in _db.Users on _age.UserId equals _user.UserId
                               where _app.ClientId == clientId
                               select new AppointmentDetail
                               {
                                   AppointmentId = _app.AppointmentId,
                                   AgentName = _age.AgencyName,
                                   ClientName = _cli.FirstName + " " + _cli.LastName,
                                   AppointmentDate = _app.AppointmentDate.ToString("yyyy-MM-dd"),
                                   AppointmentTime = _app.AppointmentTime.ToString(),
                                   AgentPhoneNumber = _user.Phone,
                                   Status = _app.Status,
                                   Note = _app.Notes,
                                   Address = _pro.Address,
                                   City = _pro.City,
                                   State = _pro.State,
                                   Price = _pro.Price,
                                   Size = _pro.Size,
                                   NumberOfBedrooms = _pro.NumberOfBedrooms,
                                   NumberOfBathrooms = _pro.NumberOfBathrooms
                               }).ToListAsync();
            var appointmentList = query
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize).ToList();
            if (appointmentList is null || appointmentList.Count == 0)
                return Result<AppointmentDetailList>.Error("No Data Found.");
            var totalCount = query.Count();
            var pageCount = totalCount / pageSize;
            if (totalCount % pageSize != 0) pageCount++;
            var newappdetailIst = new AppointmentDetailList
            {
                pageSetting = new PageSettingModel(pageNo, pageSize, pageCount, totalCount),
                appointmentDetails = appointmentList
            };
            model = Result<AppointmentDetailList>.Success(newappdetailIst);
        }
        catch (Exception ex)
        {
            model = Result<AppointmentDetailList>.Error(ex);
        }

        return model;
    }
}