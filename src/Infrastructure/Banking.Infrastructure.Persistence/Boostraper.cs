using Banking.Infrastructure.Share.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Banking.Infrastructure.Persistence
{
    public static class Boostraper
    {
        public static IServiceCollection AddDbContextSql<T>(this IServiceCollection services, IConfiguration configuration) where T : DbContext
        {
            services.AddDbContext<T>(options => options.UseSqlServer(configuration.DbConnectionString(), sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()));
            return services;
        }
    }
}