using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(r => r.Description)
                .HasMaxLength(200);

            builder.Property(r => r.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            // Índice único para el nombre del rol
            builder.HasIndex(r => r.Name)
                .IsUnique();
        }
    }
}
