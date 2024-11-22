using Api.Constants;
using Api.Validators;
using Application.Interface.Service;
using Application.Library;
using Domain.Entity;
using Domain.Enum;
using Domain.Model.Highschool;
using Domain.Model.Response;
using Domain.Model.Transaction;
using Domain.Model.Wallet;
using Infrastructure.Persistence.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

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
        [CustomAuthorize(RoleEnum.Admin)]
        [HttpGet(ApiEndPointConstant.Wallet.WalletsEndpoint)]
        public async Task<IActionResult> GetListWallet()
        {
            var result = await _walletService.GetAllWallet();
            return Ok(result);
        }
        [Authorize]
        [HttpGet(ApiEndPointConstant.Wallet.WalletEndpoint)]
        public async Task<IActionResult> GetWalletByIdAsync(Guid AccountId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _walletService.GetWalletByIdAsync(AccountId);
            return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [CustomAuthorize(RoleEnum.Admin, RoleEnum.University)]
        [HttpPut(ApiEndPointConstant.Wallet.WalletTransferringAndReceiving)]
        public async Task<IActionResult> UpdateWalletTransferringAndReceivingAsync(WalletPutModel putModel, int gold)
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
        [CustomAuthorize(RoleEnum.Admin, RoleEnum.HighSchool)]
        [HttpPut(ApiEndPointConstant.Wallet.WalletDistributionEndpoint)]
        public async Task<IActionResult> UpdateWalletUsingGoldDistributionAsync(TransactionPutWalletModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _walletService.UpdateWalletUsingGoldDistributionAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [CustomAuthorize(RoleEnum.Admin, RoleEnum.Student)]
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
        [HttpPost(ApiEndPointConstant.Wallet.WalletPayOsRequest)]
        public async Task<IActionResult> RequestTopUpWalletWithPayOs([FromQuery] Guid accountId, [FromQuery] float amount, PayOSUrl url)
        {
            try
            {
                var paymenturl = await _walletService.RequestTopUpWalletWithPayOsAsync(accountId, amount,url);
                return Ok(paymenturl);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(ApiEndPointConstant.Wallet.WalletPayOsResponse)]
        public async Task<IActionResult> RequestDepositToWalletWithPayOs([FromQuery] Guid transactionId, [FromQuery] string status)
        {
            try
            {
                var paymenturl = await _walletService.RequestDepositToWalletWithPayOs(transactionId, status);
                return Ok(paymenturl);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
