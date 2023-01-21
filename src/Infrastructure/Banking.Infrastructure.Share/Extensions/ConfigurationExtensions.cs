using Microsoft.Extensions.Configuration;

namespace Banking.Infrastructure.Share.Extensions
{
    public static class ConfigurationExtensions
    {
        public static string DbConnectionString(this IConfiguration configuration)
        {
            return configuration.GetConnectionString("BankingDatabase");
        }
    }
}