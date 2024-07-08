using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Modules.Features.Agent
{
    public class BL_Agent
    {
        private readonly DA_Agent _daAgent;

        public BL_Agent(DA_Agent daAgent)
        {
            _daAgent = daAgent;
        }
    }
}
