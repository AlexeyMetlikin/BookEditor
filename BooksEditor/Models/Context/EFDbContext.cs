using System.Data.Entity;
using BooksEditor.Models.Entities;

namespace BooksEditor.Models.Context
{
    public class EFDbContext : DbContext
    {
        public EFDbContext()
        {
            Database.SetInitializer(new BookDbInit());
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
    }
}