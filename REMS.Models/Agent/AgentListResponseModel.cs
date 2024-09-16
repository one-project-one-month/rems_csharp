using REMS.Models.Custom;

namespace REMS.Models.Agent;

public class AgentListResponseModel
{
    public List<AgentDto> AgentList { get; set; } = new();
    public PageSettingModel? pageSetting { get; set; }
}