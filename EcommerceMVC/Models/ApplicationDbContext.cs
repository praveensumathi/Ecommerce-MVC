using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace EcommerceMVC.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, AppRole, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> context) :base(context)
        {
        }

        public virtual DbSet<Product> Product { get { return Set<Product>(); } }
        public virtual DbSet<ApplicationUser> Users { get; set; }

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    base.OnModelCreating(builder);

        //    builder.Entity<ApplicationUser>(b =>
        //    {
        //        b.Property<string>("UserId")
        //            .HasColumnType("character varying");

        //        b.Property<string>("ProductId")
        //            .HasColumnType("character varying");


        //        b.HasOne<ApplicationProduct>()
        //            .WithMany()
        //            .HasForeignKey("ProductId")
        //            .OnDelete(DeleteBehavior.Cascade)
        //            .IsRequired();

        //        b.HasOne<ApplicationUser>()
        //            .WithMany()
        //            .HasForeignKey("UserId")
        //            .OnDelete(DeleteBehavior.Cascade)
        //            .IsRequired();

        //        b.HasKey("UserId", "ProductId");

        //        b.HasIndex("ProductId");

        //        b.ToTable("ApplicationUserProducts");
        //    });
                
        //}
    }
}
