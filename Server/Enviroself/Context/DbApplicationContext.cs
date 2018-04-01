using Enviroself.Features.Account.Entities;
using Enviroself.Features.Document.Entities;
using Enviroself.Models;
using Microsoft.EntityFrameworkCore;

namespace Enviroself.Context
{
    public class DbApplicationContext : DbContext
    {
        public DbApplicationContext(DbContextOptions<DbApplicationContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<UserDocumentEdit> UserDocumentEdits { get; set; }
        public DbSet<UserDocumentView> UserDocumentViews { get; set; }
    }
}
