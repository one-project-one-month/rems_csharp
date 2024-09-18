using REMS.Models.Appointment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Models.Dashboard
{
    public class DashboardModel
    {
        public List<OverviewModel>? Overview { get; set; }
        public List<WeeklyActivityModel>? WeeklyActivity { get; set; }
        public List<AgentActivityModel>? AgentActivity { get; set; }
    }

    public class OverviewModel
    {
        public int Agents { get; set; }
        public int Clients { get; set; }
        public int Properties { get; set; }
        public Decimal PropertySoldIncome { get; set; }
        public Decimal PropertyRentedIncome { get; set; }
    }

    public class WeeklyActivityModel
    {
        public string Name { get; set; }
        public Decimal Sold { get; set; }
        public Decimal Rented { get; set; }
    }

    public class AgentActivityModel
    {

        public string AgentName { get; set; }
        public int SellProperty { get; set; }
        public int RentedProperty { get; set; }
        public Decimal TotalSales { get; set; }
        public Decimal CommissionEarned { get; set; }

    }

}
