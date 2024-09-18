using Azure;
using REMS.Models.Dashboard;

namespace REMS.Modules.Features.Dashboard
{
    public class DA_Dashboard
    {
        private readonly AppDbContext _db;
        private readonly _DapperService _dapperService;

        public DA_Dashboard(AppDbContext db, _DapperService DS)
        {
            _db = db;
            _dapperService = DS;
        }
        public async Task<Result<DashboardModel>> GetDashboardAsync()
        {
            Result<DashboardModel> response = null;
            DashboardModel responseModel = new DashboardModel();
            try
            {
                var result = await _dapperService.QueryMultipleAsync<
                    OverviewModel,
                    AgentActivityModel,
                    WeeklyActivityModel>("sp_Dashboard");

                if (result.Item3 is null)
                {
                    Result<DashboardModel>.Error("Please check the data.");
                    goto result;
                }

                responseModel.Overview = result.Item1.ToList();
                responseModel.AgentActivity = result.Item2.ToList();
                responseModel.WeeklyActivity = result.Item3.ToList();

                response = Result<DashboardModel>.Success(responseModel, "We can successfully retrieve the data from the sp.");
            }
            catch (Exception ex)
            {
                responseModel.Overview = new List<OverviewModel>();
                responseModel.AgentActivity = new List<AgentActivityModel>();
                responseModel.WeeklyActivity = new List<WeeklyActivityModel>();
                response = Result<DashboardModel>.Error("Need to check the data.");
            }

        result:
            return response;
        }
    }



}
