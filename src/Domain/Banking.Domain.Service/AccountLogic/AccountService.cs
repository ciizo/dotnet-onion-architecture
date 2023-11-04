using Banking.Domain.Entities;
using Banking.Domain.Entities.Repository;
using Banking.Domain.Entities.UnitOfWork;
using Banking.Domain.Service.Dto;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Banking.Domain.Service.AccountLogic
{
    public class AccountService : IAccountService
    {
        private readonly IRepository<Account, IDbContext> _repository;

        private readonly IUnitOfWork<IDbContext> _uow;

        public AccountService(
            IRepository<Account, IDbContext> repository,
            IUnitOfWork<IDbContext> uow)
        {
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
            entity.IBAN = await GenerateIBAN();
            entity.CreatedOn = DateTime.UtcNow;

            _repository.Insert(entity);
            await _uow.CommitAsync();

            return AccountDto.FromEntity(entity);
        }

        /// <summary>
        /// TODO clone logic from js on website http://randomiban.com/?country=Netherlands
        /// </summary>
        /// <returns></returns>
        private async Task<string> GenerateIBAN()
        {
            string iban;
            var options = new ChromeOptions();
            options.AddArgument("--headless");
            options.AddArgument("--disable-gpu");
            // ref https://sites.google.com/chromium.org/driver/downloads
            ChromeDriverService service = ChromeDriverService.CreateDefaultService(@"D:\chromedriver_win32_109", "chromedriver.exe");
            using (var driver = new ChromeDriver(service, options))
            {
                driver.Navigate().GoToUrl("http://randomiban.com/?country=Netherlands");
                iban = driver.FindElement(By.Id("demo")).Text;
            }

            return iban;
        }
    }
}