using System;
namespace REMS.Models.Property;

public class PropertyRequestModel
{
    public int? PropertyId { get; set; }

    public string Address { get; set; }

    public string City { get; set; }

    public string State { get; set; }

    public string ZipCode { get; set; }

    public string PropertyType { get; set; }

    public decimal Price { get; set; }

    public decimal Size { get; set; }

    public int? NumberOfBedrooms { get; set; }

    public int? NumberOfBathrooms { get; set; }

    public int? YearBuilt { get; set; }

    public string? Description { get; set; }

    public string Status { get; set; }

    public DateTime? DateListed { get; set; }

    public List<PropertyImageRequestModel> Images { get; set; }
}

public class PropertyImageRequestModel
{
    public string? ImgBase64 { get; set; }
    public string? Description { get; set; }
}

