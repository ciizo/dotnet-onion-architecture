using Banking.Application.API.Helper;
using Banking.Domain.Service.AccountLogic;
using Microsoft.AspNetCore.Mvc;

namespace Banking.Application.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountById([FromRoute] Guid id)
        {
            var result = await _accountService.GetAccountById(id);

            return Ok(ResponseAPI.Ok(result));
        }
    }
}