using crypto_s4buby.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace crypto_s4buby.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfitController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public ProfitController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetProfit(int userId)
        {
            try
            {
                var res = await _walletService.GetProfit(userId);
                return Ok(res);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("details/{userId}")]
        public async Task<IActionResult> GetProfitDetails(int userId)
        {
            try
            {
                var res = await _walletService.GetProfitDetails(userId);
                return Ok(res);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
