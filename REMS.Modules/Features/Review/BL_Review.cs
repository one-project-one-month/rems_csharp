using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using REMS.Models;
using REMS.Models.Property;
using REMS.Models.Review;
using REMS.Modules.Features.Property;

namespace REMS.Modules.Features.Review
{
    public class BL_Review
    {
        private readonly DA_Review _daReview;
        
        public BL_Review(DA_Review daReview) => _daReview = daReview;
        

        public async Task<ReviewListResponseModel> GetReview()
        {
            var response = await _daReview.GetReview();
            return response;
        }

        public async Task<ReviewResponseModel> GetReviewById(int reviewId)
        {
            if (reviewId < 1 )
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
    }
}
