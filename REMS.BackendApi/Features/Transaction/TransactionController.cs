namespace REMS.BackendApi.Features.Transaction;

[Route("api/v1/transactions")]
[ApiController]
public class TransactionController : ControllerBase
{
    private readonly BL_Transaction _blTransaction;

    public TransactionController(BL_Transaction blTransaction)
    {
        _blTransaction = blTransaction;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTransaction(TransactionRequestModel requestModel)
    {
        try
        {
            var response = await _blTransaction.CreateTransactionAsync(requestModel);
            if (response.IsError)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
        }
    }

    [HttpGet("{pageNumber}/{pageSize}")]
    public async Task<IActionResult> GetTransactions(int pageNumber, int pageSize)
    {
        try
        {
            var response = await _blTransaction.GetTransactionsAsync(pageNumber, pageSize);
            if (response.IsError)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
        }
    }

    [HttpGet("{propertyId}/{pageNumber}/{pageSize}")]
    public async Task<IActionResult> GetTransactionsByPropertyId(int pageNumber, int pageSize, int propertyId)
    {
        try
        {
            var response = await _blTransaction.GetTransactionsByPropertyIdAsync(pageNumber, pageSize, propertyId);
            if (response.IsError)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
        }
    }

    [HttpPost("GetTransactionsByPropertyIdAndBuyerId/{buyerId}/{propertyId}/{pageNumber}/{pageSize}")]
    public async Task<IActionResult> GetTransactionsByPropertyIdAndBuyerId(int pageNumber, int pageSize, int propertyId, int buyerId)
    {
        try
        {
            var response = await _blTransaction.GetTransactionsByPropertyIdAndClientIdAsync(pageNumber, pageSize, propertyId, buyerId);
            if (response.IsError)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
        }
    }

    [HttpGet("GetTransactionsByPropertyIdAndSellerId/{pageNumber}/{pageSize}/{propertyId}/{sellerId}")]
    public async Task<IActionResult> GetTransactionsByPropertyIdAndSellerId(int pageNumber, int pageSize, int propertyId, int sellerId)
    {
        try
        {
            var response = await _blTransaction.GetTransactionsByPropertyIdAndSellerIdAsync(pageNumber, pageSize, propertyId, sellerId);
            if (response.IsError)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
        }
    }
}

