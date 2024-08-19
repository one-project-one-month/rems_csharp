using Azure;
using Microsoft.Extensions.Configuration;
using REMS.Models;

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

    public async Task<Result<PropertyListResponseModel>> GetProperties(
        int? agentId, string? address, string? city, string? state, string? zipCode,
        string? propertyType, decimal? minPrice, decimal? maxPrice,
        decimal? size, int? numberOfBedrooms, int? numberOfBathrooms,
        int? yearBuilt, string? availabilityType, int? minRentalPeriod,
        string? approvedBy, DateTime? addDate, DateTime? editDate,
        string? propertyStatus, int pageNo = 1, int pageSize = 10)
    {
        Result<PropertyListResponseModel> model = null;
        try
        {
            var query = _db.Properties.AsNoTracking().AsQueryable();

            query = ApplyFilters(query, agentId, address, city,
                                 state, zipCode, propertyType,
                                 minPrice, maxPrice, size,
                                 numberOfBedrooms, numberOfBathrooms,
                                 yearBuilt, availabilityType,
                                 minRentalPeriod, approvedBy,
                                 addDate, editDate, propertyStatus);

            var totalCount = await query.CountAsync();

            var properties = await query
                .Include(x => x.PropertyImages)
                .Include(x => x.Reviews)
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .Select(property => new PropertyResponseModel
                {
                    Property = property.Change(),
                    Images = property.PropertyImages.Select(x => x.Change()).ToList(),
                    Reviews = property.Reviews.Select(x => x.Change()).ToList()
                })
                .ToListAsync();

            var pageCount = (int)Math.Ceiling((double)totalCount / pageSize);

            var propertyListResponse = new PropertyListResponseModel
            {
                Properties = properties,
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
                                    .Include(x => x.Reviews)
                                    .FirstOrDefaultAsync(x => x.PropertyId == propertyId)
                                    ?? throw new Exception("Property Not Found");

            var propertyResponse = new PropertyResponseModel
            {
                Property = property.Change(),
                Images = property.PropertyImages.Select(x => x.Change()).ToList(),
                Reviews = property.Reviews.Select(x => x.Change()).ToList()
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

    public async Task<Result<PropertyResponseModel>> CreateProperty(PropertyRequestModel requestModel)
    {
        Result<PropertyResponseModel> model = null;
        try
        {
            if (requestModel == null)
            {
                throw new ArgumentNullException(nameof(requestModel), "Request model cannot be null");
            }
            var isAgentExist = _db.Agents.AsNoTracking()
                                         .FirstOrDefault(x => x.AgentId == requestModel.AgentId)
                                         ?? throw new Exception("Agent Id does not exist");

            var property = requestModel.Change()
                           ?? throw new Exception("Failed to convert request model to property entity");
            property.Status = nameof(PropertyStatus.Pending);

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
                                    .Include(x => x.Reviews)
                                    .FirstOrDefaultAsync(x => x.PropertyId == property.PropertyId)
                                    ?? throw new Exception("Property Not Found");

            var propertyResponse = new PropertyResponseModel
            {
                Property = createdProperty.Change(),
                Images = createdProperty.PropertyImages.Select(x => x.Change()).ToList(),
                Reviews = property.Reviews.Select(x => x.Change()).ToList()
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

            var isAgentExist = _db.Agents.AsNoTracking()
                                         .FirstOrDefault(x => x.AgentId == requestModel.AgentId)
                                         ?? throw new Exception("Agent Id does not exist");

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
            property.AvailiablityType = requestModel.AvailiablityType;
            property.MinrentalPeriod = requestModel.MinRentalPeriod;
            property.Editdate = DateTime.Now;

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
                                    .Include(x => x.Reviews)
                                    .FirstOrDefaultAsync(x => x.PropertyId == property.PropertyId)
                                    ?? throw new Exception("Property Not Found");

            var responseModel = new PropertyResponseModel
            {
                Property = updatedProperty.Change(),
                Images = updatedProperty.PropertyImages.Select(x => x.Change()).ToList(),
                Reviews = property.Reviews.Select(x => x.Change()).ToList()
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

    public async Task<Result<PropertyResponseModel>> ChangePropertyStatus(PropertyStatusChangeRequestModel requestModel)
    {
        Result<PropertyResponseModel> model = null;
        try
        {
            var property = await _db.Properties
                                    .Include(x => x.PropertyImages)
                                    .FirstOrDefaultAsync(x => x.PropertyId == requestModel.PropertyId)
                                    ?? throw new Exception("Property Not Found");

            if (!Enum.TryParse<PropertyStatus>(requestModel.PropertyStatus, out var parsedStatus) || !Enum.IsDefined(typeof(PropertyStatus), parsedStatus))
            {
                throw new Exception($"Invalid Status; Status should be one of the following: {string.Join(", ", Enum.GetNames(typeof(PropertyStatus)))}");
            }

            property.Status = requestModel.PropertyStatus;
            property.Approvedby = requestModel.ApprovedBy;

            _db.Properties.Update(property);
            await _db.SaveChangesAsync();

            var updatedProperty = await _db.Properties
                                    .AsNoTracking()
                                    .Include(x => x.PropertyImages)
                                    .Include(x => x.Reviews)
                                    .FirstOrDefaultAsync(x => x.PropertyId == property.PropertyId)
                                    ?? throw new Exception("Property Not Found");

            var responseModel = new PropertyResponseModel
            {
                Property = updatedProperty.Change(),
                Images = updatedProperty.PropertyImages.Select(x => x.Change()).ToList(),
                Reviews = property.Reviews.Select(x => x.Change()).ToList()
            };
            model = Result<PropertyResponseModel>.Success(responseModel);
            return model;
        }
        catch (Exception ex)
        {
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

            property.Status = nameof(PropertyStatus.Canceled);
            _db.Properties.Update(property);
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

    private static IQueryable<Database.AppDbContextModels.Property> ApplyFilters(
        IQueryable<Database.AppDbContextModels.Property> query,
        int? agentId, string? address, string? city,
        string? state, string? zipCode, string? propertyType,
        decimal? minPrice, decimal? maxPrice,
        decimal? size, int? numberOfBedrooms,
        int? numberOfBathrooms, int? yearBuilt, string? availabilityType,
        int? minRentalPeriod, string? approvedBy, DateTime? addDate,
        DateTime? editDate, string? propertyStatus)
    {
        if (agentId.HasValue)
        {
            query = query.Where(x => x.AgentId == agentId);
        }

        if (!string.IsNullOrEmpty(address))
        {
            query = query.Where(x => x.Address.Contains(address));
        }

        if (!string.IsNullOrEmpty(city))
        {
            query = query.Where(x => x.City.Contains(city));
        }

        if (!string.IsNullOrEmpty(state))
        {
            query = query.Where(x => x.State.Contains(state));
        }

        if (!string.IsNullOrEmpty(zipCode))
        {
            query = query.Where(x => x.ZipCode.Contains(zipCode));
        }

        if (!string.IsNullOrEmpty(propertyType))
        {
            query = query.Where(x => x.PropertyType.Contains(propertyType));
        }

        if (minPrice.HasValue)
        {
            query = query.Where(x => x.Price >= minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            query = query.Where(x => x.Price <= maxPrice.Value);
        }

        if (size.HasValue)
        {
            query = query.Where(x => x.Size == size.Value);
        }

        if (numberOfBedrooms.HasValue)
        {
            query = query.Where(x => x.NumberOfBedrooms == numberOfBedrooms.Value);
        }

        if (numberOfBathrooms.HasValue)
        {
            query = query.Where(x => x.NumberOfBathrooms == numberOfBathrooms.Value);
        }

        if (yearBuilt.HasValue)
        {
            query = query.Where(x => x.YearBuilt == yearBuilt.Value);
        }

        if (!string.IsNullOrEmpty(availabilityType))
        {
            query = query.Where(x => x.AvailiablityType == availabilityType);
        }

        if (minRentalPeriod.HasValue)
        {
            query = query.Where(x => x.MinrentalPeriod == minRentalPeriod.Value);
        }

        if (!string.IsNullOrEmpty(approvedBy))
        {
            query = query.Where(x => x.Approvedby.Contains(approvedBy));
        }

        if (addDate.HasValue)
        {
            query = query.Where(x => x.Adddate.HasValue && x.Adddate.Value.Date == addDate.Value.Date);
        }

        if (editDate.HasValue)
        {
            query = query.Where(x => x.Editdate.HasValue && x.Editdate.Value.Date == editDate.Value.Date);
        }

        if (!string.IsNullOrWhiteSpace(propertyStatus))
        {
            query = query.Where(x => x.Status == propertyStatus);
        }

        return query;
    }
}