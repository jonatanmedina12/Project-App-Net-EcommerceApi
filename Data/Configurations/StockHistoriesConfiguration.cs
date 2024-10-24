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
    public class StockHistoriesConfiguration : IEntityTypeConfiguration<StockHistory>
    {
        public void Configure(EntityTypeBuilder<StockHistory> builder)
        {
            builder.ToTable("StockHistory");

            // Clave primaria
            builder.HasKey(u => u.Id);

            // Relación con Product
            builder.HasOne(p => p.Product)
                .WithMany(u => u.StockHistories)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación con User
            builder.HasOne(p => p.ModifiedByUser)
                .WithMany()
                .HasForeignKey(p => p.ModifiedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuración de propiedades
            builder.Property(u => u.Quantity)
                .IsRequired();

            builder.Property(u => u.OperationType)
                .IsRequired();

            builder.Property(u => u.Reason)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(u => u.PreviousStock)
                .IsRequired();

            builder.Property(u => u.NewStock)
                .IsRequired();

            builder.Property(u => u.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            // Índices
            builder.HasIndex(u => u.ProductId);
            builder.HasIndex(u => u.ModifiedByUserId);
            builder.HasIndex(u => u.CreatedAt);





        }


    }
}
