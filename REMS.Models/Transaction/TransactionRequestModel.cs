using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Models.Transaction
{
    public class TransactionRequestModel
    {
        public int TransactionId { get; set; }
        public int PropertyId { get; set; }
        public int ClientId { get; set; }
        public DateTime TransactionDate { get; set; }
        public Decimal SalePrice { get; set; }
        public Decimal? Commission { get; set; }
        public string Status { get; set; }
    }
}
