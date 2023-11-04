using Banking.Infrastructure.Share.Constants;

namespace Banking.Domain.Service.TransactionLogic
{
    public static class FeeServiceHelper
    {
        public static decimal ApplyFee(Enums.TransactionType type, decimal amount)
        {
            var fee = GetFee(type);

            return amount - (amount * (decimal)fee / 100);
        }

        public static double GetFee(Enums.TransactionType type)
        {
            return type switch
            {
                //TODO move value to database and apply cache
                Enums.TransactionType.Deposit => 0.1,
                Enums.TransactionType.Withdrawal => 0,
                Enums.TransactionType.Transfer => 0,
                _ => throw new ArgumentOutOfRangeException(nameof(type), $"Not expected TransactionType value: {type}"),
            };
        }
    }
}