namespace REMS.Models.Property;

public class PropertyImageModel
{
    public int ImageId { get; set; }

    public int? PropertyId { get; set; }

    public string ImageUrl { get; set; }

    public string? Description { get; set; }

    public DateTime? DateUploaded { get; set; }
}