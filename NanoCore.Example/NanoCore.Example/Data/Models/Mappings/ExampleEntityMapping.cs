using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nano.Data.Models.Mappings;
using NanoCore.Example.Models;

namespace NanoCore.Example.Data.Models.Mappings
{
    /// <inheritdoc />
    public class ExampleEntityMapping : DefaultEntityMapping<ExampleEntity>
    {
        /// <inheritdoc />
        public override void Map(EntityTypeBuilder<ExampleEntity> builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            base.Map(builder);

            builder
                .Property(x => x.PropertyOne)
                .IsRequired();

            builder
                .HasIndex(x => x.PropertyOne)
                .IsUnique();

            builder
                .Property(x => x.PropertyTwo);

            builder
                .HasOne(x => x.NestedExampleEntity)
                .WithMany();
        }
    }
}