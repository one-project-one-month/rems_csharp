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

    [HttpPost("GetTransactionsByPropertyIdAndClientId/{clientId}/{propertyId}/{pageNumber}/{pageSize}")]
    public async Task<IActionResult> GetTransactionsByPropertyIdAndClientId(int pageNumber, int pageSize, int propertyId, int clientId)
    {
        try
        {
            var response = await _blTransaction.GetTransactionsByPropertyIdAndClientIdAsync(pageNumber, pageSize, propertyId, clientId);
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

    [HttpPost("GetTransactionsByClientId/{clientId}/{pageNumber}/{pageSize}")]
    public async Task<IActionResult> GetTransactionsByClientId(int pageNumber, int pageSize, int clientId)
    {
        try
        {
            var response = await _blTransaction.GetTransactionsByClientIdAsync(pageNumber, pageSize, clientId);
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

