using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Models.Agent;

public class AgentResponseModel
{
    public bool IsSuccess { get; set; } = false;
    public string Status { get; set; } = string.Empty;
    public AgentDto? Agent { get; set; }
}