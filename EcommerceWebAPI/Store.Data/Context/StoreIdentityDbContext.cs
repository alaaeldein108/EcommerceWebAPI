using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Store.Data.Entities.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data.Context
{
    public class StoreIdentityDbContext : IdentityDbContext<AppUser>
    {
        public StoreIdentityDbContext(DbContextOptions<StoreIdentityDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AppUser>()
           .HasOne(a => a.Address)
           .WithOne(b => b.AppUser)
           .HasForeignKey<Address>(b => b.Id);
            builder.Entity<IdentityUserLogin<string>>(b =>
            {
                b.HasKey(l => l.UserId); // Define primary key for IdentityUserLogin
            });
            builder.Entity<IdentityUserRole<string>>(b =>
            {
                b.HasKey(ur => new { ur.UserId, ur.RoleId }); // Define primary key for IdentityUserRole
            });
            builder.Entity<IdentityUserToken<string>>(b =>
            {
                b.HasKey(t => new { t.UserId, t.LoginProvider, t.Name }); // Define primary key for IdentityUserToken
            });
        }
    }
}
