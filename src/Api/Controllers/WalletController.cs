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
using Net.payOS.Types;

namespace Api.Controllers
{
    [Route("api/wallet")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;
        private readonly ILogger<WalletController> _logger;

        public WalletController(IWalletService walletService, ILogger<WalletController> logger)
        {
            _walletService = walletService;
            _logger = logger;
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
        [CustomAuthorize(RoleEnum.Student)]
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
        [HttpPost("webhook")]
        public async Task<IActionResult> ConfirmWebhook(string webhookUrl)
        {
            try
            {
                var rs = await _walletService.ConfirmWebhook(webhookUrl);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Webhook processing failed");
            }
        }
        [HttpPost("webhook-handle")]
        public async Task<IActionResult> HandleWebhook(WebhookType webhookBody)
        {
            try
            {
                var rs = _walletService.HandleWebhook(webhookBody);
                _logger.LogInformation("Received webhook: {@WebhookBody}", rs);
                return Ok(webhookBody);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
