using Banking.Domain.Entities;

namespace Banking.Domain.Service.Dto
{
    public class AccountDto
    {
        public Guid ID { get; set; }
        public string IBAN { get; set; }
        public DateTime CreatedOn { get; set; }

        public static AccountDto FromEntity(Account entity)
        {
            if (entity == null) return null;

            return new AccountDto()
            {
                ID = entity.ID,
                IBAN = entity.IBAN,
                CreatedOn = entity.CreatedOn,
            };
        }

        public static Account ToEntity(AccountDto dto)
        {
            return new Account()
            {
                ID = dto.ID,
                IBAN = dto.IBAN,
                CreatedOn = dto.CreatedOn,
            };
        }
    }
}