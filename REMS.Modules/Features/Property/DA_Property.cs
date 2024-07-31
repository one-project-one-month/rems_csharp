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

    //public async Task<Result<List<PropertyResponseModel>>> GetPropertiesByAgentId(int agentId)
    //{

    //}

    public async Task<Result<PropertyResponseModel>> CreateProperty(PropertyRequestModel requestModel)
    {
        Result<PropertyResponseModel> model = null;
        try
        {
            if (requestModel == null)
            {
                throw new ArgumentNullException(nameof(requestModel), "Request model cannot be null");
            }

            var property = requestModel.Change()
                           ?? throw new Exception("Failed to convert request model to property entity");

            await _db.Properties.AddAsync(property);
            await _db.SaveChangesAsync();

            foreach (var propertyImage in requestModel.Images)
            {
                var photoPath = await SavePhotoInFolder(propertyImage.ImgBase64!);
                await SavePhotoPathToDb(property.PropertyId, propertyImage.Description, photoPath);
            }

            var createdProperty = await _db.Properties
                                    .AsNoTracking()
                                    .Include(x => x.PropertyImages)
                                    .FirstOrDefaultAsync(x => x.PropertyId == property.PropertyId)
                                    ?? throw new Exception("Property Not Found");

            var propertyResponse = new PropertyResponseModel
            {
                Property = createdProperty.Change(),
                Images = createdProperty.PropertyImages.Select(x => x.Change()).ToList()
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

    public async Task<Result<PropertyResponseModel>> UpdateProperty(int propertyId, PropertyRequestModel requestModel)
    {
        Result<PropertyResponseModel> model = null;
        try
        {
            var property = await _db.Properties
                                    .Include(x => x.PropertyImages)
                                    .FirstOrDefaultAsync(x => x.PropertyId == propertyId)
                                    ?? throw new Exception("Property Not Found");

            foreach (var propertyImage in property.PropertyImages)
            {
                RemovePhotoFromFolder(propertyImage.ImageUrl);
            }

            _db.PropertyImages.RemoveRange(property.PropertyImages);

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
            //property.D = requestModel.DateListed;

            foreach (var propertyImage in requestModel.Images)
            {
                var photoPath = await SavePhotoInFolder(propertyImage.ImgBase64!);
                await SavePhotoPathToDb(property.PropertyId, propertyImage.Description, photoPath);
            }

            _db.Properties.Update(property);
            await _db.SaveChangesAsync();

            var updatedProperty = await _db.Properties
                                    .AsNoTracking()
                                    .Include(x => x.PropertyImages)
                                    .FirstOrDefaultAsync(x => x.PropertyId == property.PropertyId)
                                    ?? throw new Exception("Property Not Found");

            var responseModel = new PropertyResponseModel
            {
                Property = updatedProperty.Change(),
                Images = updatedProperty.PropertyImages.Select(x => x.Change()).ToList()
            };
            model = Result<PropertyResponseModel>.Success(responseModel);
            return model;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            model = Result<PropertyResponseModel>.Error(ex);
            return model;
        }
    }

    public async Task<Result<object>> DeleteProperty(int propertyId)
    {
        Result<object> model = null;
        try
        {
            var property = await _db.Properties
                                    .Include(x => x.PropertyImages)
                                    .FirstOrDefaultAsync(x => x.PropertyId == propertyId)
                                    ?? throw new Exception("Property Not Found");

            foreach (var propertyImage in property.PropertyImages)
            {
                RemovePhotoFromFolder(propertyImage.ImageUrl);
            }

            _db.Properties.Remove(property);
            _db.PropertyImages.RemoveRange(property.PropertyImages);

            await _db.SaveChangesAsync();

            model = Result<object>.Success(null);
            return model;
        }
        catch (Exception ex)
        {
            model = Result<object>.Error(ex);
            return model;
        }
    }

    //private async Task<Property> GetPropertyById(int propertyId)
    //{
    //    var property = await _db.Properties
    //                            .AsNoTracking()
    //                            .Include(x => x.PropertyImages)
    //                            .FirstOrDefaultAsync(x => x.PropertyId == propertyId)
    //                            ?? throw new Exception("Property Not Found");
    //}

    private async Task<string> SavePhotoInFolder(string base64Str)
    {
        string folderPath = _configuration.GetSection("ImageFolderPath").Value!;
        string fileName = Guid.NewGuid().ToString() + ".png";
        byte[] bytes = Convert.FromBase64String(base64Str);

        string filePath = Path.Combine(folderPath, fileName);
        await File.WriteAllBytesAsync(filePath, bytes);

        return filePath;
    }

    private async Task SavePhotoPathToDb(int propertyId, string photoDescription, string photoPath)
    {
        await _db.PropertyImages.AddAsync(new PropertyImage
        {
            DateUploaded = DateTime.Now,
            Description = photoDescription,
            ImageUrl = photoPath,
            PropertyId = propertyId
        });
        await _db.SaveChangesAsync();
    }

    private static void RemovePhotoFromFolder(string photoPath)
    {
        if (File.Exists(photoPath))
        {
            File.Delete(photoPath);
        }
    }
}