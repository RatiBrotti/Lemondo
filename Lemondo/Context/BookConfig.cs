using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Lemondo.DbClasses;

namespace Lemondo.Context
{
    public class BookConfig : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Book");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Title)
                .HasMaxLength(50);

            builder.Property(e => e.Description)
                .HasMaxLength(200);

            builder.Property(e => e.Image)
                .HasMaxLength(50);

        }
    }
}
