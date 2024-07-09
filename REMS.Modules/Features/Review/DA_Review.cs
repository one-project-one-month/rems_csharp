using Microsoft.EntityFrameworkCore;
using REMS.Database.AppDbContextModels;
using REMS.Mapper;
using REMS.Models;
using REMS.Models.Custom;
using REMS.Models.Property;
using REMS.Models.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Modules.Features.Review;

public class DA_Review
{
    private readonly AppDbContext _context;
        
    public DA_Review(AppDbContext context) => _context = context;

    public async Task<ReviewListResponseModel> GetReview()
    {
        var responseModel = new ReviewListResponseModel();
        try
        {
            var reviews = await _context
                .Reviews
                .AsNoTracking()
                .ToListAsync();
            responseModel.DataList = reviews
                .Select(x => x.Change()).ToList();
        }
        catch (Exception ex)
        {
            responseModel.DataList = new List<ReviewModel>();
        }

        return responseModel;
    }

    public async Task<ReviewListResponseModel> GetReviews(int pageNo = 1, int pageSize = 10)
    {
        try
        {
            // Calculate the total count and page count
            var totalCount = await _context.Reviews.CountAsync();
            var pageCount = (int)Math.Ceiling((double)totalCount / pageSize);

            // Retrieve the paginated reviews
            var query = await _context
                .Reviews
                .AsNoTracking()
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Construct the response model
            var response = new ReviewListResponseModel

            {
                DataList = query.Select(x => x.Change()).ToList(),
                PageSetting = new PageSettingModel(pageNo, pageSize, pageCount, totalCount)
            };

            return response;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }


    public async Task<ReviewResponseModel> GetReviewById(int reviewId)
    {
        try
        {
            var review = await _context.Reviews
                             .AsNoTracking()
                             .FirstOrDefaultAsync(x => x.ReviewId == reviewId)
                         ?? throw new Exception("Review Not Found");

            var responseModel = new ReviewResponseModel
            {
                Review = review.Change()
            };
            return responseModel;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public async Task<MessageResponseModel> CreateReview(ReviewModel requestModel)
    {
        try
        {
            await _context.Reviews.AddAsync(requestModel.Change());
            int addReview = await _context.SaveChangesAsync();
            var response = addReview > 0 ? new MessageResponseModel(true, "Successfully Save") :
                new MessageResponseModel(false, "Review Create Fail");
            return response;
        }
        catch (Exception ex)
        {
            return new MessageResponseModel(false, ex);
        }
    }

    public async Task<MessageResponseModel> UpdateReview(int id, ReviewModel requestModel)
    {
        try
        {
            var review = await _context.Reviews.AsNoTracking()
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

            if(requestModel.DateCreated != null)
            {
                review.DateCreated = requestModel.DateCreated;
            }

            #endregion

            _context.Reviews.Update(review);
            var result = await _context.SaveChangesAsync();

            var response = result > 0
                ? new MessageResponseModel(true, "Update Successfully")
                : new MessageResponseModel(false, "Update failed");
            return response;
        }
        catch (Exception ex)
        {
            return new MessageResponseModel(false, ex);
        }
    }

    public async Task<MessageResponseModel> DeleteReview(int id)
    {
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
            var response = result > 0
                ? new MessageResponseModel(true, "Delete Successfully")
                : new MessageResponseModel(false, "Delete Fail");
            return response;
        }
        catch (Exception ex)
        {
            return new MessageResponseModel(false, ex);
        }
    }

}