using Microsoft.Extensions.Configuration;

namespace REMS.Modules.Features.Property;

public class DA_Property
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _configuration;

    public DA_Property(AppDbContext db, IConfiguration configuration)
    {
        _db = db;
        _configuration = configuration;
    }

    public async Task<Result<List<PropertyResponseModel>>> GetProperties()
    {
        Result<List<PropertyResponseModel>> model = null;
        try
        {
            var properties = await _db.Properties
                                            .AsNoTracking()
                                            .Include(x => x.PropertyImages)
                                            .ToListAsync();

            var propertyResponseModels = properties.Select(property => new PropertyResponseModel
            {
                Property = property.Change(),
                Images = property.PropertyImages.Select(x => x.Change()).ToList()
            }).ToList();

            model = Result<List<PropertyResponseModel>>.Success(propertyResponseModels);

            return model;
        }
        catch (Exception ex)
        {
            model = Result<List<PropertyResponseModel>>.Error(ex);
            return model;
        }
    }

    public async Task<Result<PropertyListResponseModel>> GetProperties(int pageNo = 1, int pageSize = 10)
    {
        Result<PropertyListResponseModel> model = null;
        try
        {
            var properties = await _db.Properties
                                      .AsNoTracking()
                                      .Include(x => x.PropertyImages)
                                      .Skip((pageNo - 1) * pageSize)
                                      .Take(pageSize)
                                      .ToListAsync();

            var propertyResponseModel = properties.Select(property => new PropertyResponseModel
            {
                Property = property.Change(),
                Images = property.PropertyImages.Select(x => x.Change()).ToList()
            }).ToList();

            var totalCount = await _db.Properties.CountAsync();
            var pageCount = (int)Math.Ceiling((double)totalCount / pageSize);

            var propertyListResponse = new PropertyListResponseModel
            {
                Properties = propertyResponseModel,
                PageSetting = new PageSettingModel(pageNo, pageSize, pageCount, totalCount)
            };

            model = Result<PropertyListResponseModel>.Success(propertyListResponse);

            return model;
        }
        catch (Exception ex)
        {
            model = Result<PropertyListResponseModel>.Error(ex);
            return model;
        }
    }

    public async Task<Result<PropertyResponseModel>> GetPropertyById(int propertyId)
    {
        Result<PropertyResponseModel> model = null;
        try
        {
            var property = await _db.Properties
                                    .AsNoTracking()
                                    .Include(x => x.PropertyImages)
                                    .FirstOrDefaultAsync(x => x.PropertyId == propertyId)
                                    ?? throw new Exception("Property Not Found");

            var propertyResponse = new PropertyResponseModel
            {
                Property = property.Change(),
                Images = property.PropertyImages.Select(x => x.Change()).ToList()
            };

            model = Result<PropertyResponseModel>.Success(propertyResponse);
            return model;
        }
        catch (Exception ex)
        {
            model = Result<PropertyResponseModel>.Error(ex);
            return model;
        }
    }

    public async Task<PropertyResponseModel> CreateProperty(PropertyRequestModel requestModel)
    {
        try
        {
            if (requestModel == null)
            {
                throw new ArgumentNullException(nameof(requestModel), "Request model cannot be null");
            }

            var property = requestModel.Change();
            if (property == null)
            {
                throw new Exception("Failed to convert request model to property entity");
            }

            await _db.Properties.AddAsync(property);
            await _db.SaveChangesAsync();

            string folderPath = _configuration.GetSection("ImageFolderPath").Value!;
            foreach (var item in requestModel.Images)
            {
                string fileName = Guid.NewGuid().ToString() + ".png";
                string base64Str = item.ImgBase64!;
                byte[] bytes = Convert.FromBase64String(base64Str!);

                string filePath = Path.Combine(folderPath, fileName);
                await File.WriteAllBytesAsync(filePath, bytes);

                // Save File in Folder
                // Save Path in Db

                await _db.PropertyImages.AddAsync(new PropertyImage
                {
                    DateUploaded = DateTime.Now,
                    Description = item.Description,
                    ImageUrl = filePath,
                    PropertyId = property.PropertyId
                });
                await _db.SaveChangesAsync();
            }

             var responseModel = new PropertyResponseModel
            {
                Property = property.Change(),
                Images = new List<PropertyImageModel>()
            };

            return responseModel;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    public async Task<PropertyResponseModel> UpdateProperty(int propertyId, PropertyRequestModel requestModel)
    {
        try
        {
            var property = await _db.Properties
                                    .Include(x => x.PropertyImages)
                                    .FirstOrDefaultAsync(x => x.PropertyId == propertyId)
                                    ?? throw new Exception("Property Not Found");

            property.Address = requestModel.Address;
            property.City = requestModel.City;
            property.State = requestModel.State;
            property.ZipCode = requestModel.ZipCode;
            property.PropertyType = requestModel.PropertyType;
            property.Price = requestModel.Price;
            property.Size = requestModel.Size;
            property.NumberOfBedrooms = requestModel.NumberOfBedrooms;
            property.NumberOfBathrooms = requestModel.NumberOfBathrooms;
            property.YearBuilt = requestModel.YearBuilt;
            property.Description = requestModel.Description;
            property.Status = requestModel.Status;
            property.DateListed = requestModel.DateListed;

            _db.Properties.Update(property);
            await _db.SaveChangesAsync();

            var responseModel = new PropertyResponseModel
            {
                Property = property.Change(),
                Images = property.PropertyImages.Select(x => x.Change()).ToList()
            };

            return responseModel;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return null;
        }
    }

    public async Task<bool> DeleteProperty(int propertyId)
    {
        try
        {
            var property = await _db.Properties
                                    .Include(x => x.PropertyImages)
                                    .FirstOrDefaultAsync(x => x.PropertyId == propertyId)
                                    ?? throw new Exception("Property Not Found");

            _db.Properties.Remove(property);
            await _db.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return false;
        }
    }
}