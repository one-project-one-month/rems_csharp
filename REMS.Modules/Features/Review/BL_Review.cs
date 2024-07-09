namespace REMS.Modules.Features.Review;

public class BL_Review
{
    private readonly DA_Review _daReview;

    public BL_Review(DA_Review daReview) => _daReview = daReview;


    public async Task<ReviewListResponseModel> GetReview()
    {
        var response = await _daReview.GetReview();
        return response;
    }

    public async Task<ReviewListResponseModel> GetReviews(int pageNo, int pageSize)
    {
        if (pageNo < 1 || pageSize < 1)
        {
            throw new Exception("PageNo or PageSize Cannot be less than 1");
        }

        var response = await _daReview.GetReviews(pageNo, pageSize);
        return response;
    }

    public async Task<ReviewResponseModel> GetReviewById(int reviewId)
    {
        if (reviewId < 1)
        {
            throw new Exception("Invalid ReviewId");
        }

        var response = await _daReview.GetReviewById(reviewId);
        return response;
    }

    public async Task<MessageResponseModel> CreateReview(ReviewModel requestModel)
    {
        var response = await _daReview.CreateReview(requestModel);
        return response;
    }

    public async Task<MessageResponseModel> UpdateReview(int id, ReviewModel requestModel)
    {
        if (id <= 0) throw new Exception("ReviewId is null");
        //CheckProductNullValue(requestModel);
        var response = await _daReview.UpdateReview(id, requestModel);
        return response;
    }

    public async Task<MessageResponseModel> DeleteReview(int id)
    {
        if (id <= 0) throw new Exception("ReviewId is null");
        var response = await _daReview.DeleteReview(id);
        return response;
    }
}