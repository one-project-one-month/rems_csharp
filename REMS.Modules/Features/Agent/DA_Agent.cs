using Microsoft.EntityFrameworkCore;
using REMS.Database.AppDbContextModels;
using REMS.Mapper;
using REMS.Models;
using REMS.Models.Agent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Modules.Features.Agent
{
    public class DA_Agent
    {
        private readonly AppDbContext _db;

        public DA_Agent(AppDbContext db)
        {
            _db = db;
        }

        public async Task<MessageResponseModel> CreateAgentAsync(AgentRequestModel requestModel)
        {
            try
            {
                await _db.Users.AddAsync(requestModel.ChangeUser());
                int result = await _db.SaveChangesAsync();
                if (result < 0)
                {
                    return new MessageResponseModel(false, "Registration Fail");
                }
                var user = await _db.Users
                                    .OrderByDescending(x => x.UserId)
                                    .AsNoTracking()
                                    .FirstAsync();
                requestModel.UserId = user.UserId;
                await _db.Agents.AddAsync(requestModel.Change());
                int addAgent = await _db.SaveChangesAsync();
                var response = addAgent > 0 ? new MessageResponseModel(true, "Successfully Save") :
                                               new MessageResponseModel(false, "Agent Register Fail");
                return response;
            }
            catch (Exception ex)
            {
                return new MessageResponseModel(false, ex);
            }
        }

        public async Task<MessageResponseModel> UpdateAgentAsync(int id, AgentRequestModel requestModel)
        {
            try
            {
                var user = await _db.Users
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(x => x.UserId == id);
                var agent = await _db.Agents
                                   .AsNoTracking()
                                   .FirstOrDefaultAsync(x => x.UserId == id);
                if (user is null || agent is null)
                {
                    return new MessageResponseModel(false, "User Not Found");
                }
                if (!string.IsNullOrWhiteSpace(requestModel.AgentName))
                {
                    user.Name = requestModel.AgentName;
                    agent.AgencyName = requestModel.AgentName;
                }
                if (!string.IsNullOrWhiteSpace(requestModel.LicenseNumber))
                {
                    agent.LicenseNumber = requestModel.LicenseNumber;
                }
                if (!string.IsNullOrWhiteSpace(requestModel.Email))
                {
                    user.Email = requestModel.Email;
                    agent.Email = requestModel.Email;
                }
                if (!string.IsNullOrWhiteSpace(requestModel.Password))
                {
                    user.Password = requestModel.Password;
                }
                if (!string.IsNullOrWhiteSpace(requestModel.Phone))
                {
                    user.Phone = requestModel.Phone;
                    agent.Phone = requestModel.Phone;
                }
                if (!string.IsNullOrWhiteSpace(requestModel.Address))
                {
                    agent.Address = requestModel.Address;
                }
                _db.Entry(user).State = EntityState.Modified;
                _db.Entry(agent).State = EntityState.Modified;
                int result = await _db.SaveChangesAsync();
                var response = result > 0 ? new MessageResponseModel(true, "Successfully Update") :
                                            new MessageResponseModel(false, "Updating Fail");
                return response;
            }
            catch (Exception ex)
            {
                return new MessageResponseModel(false, ex);
            }
        }

        public async Task<MessageResponseModel> DeleteAgentAsync(int userId)
        {
            try
            {
                var user = await _db.Users
                                  .AsNoTracking()
                                  .FirstOrDefaultAsync(x => x.UserId == userId);
                var agent = await _db.Agents
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(x => x.UserId == userId);
                if (user is null || agent is null)
                {
                    return new MessageResponseModel(false, "User Not Found");
                }
                _db.Users.Remove(user);
                _db.Agents.Remove(agent);
                int result = await _db.SaveChangesAsync();
                var response = result > 0 ? new MessageResponseModel(true, "Successfully Delete") :
                                            new MessageResponseModel(false, "Deleting Fail");
                return response;
            }
            catch (Exception ex)
            {
                return new MessageResponseModel(false, ex);
            }
        }
    }
}
