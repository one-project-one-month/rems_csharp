using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Models.Property
{
    public class PropertyResponseModel
    {
        public PropertyModel Property { get; set; }
        public List<PropertyImageModel> Images { get; set; }
    }
}
