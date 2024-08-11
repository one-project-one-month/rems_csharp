using REMS.Models.Client;
using REMS.Models.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Models.Transaction
{
    public class TransactionResponseModel
    {
        public TransactionModel Transaction { get; set; }
        public ClientModel Client { get; set; }
        public PropertyModel Property { get; set; }
    }
}
