using REMS.Database.AppDbContextModels;
using REMS.Models.Agent;
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
        #region Agent

        public static User ChangeUser(this AgentRequestModel requestModel)
        {
            User user = new User
            {
                Name = requestModel.AgentName!,
                Email = requestModel.Email!,
                Password = requestModel.Password!,
                Phone = requestModel.Phone,
                Role = "Agent",
                DateCreated = requestModel.DateCreate
            };
            return user;
        }

        public static Agent Change(this AgentRequestModel requestModel)
        {
            Agent agent = new Agent
            {
                UserId = requestModel.UserId,
                AgencyName = requestModel.AgentName!,
                LicenseNumber = requestModel.LicenseNumber!,
                Phone = requestModel.Phone!,
                Email = requestModel.Email!,
                Address = requestModel.Address!
            };
            return agent;
        }

        #endregion

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
