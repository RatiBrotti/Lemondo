using Lemondo.DbClasses;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Lemondo.Context
{
    public partial class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        //public DbSet<BookAuthor> BookAuthors { get; set; }
        //public DbSet<BookRating> BookRatings { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            modelBuilder.HasSequence("ContactIDSequence")
                .StartsAt(0)
                .IncrementsBy(10);

            modelBuilder.HasSequence("D").StartsAt(0);

            modelBuilder.HasSequence("val");

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

