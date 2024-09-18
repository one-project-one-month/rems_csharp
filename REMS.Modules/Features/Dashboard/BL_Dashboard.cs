using REMS.Models.Dashboard;

namespace REMS.Modules.Features.Dashboard
{
    public class BL_Dashboard
    {
        private readonly DA_Dashboard _daDashboard;

        public BL_Dashboard(DA_Dashboard daDashboard)
        {
            _daDashboard = daDashboard;
        }

        public async Task<Result<DashboardModel>> GetDashboardAsync()
        {
            return await _daDashboard.GetDashboardAsync();
        }
    }
}
