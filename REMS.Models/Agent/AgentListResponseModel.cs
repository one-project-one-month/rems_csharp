using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Models.Agent;

public class AgentListResponseModel
{
    public bool IsSuccess { get; set; } = false;
    public string Status { get; set; } = string.Empty;
    public List<AgentDto> AgentList { get; set; } = new List<AgentDto>();
}