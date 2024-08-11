namespace REMS.Modules.Features.Review;

public class DA_Review
{
    private readonly AppDbContext _context;

    public DA_Review(AppDbContext context) => _context = context;

    public async Task<Result<ReviewListResponseModel>> GetReview()
    {
        Result<ReviewListResponseModel> model = null;
        var responseModel = new ReviewListResponseModel();
        try
        {
            var reviews = await _context
                .Reviews
                .AsNoTracking()
                .ToListAsync();
            var reviewResponseModel = reviews.Select(review => new ReviewResponseModel
            {
                Review = review.Change()
            }).ToList();

            var reviewListResponse = new ReviewListResponseModel
            {
                DataList = reviewResponseModel,
            };

            model = Result<ReviewListResponseModel>.Success(reviewListResponse);
        }
        catch (Exception ex)
        {
            model = Result<ReviewListResponseModel>.Error(ex);
            return model;
        }

        return model;
    }

    public async Task<Result<ReviewListResponseModel>> GetReviews(int pageNo = 1, int pageSize = 10)
    {
        Result<ReviewListResponseModel> model = null;
        try
        {
            var totalCount = await _context.Reviews.CountAsync();
            var pageCount = (int)Math.Ceiling((double)totalCount / pageSize);

            var reviews = await _context.Reviews
                .AsNoTracking()
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var reviewResponseModel = reviews.Select(review => new ReviewResponseModel
            {
                Review = review.Change()
            }).ToList();
            var reviewListResponse = new ReviewListResponseModel
            {
                DataList = reviewResponseModel,
                PageSetting = new PageSettingModel(pageNo, pageSize, pageCount, totalCount)
            };

            model = Result<ReviewListResponseModel>.Success(reviewListResponse);
        }
        catch (Exception ex)
        {
            model = Result<ReviewListResponseModel>.Error(ex);
            return model;
        }
        return model;
    }

    public async Task<Result<ReviewResponseModel>> GetReviewById(int reviewId)
    {
        Result<ReviewResponseModel> model = null;
        try
        {
            var review = await _context
                .Reviews
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ReviewId == reviewId);
            if (review is null)
            {
                throw new Exception("review Not Found");
            }
            var responseModel = new ReviewResponseModel
            {
                Review = review.Change()
            };
            model = Result<ReviewResponseModel>.Success(responseModel);
        }
        catch (Exception ex)
        {
            model = Result<ReviewResponseModel>.Error(ex);
        }

        return model;
    }

    public async Task<Result<ReviewResponseModel>> CreateReview(ReviewRequestModel requestModel)
    {
        Result<ReviewResponseModel> model = null;
        try
        {
            if (requestModel == null)
            {
                throw new ArgumentNullException(nameof(requestModel), "Request model cannot be null");
            }

            var review = requestModel.Change();
            await _context.Reviews.AddAsync(review);
            int addReview = await _context.SaveChangesAsync();
            var responseModel = new ReviewResponseModel
            {
                Review = review.Change(),
            };

            model = addReview > 0
                ? Result<ReviewResponseModel>.Success(responseModel)
                : Result<ReviewResponseModel>.Error("Review create failed.");
        }
        catch (Exception ex)
        {
            model = Result<ReviewResponseModel>.Error(ex);
        }
        return model;
    }

    public async Task<Result<ReviewResponseModel>> UpdateReview(int id, ReviewRequestModel requestModel)
    {
        Result<ReviewResponseModel> model = null;
        try
        {
            var review = await _context.Reviews
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ReviewId == id);

            if (review is null)
            {
                new MessageResponseModel(false, "Not Found Error");
            }

            #region Patch Method Validation Conditions

            if (requestModel.UserId != null)
            {
                review.UserId = requestModel.UserId;
            }

            if (requestModel.PropertyId != null)
            {
                review.PropertyId = requestModel.PropertyId;
            }

            if (requestModel.Rating != null)
            {
                review.Rating = requestModel.Rating;
            }

            if (!string.IsNullOrEmpty(requestModel.Comments))
            {
                review.Comments = requestModel.Comments;
            }

            if (requestModel.DateCreated != null)
            {
                review.DateCreated = requestModel.DateCreated;
            }

            #endregion

            _context.Reviews.Update(review);
            var result = await _context.SaveChangesAsync();

            var reviewResponseModel = new ReviewResponseModel
            {
                Review = review.Change()
            };

            model = Result<ReviewResponseModel>.Success(reviewResponseModel);
        }
        catch (Exception ex)
        {
            model = Result<ReviewResponseModel>.Error(ex);
        }
        return model;
    }

    public async Task<Result<object>> DeleteReview(int id)
    {
        Result<object> model = null;
        try
        {
            var review = await _context
                .Reviews
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ReviewId == id);
            if (review is null)
            {
                new MessageResponseModel(false, "Review Not Found");
            }

            _context.Reviews.Remove(review);
            var result = await _context.SaveChangesAsync();
            model = result > 0
                ? Result<object>.Success(null)
                : Result<object>.Error("Delete failed.");
        }
        catch (Exception ex)
        {
            return model = Result<object>.Error(ex);
        }
        return model;
    }
}