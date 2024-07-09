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
    }
}
