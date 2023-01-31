using Banking.Domain.Entities;
using Banking.Domain.Entities.Repository;
using Banking.Domain.Service.Dto;
using Banking.Infrastructure.Persistence;
using Banking.Infrastructure.Persistence.UnitOfWork;

namespace Banking.Domain.Service.AccountLogic
{
    public class AccountService : IAccountService
    {
        private readonly IRepository<Account, BankingContext> _repository;

        private readonly IUnitOfWork<BankingContext> _uow;

        private readonly IIBAN_Service _ibanService;

        public AccountService(IIBAN_Service ibanService,
            IRepository<Account, BankingContext> repository,
            IUnitOfWork<BankingContext> uow)
        {
            _ibanService = ibanService;
            _repository = repository;
            _uow = uow;
        }

        public async Task<AccountDto> GetAccountById(Guid id)
        {
            return AccountDto.FromEntity(await _repository.GetByIdAsync(id));
        }

        public async Task<AccountDto> CreateAccount(AccountDto dto)
        {
            var entity = AccountDto.ToEntity(dto);
            entity.IBAN = await _ibanService.GenerateIBAN();
            entity.CreatedOn = DateTime.UtcNow;

            _repository.Insert(entity);
            await _uow.CommitAsync();

            return AccountDto.FromEntity(entity);
        }
    }
}