﻿using Banking.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Banking.Infrastructure.Persistence
{
    public class BankingContext : DbContext
    {
        public BankingContext(DbContextOptions<BankingContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
    }
}