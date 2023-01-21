namespace Banking.Infrastructure.Persistence.Entities
{
    public class Account
    {
        public Guid ID { get; set; }
        public string IBAN { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}