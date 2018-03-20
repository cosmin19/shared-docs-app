using ASP.NET_Core_Basic_Web_API.Features.Account.Entities;
using ASP.NET_Core_Basic_Web_API.Models;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_Core_Basic_Web_API.Context
{
    public class DbApplicationContext : DbContext
    {
        public DbApplicationContext(DbContextOptions<DbApplicationContext> options)
            : base(options)
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
