using Banking.Application.API.Dto;
using Banking.Application.API.Helper;
using Banking.Domain.Service.TransactionLogic;
using Microsoft.AspNetCore.Mvc;

namespace Banking.Application.API.Controllers
{
    [Route("api/transactions")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit([FromBody] DepositRequest req)
        {
            var result = await _transactionService.Deposit(req.AccountId, req.Amount);

            return Ok(ResponseAPI.Ok(result));
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> Tranfer([FromBody] TransferRequest req)
        {
            var result = await _transactionService.Transfer(req.FromAccountId, req.ToAccountId, req.Amount);

            return Ok(ResponseAPI.Ok(result));
        }
    }
}