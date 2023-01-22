using Banking.Domain.Service.AccountLogic;
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

        builder.Services.AddDbContextSql<BankingContext>(builder.Configuration);

        builder.Services.AddScoped<IAccountService, AccountService>();

        builder.Services.AddScoped<DbContext, BankingContext>();
        builder.Services.AddScoped(typeof(IRepositoryEF<>), typeof(RepositoryEF<>));
        builder.Services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
        {
            var dbContext = serviceScope.ServiceProvider.GetRequiredService<BankingContext>();
            dbContext.Database.Migrate();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}