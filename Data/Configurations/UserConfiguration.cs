using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.PasswordHash)
                .IsRequired();

            builder.Property(u => u.PasswordSalt)
             .IsRequired();

            builder.Property(u => u.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(u => u.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            // Índices únicos
            builder.HasIndex(u => u.Username)
                .IsUnique();

            builder.HasIndex(u => u.Email)
                .IsUnique();
        }
    
    }
}
