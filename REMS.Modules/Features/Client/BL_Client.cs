using REMS.Models.Agent;
using REMS.Models;
using REMS.Modules.Features.Agent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using REMS.Models.Client;

namespace REMS.Modules.Features.Client
{
    public class BL_Client
    {
        private readonly DA_Client _daClient;

        public BL_Client(DA_Client daClient)
        {
            _daClient = daClient;
        }

        public async Task<MessageResponseModel> CreateClientAsync(ClientRequestModel requestModel)
        {
            var response = await _daClient.CreateClientAsync(requestModel);
            return response;
        }
    }
}
