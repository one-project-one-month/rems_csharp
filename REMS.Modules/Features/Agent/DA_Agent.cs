using REMS.Database.AppDbContextModels;
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
    }
}
