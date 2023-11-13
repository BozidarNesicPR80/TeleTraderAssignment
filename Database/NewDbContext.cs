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

        //prilikom kreiranja konteksta prosledjuje mu se fajl koji predstavlja bazu, time je moguce ukljuciti vise razlicitih baza
        //u projekat
        public NewDbContext(string databasePath)
        {
            _databasePath = databasePath;
        }

        //Od korisnika se trazi ucitavanje baze, pa ucitani fajl prosledjujemo metodi kako bi se baza mogla koristiti
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={_databasePath}");
        }

        //modelovanje veza 1 na vise, gde 1 type ili exchange mogu da se odnose na vise symbol-a
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
