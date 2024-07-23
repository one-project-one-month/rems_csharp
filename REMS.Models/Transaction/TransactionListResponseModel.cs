using REMS.Models.Appointment;
using REMS.Models.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Models.Transaction
{
    public class TransactionListResponseModel
    {
        public PageSettingModel? pageSetting { get; set; }

        public List<TransactionModel>? lstTransaction { get; set; }
    }
}
