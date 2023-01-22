using Banking.Infrastructure.Persistence.Entities;
using Banking.Infrastructure.Share.Constants;

namespace Banking.Domain.Service.Dto
{
    public class TransactionDto
    {
        public Guid ID { get; set; }

        public Guid? SourceAccountID { get; set; }
        public Guid? DestinationAccountID { get; set; }
        public Enums.TransactionStatus Status { get; set; }
        public Enums.TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedOn { get; set; }

        public static TransactionDto FromEntity(Transaction entity)
        {
            if (entity == null) return null;

            return new TransactionDto()
            {
                ID = entity.ID,
                SourceAccountID = entity.SourceAccountID,
                DestinationAccountID = entity.DestinationAccountID,
                Status = entity.Status,
                Type = entity.Type,
                Amount = entity.Amount,
                CreatedOn = entity.CreatedOn,
            };
        }
    }
}