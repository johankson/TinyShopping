using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TinyShopping.Api.Data.Models;

namespace TinyShopping.Api.Data
{
    public class ShoppingDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<ShoppingList> Lists { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Store> Stores { get; set; }

        public ShoppingDbContext(DbContextOptions<ShoppingDbContext> options) : base(options)
        {
            
        }

        public IEnumerable<Item> GetListItems(int id)
        {
            return Items.Where(d => d.ShoppingListId == id);
        }
    }
}
