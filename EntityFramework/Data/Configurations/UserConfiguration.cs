using EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace EntityFramework.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
              .Property(e => e.Id)
              .ValueGeneratedOnAdd()
              .HasDefaultValueSql("NEWID()");

            builder
                .Property(x => x.Nickname)
                .IsRequired()
                .HasMaxLength(25);

            builder
                 .Property(u => u.Email)
                 .IsRequired()
                 .HasMaxLength(50)
                 .HasAnnotation("RegularExpression", @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$")
                 .HasAnnotation("RegularExpressionErrorMessage", "Invalid Email Address");
        }
    }
}
