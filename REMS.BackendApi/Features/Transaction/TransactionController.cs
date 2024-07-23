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
    }

