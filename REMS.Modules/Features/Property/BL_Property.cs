using REMS.Models.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Modules.Features.Property
{
    public class BL_Property
    {
        private readonly DA_Property _daProperty;

        public BL_Property(DA_Property daProperty)
        {
            _daProperty = daProperty;
        }

        public async Task<PropertyResponseModel> GetPropertyById(int propertyId)
        {
            if (propertyId < 1)
            {
                throw new Exception("Invalid Property Id");
            }

            var response = await _daProperty.GetPropertyById(propertyId);
            return response;
        }
    }
}
