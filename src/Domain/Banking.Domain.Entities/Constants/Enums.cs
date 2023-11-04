namespace Banking.Domain.Entities.Constants
{
    public static class Enums
    {
        public enum TransactionStatus
        {
            Failed = 1,
            Success = 2,
        }

        public enum TransactionType
        {
            Deposit = 1,
            Withdrawal = 2,
            Transfer = 3,
        }
    }
}