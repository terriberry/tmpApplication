using System;
using CoreApplicationFilterVal_10.Domain.Models.Entities.Owners;
using Intent.RoslynWeaver.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.EntityFrameworkCore.EntityTypeConfiguration", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Persistence.Persistence.Configurations
{
    public class OwnerConfiguration : IEntityTypeConfiguration<Owner>
    {
        public void Configure(EntityTypeBuilder<Owner> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired();

            builder.Property(x => x.Age)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql()
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(x => x.LastModifiedAt)
                .HasDefaultValueSql()
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(x => x.DeletedAt);

            builder.HasQueryFilter(x => x.DeletedAt == null);

        }
    }
}