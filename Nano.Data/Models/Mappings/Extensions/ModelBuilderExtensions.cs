using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Nano.Models.Extensions;
using Nano.Models.Interfaces;

namespace Nano.Data.Models.Mappings.Extensions
{
    /// <summary>
    /// Model Builder Extensions.
    /// </summary>
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Adds a mapping for the type <typeparamref name="TEntity"/> using the <typeparamref name="TMapping"/> implementation.
        /// </summary>
        /// <typeparam name="TEntity">The <see cref="IEntity"/>.</typeparam>
        /// <typeparam name="TMapping">The <see cref="BaseEntityMapping{TEntity}"/>.</typeparam>
        /// <param name="builder">The <see cref="ModelBuilder"/>.</param>
        /// <returns>The <see cref="ModelBuilder"/>.</returns>
        public static ModelBuilder AddMapping<TEntity, TMapping>(this ModelBuilder builder)
            where TEntity : class, IEntity
            where TMapping : BaseEntityMapping<TEntity>, new()
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var mapping = new TMapping();
            var entity = builder.Entity<TEntity>();

            // TODO: TEST: Soft-delete unique indexes (include "IsDeleted")
            //if (typeof(TEntity).IsTypeDef<IEntityDeletableSoft>())
            //{
            //    entity.Metadata.
            //        GetIndexes()
            //        .Where(x => x.IsUnique)
            //        .ToList()
            //        .ForEach(x =>
            //        {
            //            entity.Metadata.RemoveIndex(x.Properties);
            //            entity.HasIndex(x.Properties.Select(y => y.Name).Union(new[] { "IsDeletetd" }).ToArray());
            //        });
            //}

            mapping
                .Map(builder.Entity<TEntity>());

            return builder;
        }
    }
}