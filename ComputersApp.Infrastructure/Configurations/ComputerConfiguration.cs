using ComputersApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ComputersApp.Infrastructure.Configurations
{
    class ComputerConfiguration : IEntityTypeConfiguration<Computer>
    {
        public void Configure(EntityTypeBuilder<Computer> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.OsType)
                .HasConversion(x => x.ToString(),
                    x => (OsType)Enum.Parse(typeof(OsType), x))
                .HasDefaultValue(OsType.Linux);
            builder.Property(x => x.Name)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(x => x.RamAmout)
                .IsRequired();
            builder.Property(x => x.SsdAmount)
                .IsRequired();
            builder.HasOne(x => x.Cpu)
                .WithMany(x => x.Computers);
        }
    }
}
