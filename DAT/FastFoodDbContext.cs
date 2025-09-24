using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAT.Entity;
using Microsoft.EntityFrameworkCore;

namespace DAT
{
    public class FastFoodDbContext : DbContext
    {
        public FastFoodDbContext(DbContextOptions<FastFoodDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ComboDetail> ComboDetails { get; set; }
        public DbSet<ComboOptionGroup> ComboOptionGroups { get; set; }
        public DbSet<ComboOptionItem> ComboOptionItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.ComboDetails)
                .WithOne(cd => cd.ComboProduct)
                .HasForeignKey(cd => cd.ComboProductId);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.ComboOptionGroups)
                .WithOne(cog => cog.ComboProduct)
                .HasForeignKey(cog => cog.ComboProductId);

            modelBuilder.Entity<ComboOptionGroup>()
                .HasMany(cog => cog.ComboOptionItems)
                .WithOne(coi => coi.OptionGroup)
                .HasForeignKey(coi => coi.OptionGroupId);
        }
    }
}
