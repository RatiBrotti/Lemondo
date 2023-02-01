using Lemondo.DbClasses;
using Microsoft.EntityFrameworkCore;

namespace Lemondo.Context
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options)
        : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<BookRating> BookRatings { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookAuthor>()
                .HasKey(ba => new { ba.BookId, ba.AuthorId });

            modelBuilder.Entity<BookAuthor>()
                .HasOne(ba => ba.Book)
                .WithMany(b => b.BookAuthors)
                .HasForeignKey(ba => ba.BookId);

            modelBuilder.Entity<BookAuthor>()
                .HasOne(ba => ba.Author)
                .WithMany(a => a.BookAuthors)
                .HasForeignKey(ba => ba.AuthorId);

            modelBuilder.Entity<BookRating>()
                .HasOne(br => br.Book)
                .WithMany(b => b.BookRatings)
                .HasForeignKey(br => br.BookId);

            modelBuilder.Entity<BookRating>()
               .HasOne(br => br.User)
               .WithMany(u => u.BookRatings)
               .HasForeignKey(br => br.UserId);
        }
    }
}

