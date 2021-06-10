using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MIU.Core.ValueObjects;
using MIU.Movimentations.Domain.Entities;

namespace MIU.Movimentations.Infra.Mappings
{
    public class MovimentationMapping : IEntityTypeConfiguration<Movimentation>
    {
        public void Configure(EntityTypeBuilder<Movimentation> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.CustomerName)
                .IsRequired()
                .HasColumnType("VARCHAR(500)");

            builder.Property(x => x.TributeCode)
                .IsRequired()
                .HasColumnType("VARCHAR(1000)");

            builder.Property(x => x.TributeDescription)
               .HasColumnType("VARCHAR(1000)");

            builder.Property(x => x.MovimentationDate)
               .IsRequired()
               .HasColumnType("DATETIME");

            builder.Property(x => x.TributeAliquot)
               .IsRequired()
               .HasColumnType("INT");

            builder.Property(x => x.MovimentationGain)
               .IsRequired()
               .HasColumnType("DECIMAL");

            builder.Property(x => x.MovimentationLoss)
               .IsRequired()
               .HasColumnType("DECIMAL");

            builder.OwnsOne(x => x.Cpf, tf =>
            {
                tf.Property(x => x.Number)
                    .IsRequired()
                    .HasMaxLength(CPF.CpfMaxLength)
                    .HasColumnName("Cpf")
                    .HasColumnType($"VARCHAR(${CPF.CpfMaxLength})");
            });

            builder.ToTable("Movimentation");
        }
    }
}
