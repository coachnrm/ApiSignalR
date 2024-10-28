using Microsoft.EntityFrameworkCore;
using CrudSignalR.Models;

namespace CrudSignalR.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        public DbSet<Item> Items { get; set; }
    }
}