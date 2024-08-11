using REMS.Models.Review;

namespace REMS.Models.Property;

public class PropertyResponseModel
{
    public PropertyModel Property { get; set; }
    public List<PropertyImageModel> Images { get; set; }

    public List<ReviewModel> Reviews { get; set; }
}