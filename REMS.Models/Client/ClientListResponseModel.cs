using REMS.Models.Custom;

namespace REMS.Models.Client;

public class ClientListResponseModel
{
    public List<ClientModel> DataLst { get; set; }
    public PageSettingModel PageSetting { get; set; }
}