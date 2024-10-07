using Api.Constants;
using Application.Interface.Service;
using Domain.Entity;
using Domain.Model.Highschool;
using Domain.Model.Response;
using Domain.Model.Transaction;
using Domain.Model.Wallet;
using Infrastructure.Persistence.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/wallet")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }
        [HttpGet(ApiEndPointConstant.Wallet.WalletsEndpoint)]
        public async Task<IActionResult> GetListWallet()
        {
            var result = await _walletService.GetAllWallet();
            return Ok(result);
        }

        [HttpGet(ApiEndPointConstant.Wallet.WalletEndpoint)]
        public async Task<IActionResult> GetWalletByIdAsync(Guid id)
        {
            var result = await _walletService.GetWalletByIdAsync(id);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateWalletUsingGoldBookConsultantAsync( WalletPutModel putModel, int goldTransaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _walletService.UpdateWalletUsingGoldBookConsultantAsync(putModel, goldTransaction);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut(ApiEndPointConstant.Wallet.WalletPutEndpoint)]
        public async Task<IActionResult> UpdateWalletUsingGoldDistributionAsync([FromForm]Guid putModel, int goldTransaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _walletService.UpdateWalletUsingGoldDistributionAsync(putModel, goldTransaction);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
