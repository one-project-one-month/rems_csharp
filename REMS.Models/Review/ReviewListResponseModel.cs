using REMS.Models.Custom;

namespace REMS.Models.Review;

public class ReviewListResponseModel
{
    public List<ReviewResponseModel> DataList { get; set; }

    public PageSettingModel PageSetting { get; set; }
}