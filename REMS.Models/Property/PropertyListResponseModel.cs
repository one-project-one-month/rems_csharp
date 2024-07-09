using REMS.Models.Custom;

namespace REMS.Models.Property;

public class PropertyListResponseModel
{
    public List<PropertyResponseModel> Properties {  get; set; }
    public PageSettingModel PageSetting {  get; set; }
}