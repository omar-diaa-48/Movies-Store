using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TMDbLib.Objects.Movies;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Demo.Models
{
    public class MovieStoreDBContext : IdentityDbContext<ApplicationUser>
    {
        public MovieStoreDBContext(DbContextOptions<MovieStoreDBContext> options)
            :base(options)
        {

        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<MovieOrder>()
        //        .HasKey(mo => new { mo.MovieID, mo.OrderID });

        //    modelBuilder.Entity<MovieOrder>()
        //        .HasOne(x => x.Movie)
        //        .WithMany(y => y.MovieOrder)
        //        .HasForeignKey(y => y.MovieID);

        //    modelBuilder.Entity<MovieOrder>()
        //        .HasOne(x => x.Order)
        //        .WithMany(y => y.MovieOrder)
        //        .HasForeignKey(y => y.OrderID);

        //    modelBuilder.Entity<Movie>()
        //    .HasMany<Order>(m => m.Orders)
        //    .WithMany(o => o.Movies)
        //    .Map(cs =>
        //    {
        //        cs.MapLeftKey("MovieRefId");
        //        cs.MapRightKey("OrderRefId");
        //        cs.ToTable("MovieOrder");
        //    });
        //    base.OnModelCreating(modelBuilder);
        //}

        public DbSet<Order> Orders { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<OrderedMovie> OrderedMovies { get; set; }

    }
}
