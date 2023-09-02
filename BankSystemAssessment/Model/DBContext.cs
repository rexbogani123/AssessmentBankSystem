using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using BankSystemAssessment.Model;
using Microsoft.OpenApi.Models;

namespace BankSystemAssessment.Model
{
    public class DBContext : DbContext
    {

        public DBContext(DbContextOptions option) : base(option)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .Property(p => p.Balance)
                .HasColumnType("decimal(18,4)");

        }
    }
}

