namespace REMS.Models.Agent;

public class SearchAgentRequestModel
{
    public string? AgentName { get; set; }
    public string? Address { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}