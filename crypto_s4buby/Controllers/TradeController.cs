using crypto_s4buby.Context.Dtos;
using crypto_s4buby.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace crypto_s4buby.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TradeController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TradeController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost("buy")]
        public async Task<IActionResult> Buy([FromBody]TransactionBuyDto transactionBuyDto)
        {
            try
            {
                var res = await _transactionService.Buy(transactionBuyDto);
                return Ok(res);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("sell")]
        public async Task<IActionResult> Sell([FromBody] TransactionSellDto transactionSellDto)
        {
            try
            {
                var res = await _transactionService.Sell(transactionSellDto);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
