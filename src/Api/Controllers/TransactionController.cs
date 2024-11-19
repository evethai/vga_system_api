using Api.Constants;
using Api.Validators;
using Application.Interface.Service;
using Domain.Enum;
using Domain.Model.Transaction;
using Infrastructure.Persistence.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }
        [Authorize]
        [HttpGet(ApiEndPointConstant.Transaction.TransactionEndPoint)]
        public async Task<IActionResult> GetListTransactionAsync([FromQuery] TransactionSearchModel searchModel)
        {
            var result = await _transactionService.GetListTransactionAsync(searchModel);
            return Ok(result);
        }

        //[CustomAuthorize(RoleEnum.Consultant)]
        [HttpPost(ApiEndPointConstant.Transaction.TransactionWithdrawRequest)]
        public async Task<IActionResult> CreateWithdrawAsync(Guid id, int goldAmount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _transactionService.CreateWithdrawAsync(id, goldAmount);
                return (result.IsSuccess == false)
                    ? BadRequest(result)
                    : Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[CustomAuthorize(RoleEnum.University)]
        [HttpPut(ApiEndPointConstant.Transaction.TransactionProcessRequest)]
        public async Task<IActionResult> ProcessWithdrawRequestAsync(Guid id, TransactionType type)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _transactionService.ProcessWithdrawRequestAsync(id, type);
                return (result.IsSuccess == false)
                    ? BadRequest(result)
                    : Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
