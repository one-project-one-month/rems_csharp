using REMS.Database.AppDbContextModels;
using REMS.Models.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Mapper
{
    public static class ChangeModel
    {
        #region Property

        public static PropertyModel Change(this Property dataModel)
        {
            var propertyModel = new PropertyModel()
            {
                PropertyId = dataModel.PropertyId,
                Address = dataModel.Address,
                City = dataModel.City,
                State = dataModel.State,
                ZipCode = dataModel.ZipCode,
                PropertyType = dataModel.PropertyType,
                Price = dataModel.Price,
                Size = dataModel.Size,
                NumberOfBedrooms = dataModel.NumberOfBedrooms,
                NumberOfBathrooms = dataModel.NumberOfBathrooms,
                YearBuilt = dataModel.YearBuilt,
                Description = dataModel.Description,
                Status = dataModel.Status,
                DateListed = dataModel.DateListed
            };

            return propertyModel;
        }

        #endregion

        #region Property Image

        public static PropertyImageModel Change(this PropertyImage dataModel)
        {
            var propertyModel = new PropertyImageModel()
            {
                ImageId = dataModel.ImageId,
                PropertyId = dataModel.PropertyId,
                ImageUrl = dataModel.ImageUrl,
                Description = dataModel.Description,
                DateUploaded = dataModel.DateUploaded
            };

            return propertyModel;
        }

        #endregion
    }
}
