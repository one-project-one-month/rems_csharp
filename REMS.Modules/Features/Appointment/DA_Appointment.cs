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
                await _db.Appointments.AddAsync(requestModel.Change());
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
    }
}
