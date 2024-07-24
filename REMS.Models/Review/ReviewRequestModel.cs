namespace REMS.Models.Review;

public class ReviewRequestModel
{

    public int? UserId { get; set; }

    public int? PropertyId { get; set; }

    public int Rating { get; set; }

    public string? Comments { get; set; }

    public DateTime? DateCreated { get; set; } = DateTime.Now;
}
