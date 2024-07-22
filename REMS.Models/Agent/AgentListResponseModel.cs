using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Models.Agent;

public class AgentListResponseModel
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public int PageCount { get; set; }
    public bool IsEndOfPages => PageNumber >= PageCount;
    public List<AgentDto> AgentList { get; set; } = new List<AgentDto>();
}