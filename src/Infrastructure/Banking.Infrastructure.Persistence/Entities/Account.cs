using System.ComponentModel.DataAnnotations;

namespace Banking.Infrastructure.Persistence.Entities
{
    public class Account
    {
        [Key]
        public Guid ID { get; set; }

        [Required]
        public string IBAN { get; set; }

        public decimal Balance { get; set; }
        public DateTime CreatedOn { get; set; }

        public ICollection<Transaction>? TransactionsOut { get; set; }
        public ICollection<Transaction>? TransactionsIn { get; set; }
    }
}