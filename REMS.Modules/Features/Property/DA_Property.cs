using Microsoft.EntityFrameworkCore;
using REMS.Database.AppDbContextModels;
using REMS.Mapper;
using REMS.Models.Property;
using REMS.Models.Custom;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace REMS.Modules.Features.Property
{
    public class DA_Property
    {
        private readonly AppDbContext _db;

        public DA_Property(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<PropertyResponseModel>> GetProperties()
        {
            try
            {
                var properties = await _db.Properties.AsNoTracking().ToListAsync();

                var propertyResponseModels = new List<PropertyResponseModel>();

                foreach (var property in properties)
                {
                    var propertyImages = await GetPropertyImagesById(property.PropertyId);
                    var responseModel = new PropertyResponseModel
                    {
                        Property = property.Change(),
                        Images = propertyImages.Select(x => x.Change()).ToList()
                    };

                    propertyResponseModels.Add(responseModel);
                }

                return propertyResponseModels;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<PropertyListResponseModel> GetProperties(int pageNo = 1, int pageSize = 10)
        {
            try
            {
                var properties = await _db.Properties
                                          .AsNoTracking()
                                          .Skip((pageNo - 1) * pageSize)
                                          .Take(pageSize)
                                          .ToListAsync();

                var propertyResponseModels = new List<PropertyResponseModel>();

                foreach (var property in properties)
                {
                    var propertyImages = await GetPropertyImagesById(property.PropertyId);
                    var responseModel = new PropertyResponseModel
                    {
                        Property = property.Change(),
                        Images = propertyImages.Select(x => x.Change()).ToList()
                    };

                    propertyResponseModels.Add(responseModel);
                }

                var totalCount = await _db.Properties.CountAsync();
                var pageCount = (int)Math.Ceiling((double)totalCount / pageSize);

                var response = new PropertyListResponseModel
                {
                    Properties = propertyResponseModels,
                    PageSetting = new PageSettingModel(pageNo, pageSize, pageCount, totalCount)
                };

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }


        public async Task<PropertyResponseModel> GetPropertyById(int propertyId)
        {
            try
            {
                var property = await _db.Properties
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(x => x.PropertyId == propertyId)
                                        ?? throw new Exception("Property Not Found");

                var propertyImages = await GetPropertyImagesById(property.PropertyId);

                var responseModel = new PropertyResponseModel
                {
                    Property = property.Change(),
                    Images = propertyImages.Select(x => x.Change()).ToList()
                };

                return responseModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<List<PropertyImage>> GetPropertyImagesById(int propertyId)
        {
            var propertyImages = await _db.PropertyImages.AsNoTracking().Where(x => x.PropertyId == propertyId).ToListAsync();
            return propertyImages;
        }
    }
}
