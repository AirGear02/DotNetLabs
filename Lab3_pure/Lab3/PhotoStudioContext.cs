using Lab3.Models;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace Lab3
{
    public class PhotoStudioContext : DbContext
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
          
            optionsBuilder.UseSqlServer("data source=DESKTOP-TJTB3QL; initial catalog=university;persist security info=True; Integrated Security=SSPI;MultipleActiveResultSets=true");
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
