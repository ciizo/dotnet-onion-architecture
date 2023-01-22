using Banking.Application.API.Middlewares;
using Banking.Domain.Service.AccountLogic;
using Banking.Domain.Service.TransactionLogic;
using Banking.Infrastructure.Persistence;
using Banking.Infrastructure.Persistence.Repository.EFCore;
using Banking.Infrastructure.Persistence.UnitOfWork;
using Microsoft.EntityFrameworkCore;

internal class Startup
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddHttpClient();

        RegisterServices(builder);
        RegisterPersistence(builder);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        InitDatabase(app);
        RegisterMiddlewares(app);

        app.MapControllers();

        app.Run();
    }

    private static void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IAccountService, AccountService>();
        builder.Services.AddScoped<ITransactionService, TransactionService>();

        builder.Services.AddScoped<IIBAN_Service, IBAN_Service>();
    }

    private static void RegisterPersistence(WebApplicationBuilder builder)
    {
        builder.Services.AddDbContextSql<BankingContext>(builder.Configuration);
        builder.Services.AddScoped(typeof(IRepositoryEF<,>), typeof(RepositoryEF<,>));
        builder.Services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
    }

    private static void RegisterMiddlewares(WebApplication app)
    {
        app.UseHttpsRedirection();

        app.UseMiddleware<ExceptionMiddleware>();
        app.UseAuthorization();
    }

    private static void InitDatabase(WebApplication app)
    {
        using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
        {
            var dbContext = serviceScope.ServiceProvider.GetRequiredService<BankingContext>();
            dbContext.Database.Migrate();
        }
    }
}