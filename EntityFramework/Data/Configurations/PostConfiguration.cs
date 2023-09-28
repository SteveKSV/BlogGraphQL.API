using EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFramework.Data.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder
               .HasKey(x => x.Id);

            builder
              .Property(e => e.Id)
              .ValueGeneratedOnAdd()
              .HasDefaultValueSql("NEWID()");

            builder
               .Property(x => x.Title)
               .IsRequired()
               .HasMaxLength(100);

            builder
               .Property(x => x.Content)
               .IsRequired()
               .HasMaxLength(250);

            builder
                .HasOne(p => p.Author)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
