using REMS.Models.Custom;
using REMS.Models.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Models.Client;

public class ClientListResponseModel
{
    public List<ClientModel> DataLst { get; set; }
    public PageSettingModel PageSetting { get; set; }
}
