using ComputersApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ComputersApp.Infrastructure.Configurations
{
    class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Email)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(x => x.Password)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(x => x.Role)
                .HasMaxLength(50)
                .HasDefaultValue("User");
        }
    }
}
