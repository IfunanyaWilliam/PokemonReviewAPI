using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PokemonReviewAPI.Models;
using System;

namespace PokemonReviewAPI.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Country> Countries { get; set; }   
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Pokemon> Pokemons { get; set; }
        public DbSet<PokemonCategory> PokemonCategories { get; set; }
        public DbSet<PokemonOwner> PokemonOwners { get; set; }
        public DbSet<Review> Reviews { get; set; }  
        public DbSet<Reviewer> Reviewers { get; set; }

        public DbSet<AppUser> AppUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PokemonCategory>()
                        .HasKey(pc => new { pc.PokemonId, pc.CategoryId });
            modelBuilder.Entity<PokemonCategory>()
                        .HasOne(p => p.Pokemon)
                        .WithMany(pc => pc.PokemonCategories)
                        .HasForeignKey(p => p.PokemonId);
            modelBuilder.Entity<PokemonCategory>()
                        .HasOne(c => c.Category)
                        .WithMany(pc => pc.PokemonCategories)
                        .HasForeignKey(c => c.CategoryId);

            modelBuilder.Entity<PokemonOwner>()
                        .HasKey(po => new { po.PokemonId, po.OwnerId });
            modelBuilder.Entity<PokemonOwner>()
                        .HasOne(p => p.Pokemon)
                        .WithMany(po => po.PokemonOwners)
                        .HasForeignKey(p => p.PokemonId);
            modelBuilder.Entity<PokemonOwner>()
                        .HasOne(c => c.Owner)
                        .WithMany(po => po.PokemonOwners)
                        .HasForeignKey(o => o.OwnerId);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder
        //    .UseSqlServer(
        //            "Server=IFUNANYA-ONAH; Database=PokemonReviewAPI; Trusted_Connection=True;MultipleActiveResultSets=True",
        //            options => options.EnableRetryOnFailure());
        //}
    }
}
