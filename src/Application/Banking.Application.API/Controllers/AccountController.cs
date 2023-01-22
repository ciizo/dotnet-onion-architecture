using Banking.Application.API.Helper;
using Banking.Domain.Service.AccountLogic;
using Microsoft.AspNetCore.Mvc;

namespace Banking.Application.API.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccount([FromRoute] Guid id)
        {
            var result = await _accountService.GetAccountById(id);

            return Ok(ResponseAPI.Ok(result));
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateAccount()
        {
            var result = await _accountService.CreateAccount(new Domain.Service.Dto.AccountDto());

            return Ok(ResponseAPI.Ok(result));
        }
    }
}