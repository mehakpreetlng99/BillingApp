
using BillingApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BillingApp.Data
{
    public class BillingDbContext : IdentityDbContext<User>
    {
        public BillingDbContext(DbContextOptions<BillingDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
        public DbSet<Customer> Customers { get; set; }




        // Removing role seeding logic here and relying on DbSeeder.cs for seeding roles
        // If you want to seed roles, handle it inside DbSeeder.cs manually

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // No need to seed roles here anymore
            // Roles are handled manually in DbSeeder.cs
        }
    }
}

//using BillingApp.Models;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Identity.Client;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection.Emit;
//using System.Text;
//using System.Threading.Tasks;

//namespace BillingApp.Data
//{
//    public class BillingDbContext:IdentityDbContext<User>
//    {
//        public BillingDbContext(DbContextOptions<BillingDbContext> options) : base(options)
//        {
//        }

//        public DbSet<Category> Categories { get; set; }
//        protected override void OnModelCreating(ModelBuilder builder)
//        {
//            base.OnModelCreating(builder);


//            builder.Entity<IdentityRole>().HasData(
//                new IdentityRole { Name = "SuperAdmin", NormalizedName = "SUPERADMIN" },
//                new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
//                new IdentityRole { Name = "Agent", NormalizedName = "AGENT" }
//            );

//        }

//    }
//}
