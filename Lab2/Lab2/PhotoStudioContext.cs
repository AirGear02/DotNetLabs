using System;
using System.Collections.Generic;
using System.Text;
using Lab2.Models;
using System.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Lab2
{
    class PhotoStudioContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<Order> Orders { get; set; }
        
        public PhotoStudioContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["photoStudio"].ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasOne(order => order.Client)
                .WithMany(o => o.Orders)
                .HasForeignKey(order => order.ClientId);

            modelBuilder.Entity<Order>()
                .HasOne(order => order.Option)
                .WithMany(o => o.Orders)
                .HasForeignKey(order => order.OptionId);
        }



    }
}
