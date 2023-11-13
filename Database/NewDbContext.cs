using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleTraderAssignment.Models;

namespace TeleTraderAssignment.Database
{
    public class NewDbContext : DbContext
    {
        private string _databasePath;
        public DbSet<Symbol> Symbol { get; set; }
        public DbSet<Models.Type> Type { get; set; }
        public DbSet<Exchange> Exchange { get; set; }

        public NewDbContext(string databasePath)
        {
            _databasePath = databasePath;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={_databasePath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Exchange>().HasKey(e => e.Id);
            modelBuilder.Entity<Models.Type>().HasKey(e => e.Id);

            modelBuilder.Entity<Symbol>()
                .HasOne(s => s.Exchange)
                .WithMany()
                .HasForeignKey(s => s.ExchangeId);

            modelBuilder.Entity<Symbol>()
                .HasOne(s => s.Type)
                .WithMany()
                .HasForeignKey(s => s.TypeId);
        }
    }
}
