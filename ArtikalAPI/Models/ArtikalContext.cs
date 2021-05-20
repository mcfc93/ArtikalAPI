using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using ArtikalAPI.Models;

namespace ArtikalAPI.Models
{
    public class ArtikalContext : DbContext
    {
        public DbSet<Korisnik> Korisnici { get; set; }
        public DbSet<Artikal> Artikli { get; set; }
        public DbSet<Atribut> Atributi { get; set; }
        public DbSet<Vrsta> Vrste { get; set; }

        public ArtikalContext(DbContextOptions<ArtikalContext> options) : base(options)
        {

        }
        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(ConfigurationManager.ConnectionStrings["Artikal"].ConnectionString);
        }
        */
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Korisnik>().HasIndex(korisnik => korisnik.Email).IsUnique();
        }
    }
}
