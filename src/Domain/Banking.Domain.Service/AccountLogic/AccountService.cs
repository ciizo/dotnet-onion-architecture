using Banking.Domain.Service.Dto;
using Banking.Infrastructure.Persistence;
using Banking.Infrastructure.Persistence.Entities;
using Banking.Infrastructure.Persistence.Repository.EFCore;
using Banking.Infrastructure.Persistence.UnitOfWork;

namespace Banking.Domain.Service.AccountLogic
{
    public class AccountService : IAccountService
    {
        protected IRepositoryEF<Account> _repository;

        protected IUnitOfWork<BankingContext> _uow;

        public AccountService(IRepositoryEF<Account> repository, IUnitOfWork<BankingContext> uow)
        {
            _repository = repository;
            _uow = uow;
        }

        public async Task<AccountDto> GetAccountById(Guid id)
        {
            return AccountDto.FromEntity(await _repository.GetByIdAsync(id));
        }
    }
}