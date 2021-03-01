﻿using Microsoft.EntityFrameworkCore;
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

        #region Many to many relation
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
        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>()
                .HasIndex(b => b.UserName)
                .IsUnique();

            base.OnModelCreating(builder);
        }

        public DbSet<Order> Orders { get; set; }

        //ShoppingCartItem
        public DbSet<OrderedMovie> OrderedMovies { get; set; }


    }
}
