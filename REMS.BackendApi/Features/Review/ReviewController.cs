namespace REMS.BackendApi.Features.Review;

[Route("api/v1/reviews")]
[ApiController]
public class ReviewController : ControllerBase
{
    private readonly BL_Review _blReview;

    public ReviewController(BL_Review blReview)
    {
        _blReview = blReview;
    }

    [HttpGet]
    public async Task<IActionResult> GetReview()
    {
        try
        {
            var reviewList = await _blReview.GetReview();
            return Ok(reviewList);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
        }
    }

    [HttpGet("{pageNo}/{pageSize}")]
    public async Task<IActionResult> GetReviews(int pageNo, int pageSize)
    {
        try
        {
            var response = await _blReview.GetReviews(pageNo, pageSize);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{reviewId}")]
    public async Task<IActionResult> GetReviewById(int reviewId)
    {
        try
        {
            var response = await _blReview.GetReviewById(reviewId);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateReview(ReviewRequestModel requestModel)
    {
        try
        {
            var customer = await _blReview.CreateReview(requestModel);
            return Ok(customer);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
        }
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(int id, ReviewRequestModel requestModel)
    {
        try
        {
            var review = await _blReview.UpdateReview(id, requestModel);
            return Ok(review);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var review = await _blReview.DeleteReview(id);
            return Ok(review);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
        }
    }
}