using System.ComponentModel.DataAnnotations;

namespace Banking.Infrastructure.Persistence.Entities
{
    public class Account
    {
        [Key]
        public Guid ID { get; set; }

        public string IBAN { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}