using Api.Constants;
using Application.Interface.Service;
using Domain.Model.Transaction;
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
        [HttpGet(ApiEndPointConstant.Transaction.TransactionEndPoint)]
        public async Task<IActionResult> GetListTransactionAsync([FromQuery] TransactionSearchModel searchModel)
        {
            var result = await _transactionService.GetListTransactionAsync(searchModel);
            return Ok(result);
        }  
    }
}
