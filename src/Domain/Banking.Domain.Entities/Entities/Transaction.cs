using Banking.Infrastructure.Share.Constants;
using System.ComponentModel.DataAnnotations;

namespace Banking.Domain.Entities
{
    public class Transaction
    {
        [Key]
        public Guid ID { get; set; }

        public Guid? SourceAccountID { get; set; }
        public Guid? DestinationAccountID { get; set; }
        public Enums.TransactionStatus Status { get; set; }
        public Enums.TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedOn { get; set; }

        public Account? SourceAccount { get; set; }
        public Account? DestinationAccount { get; set; }
    }
}