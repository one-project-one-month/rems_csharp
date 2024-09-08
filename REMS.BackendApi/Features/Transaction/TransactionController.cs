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

    [HttpGet]
    public async Task<IActionResult> GetTransactions(int pageNumber = 1, int pageSize = 10)
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

    [HttpGet("Property")]
    public async Task<IActionResult> GetTransactionsByPropertyId(int propertyId, int pageNo = 1, int pageSize = 10)
    {
        try
        {
            var response = await _blTransaction.GetTransactionsByPropertyIdAsync(propertyId, pageNo, pageSize);
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

    [HttpGet("Property/Client")]
    public async Task<IActionResult> GetTransactionsByPropertyIdAndClientId(int propertyId, int clientId, int pageNo = 1, int pageSize = 10)
    {
        try
        {
            var response = await _blTransaction.GetTransactionsByPropertyIdAndClientIdAsync(propertyId, clientId, pageNo, pageSize);
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

    [HttpGet("Client")]
    public async Task<IActionResult> GetTransactionsByClientId(int clientId, int pageNo, int pageSize)
    {
        try
        {
            var response = await _blTransaction.GetTransactionsByClientIdAsync(clientId, pageNo, pageSize);
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

