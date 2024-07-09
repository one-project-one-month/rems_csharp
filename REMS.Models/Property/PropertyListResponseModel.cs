using REMS.Models.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Models.Property
{
    public class PropertyListResponseModel
    {
        public List<PropertyResponseModel> Properties {  get; set; }
        public PageSettingModel PageSetting {  get; set; }
    }
}
