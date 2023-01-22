using Banking.Application.API.Middlewares;
using Banking.Domain.Service.AccountLogic;
using Banking.Domain.Service.TransactionLogic;
using Banking.Infrastructure.Persistence;
using Banking.Infrastructure.Persistence.Repository.EFCore;
using Banking.Infrastructure.Persistence.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Banking.Application.API
{
    public static class Boostraper
    {
        public static void RegisterServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<ITransactionService, TransactionService>();

            builder.Services.AddScoped<IIBAN_Service, IBAN_Service>();
        }

        public static void RegisterPersistence(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContextSql<BankingContext>(builder.Configuration);
            builder.Services.AddScoped(typeof(IRepositoryEF<,>), typeof(RepositoryEF<,>));
            builder.Services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        }

        public static void RegisterMiddlewares(this WebApplication app)
        {
            app.UseHttpsRedirection();

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseAuthorization();
        }

        public static void InitDatabase(this WebApplication app)
        {
            using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<BankingContext>();
                dbContext.Database.Migrate();
            }
        }
    }
}