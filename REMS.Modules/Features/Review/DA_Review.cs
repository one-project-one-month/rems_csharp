using Microsoft.EntityFrameworkCore;
using REMS.Database.AppDbContextModels;
using REMS.Mapper;
using REMS.Models;
using REMS.Models.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Modules.Features.Review
{
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

    }
}
