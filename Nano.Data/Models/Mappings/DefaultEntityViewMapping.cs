using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nano.Models;

namespace Nano.Data.Models.Mappings
{
    /// <inheritdoc />
    public abstract class DefaultEntityViewMapping<TEntity> : BaseEntityViewMapping<TEntity>
        where TEntity : DefaultEntity
    {
        /// <inheritdoc />
        public override void Map(EntityTypeBuilder<TEntity> builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            base.Map(builder);
        }
    }
}