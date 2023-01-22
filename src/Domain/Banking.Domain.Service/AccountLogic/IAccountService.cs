using Banking.Domain.Service.Dto;

namespace Banking.Domain.Service.AccountLogic
{
    public interface IAccountService
    {
        Task<AccountDto> GetAccountById(Guid id);

        Task<AccountDto> CreateAccount(AccountDto dto);
    }
}