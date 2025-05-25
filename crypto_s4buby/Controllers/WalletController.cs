using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using crypto_s4buby.Context.Context;
using crypto_s4buby.Context.Entities;
using crypto_s4buby.Services;
using System.Runtime.CompilerServices;
using crypto_s4buby.Context.Dtos;

namespace crypto_s4buby.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        // GET: api/Wallet/5
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetWallet(int userId)
        {
            try
            {
                var res = await _walletService.GetWalletAsync(userId);
                return Ok(res);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> PutWallet(int userId, [FromBody]WalletUpdateDto walletUpdateDto)
        {
            try
            {
                var res = await _walletService.SetBalanceAsync(userId, walletUpdateDto);
                return Ok(res);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Wallet/5
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteWallet(int userId)
        {
            try
            {
                var res = await _walletService.DeleteWalletAsync(userId);
                return Ok(res);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
