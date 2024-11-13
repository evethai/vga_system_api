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
        public async Task<IActionResult> GetWalletByIdAsync(Guid AcccountId)
        {
            var result = await _walletService.GetWalletByIdAsync(AcccountId);
            return Ok(result);
        }
        [HttpPut(ApiEndPointConstant.Wallet.WalletTransferringAndReceiving)]
        public async Task<IActionResult> UpdateWalletUsingGoldBookConsultantAsync([FromForm] WalletPutModel putModel, int gold)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _walletService.UpdateWalletByTransferringAndReceivingAsync(putModel, gold);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut(ApiEndPointConstant.Wallet.WalletDistribution)]
        public async Task<IActionResult> UpdateWalletUsingGoldDistributionAsync([FromForm] Guid WalletHighschoolId, int GoldDistribution)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _walletService.UpdateWalletUsingGoldDistributionAsync(WalletHighschoolId, GoldDistribution);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut(ApiEndPointConstant.Wallet.WalletTest)]
        public async Task<IActionResult> UpdateWalletUsingByTestAsync(Guid AccountId, int goldUsingTest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _walletService.UpdateWalletUsingByTestAsync(AccountId, goldUsingTest);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
