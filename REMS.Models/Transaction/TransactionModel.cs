using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Models.Transaction
{
    public class TransactionModel
    {
        public int TransactionId { get; set; }
        public int PropertyId { get; set; }
        public int BuyerId { get; set; }
        public int SellerId { get; set; }
        public int AgentId { get; set; }
        public DateTime TransactionDate { get; set; }
        public Decimal SalePrice { get; set; }
        public Decimal? Commission { get; set; }
        public string Status { get; set; }
    }
}
