using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ComputersApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ComputersApp.Infrastructure.Configurations
{
    class CpuConfiguration : IEntityTypeConfiguration<Cpu>
    {
        public void Configure(EntityTypeBuilder<Cpu> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(x => x.CorsAmount)
                .IsRequired();
            builder.Property(x => x.Frequency)
                .IsRequired();
            builder.HasMany(x => x.Computers)
                .WithOne(x => x.Cpu);
        }
    }
}
