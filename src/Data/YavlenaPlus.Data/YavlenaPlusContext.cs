using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YavlenaPlus.Data.Models;
using YavlenaPlus;


namespace YavlenaPlus.Data
{
    public class YavlenaPlusContext : IdentityDbContext<YavlenaPlusUser>
    {
        public YavlenaPlusContext(DbContextOptions<YavlenaPlusContext> options)
            : base(options)
        {
        }

        public DbSet<Offer> Offers { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Comment> Comments { get; set; }

        //public DbSet<YavlenaPlusUser> Users { get; set; }

        // public DbSet<UsersOffers> UsersOffers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // builder.Entity<YavlenaPlusUser>().HasMany<Offer>().WithOne();
            // builder.Entity<UsersOffers>().HasKey(c => new { c.UserId, c.OfferId});
            //builder.Entity<YavlenaPlusUser>().HasOne(x => x.Offer).WithMany(x => x.Users);
            //builder.Entity<YavlenaPlusUser>().HasMany<Offer>().WithOne(x=>x.)

            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
