using REMS.Database.AppDbContextModels;

namespace REMS.Models.Review;

public class ReviewModel
{
    public int ReviewId { get; set; }

    public int? UserId { get; set; }

    public int? PropertyId { get; set; }

    public int Rating { get; set; }

    public string? Comments { get; set; }

    public DateTime? DateCreated { get; set; }
}