namespace Banking.Application.API.Dto
{
    public class DepositRequest
    {
        public Guid AccountId { get; set; }
        public decimal Amount { get; set; }
    }
}