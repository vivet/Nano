using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DynamicExpression.Interfaces;
using Microsoft.EntityFrameworkCore;
using Nano.Models.Interfaces;

namespace Nano.Repository.Interfaces
{ 
    /// <summary>
    /// (Base) Interface for a repository.
    /// Defines methods for the most rudamentory operations (get, criteria2, add, update, delete) on instances of <see cref="IEntity"/>.
    /// <see cref="IEntityCreatable"/>, <see cref="IEntityUpdatable"/> and <see cref="IEntityDeletable"/>). 
    /// </summary>
    public interface IRepository : IDisposable
    {
        /// <summary>
        /// Is Lazy Loading Enabled.
        /// </summary>
        bool IsLazyLoadingEnabled { get; set; }

        /// <summary>
        /// Gets the <see cref="DbSet{TEntity}"/> mathcing the type of <typeparamref name="TEntity"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type</typeparam>
        /// <returns>The <see cref="DbSet{TEntity}"/>.</returns>
        DbSet<TEntity> GetEntitySet<TEntity>()
           where TEntity : class, IEntity;

        /// <summary>
        /// Gets an instance of type <typeparamref name="TEntity"/>, matching the passed <paramref name="key"/> of the <see cref="IEntity"/>
        /// </summary>
        /// <typeparam name="TEntity">The <see cref="IEntity"/> type.</typeparam>
        /// <typeparam name="TIdentity">The identity type.</typeparam>
        /// <param name="key">The <typeparamref name="TIdentity"/> type, uniquely identify the <see cref="IEntity"/>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> (optional).</param>
        /// <returns>The first instance, matching the passed <paramref name="key"/>.</returns>
        Task<TEntity> GetAsync<TEntity, TIdentity>(TIdentity key, CancellationToken cancellationToken = default)
            where TEntity : class, IEntityIdentity<TIdentity>;

        /// <summary>
        /// Gets all instances of type <typeparamref name="TEntity"/>, matching the passed <paramref name="keys"/>.
        /// </summary>
        /// <typeparam name="TEntity">The <see cref="IEntity"/> type.</typeparam>
        /// <typeparam name="TIdentity">The identity type.</typeparam>
        /// <param name="keys">The <typeparamref name="TIdentity"/> type, uniquely identifying the <see cref="IEntity"/>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> (optional).</param>
        /// <returns>The instances, matching the passed <paramref name="keys"/>.</returns>
        Task<IEnumerable<TEntity>> GetManyAsync<TEntity, TIdentity>(IEnumerable<TIdentity> keys, CancellationToken cancellationToken = default)
            where TEntity : class, IEntityIdentity<TIdentity>;

        /// <summary>
        /// Gets all instances of type <typeparamref name="TEntity"/>, matching the passed <paramref name="query"/>.
        /// </summary>
        /// <typeparam name="TEntity">The <see cref="IEntity"/> type.</typeparam>
        /// <param name="query">The <see cref="IQuery"/>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> (optional).</param>
        /// <returns>The instances, matching the passed <paramref name="query"/>.</returns>
        Task<IEnumerable<TEntity>> GetManyAsync<TEntity>(IQuery query, CancellationToken cancellationToken = default)
            where TEntity : class, IEntity;

        /// <summary>
        /// Gets all instances of type <typeparamref name="TEntity"/>, matching the passed <paramref name="query"/>.
        /// </summary>
        /// <typeparam name="TEntity">The <see cref="IEntity"/> type.</typeparam>
        /// <typeparam name="TCriteria">The <see cref="IQueryCriteria"/>.</typeparam>
        /// <param name="query">The <see cref="IQuery{TCriteria}"/>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> (optional).</param>
        /// <returns>The instances, matching the passed <paramref name="query"/>.</returns>
        Task<IEnumerable<TEntity>> GetManyAsync<TEntity, TCriteria>(IQuery<TCriteria> query, CancellationToken cancellationToken = default)
            where TEntity : class, IEntity
            where TCriteria : class, IQueryCriteria, new();

        /// <summary>
        /// Gets all instances of type <typeparamref name="TEntity"/>, matching the passed <paramref name="where"/> clause.
        /// </summary>
        /// <typeparam name="TEntity">The <see cref="IEntity"/> type.</typeparam>
        /// <param name="where">The where clause</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> (optional).</param>
        /// <returns>The instances, matching the passed parameters.</returns>
        Task<IEnumerable<TEntity>> GetManyAsync<TEntity>(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default)
            where TEntity : class, IEntity;

        /// <summary>
        /// Gets all instances of type <typeparamref name="TEntity"/>, matching the passed <paramref name="where"/> clause.
        /// </summary>
        /// <typeparam name="TEntity">The <see cref="IEntity"/> type.</typeparam>
        /// <param name="where">The where clause</param>
        /// <param name="pagination">The <see cref="IPagination"/>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> (optional).</param>
        /// <returns>The instances, matching the passed parameters.</returns>
        Task<IEnumerable<TEntity>> GetManyAsync<TEntity>(Expression<Func<TEntity, bool>> where, IPagination pagination, CancellationToken cancellationToken = default)
            where TEntity : class, IEntity;

        /// <summary>
        /// Gets all instances of type <typeparamref name="TEntity"/>, matching the passed <paramref name="where"/> clause, ordering by the passed <paramref name="ordering"/>
        /// </summary>
        /// <typeparam name="TEntity">The <see cref="IEntity"/> type.</typeparam>
        /// <param name="where">The where clause</param>
        /// <param name="ordering">The order by clause</param>
        /// <param name="pagination">The <see cref="IPagination"/>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> (optional).</param>
        /// <returns>The instances, matching the passed parameters.</returns>
        Task<IEnumerable<TEntity>> GetManyAsync<TEntity>(Expression<Func<TEntity, bool>> where, IPagination pagination, IOrdering ordering, CancellationToken cancellationToken = default)
            where TEntity : class, IEntity;

        /// <summary>
        /// Adds the instance of the passed <see cref="IEntityCreatable"/>.
        /// </summary>
        /// <typeparam name="TEntity">The <see cref="IEntityCreatable"/> type.</typeparam>
        /// <param name="entity">The instance of <see cref="IEntityCreatable"/>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> (optional).</param>
        /// <returns>The <see cref="Task{TEntity}"/>.</returns>
        Task<TEntity> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
            where TEntity : class, IEntityCreatable;

        /// <summary>
        /// Adds all instances of the passed <see cref="IEntityCreatable"/>'s.
        /// </summary>
        /// <typeparam name="TEntity">The <see cref="IEntityCreatable"/> type.</typeparam>
        /// <param name="entities">The instances of <see cref="IEntityCreatable"/>'s.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> (optional).</param>
        /// <returns>The <see cref="Task"/> (void).</returns>
        Task AddManyAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
            where TEntity : class, IEntityCreatable;

        /// <summary>
        /// Updates the instance of the passed <see cref="IEntityUpdatable"/>.
        /// </summary>
        /// <typeparam name="TEntity">The <see cref="IEntityUpdatable"/> type.</typeparam>
        /// <param name="entity">The instance of <see cref="IEntityUpdatable"/>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> (optional).</param>
        /// <returns>The <see cref="Task{TEntity}"/>.</returns>
        Task<TEntity> UpdateAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
            where TEntity : class, IEntityUpdatable;

        /// <summary>
        /// Updates all instances of the passed <see cref="IEntityUpdatable"/>'s.
        /// </summary>
        /// <typeparam name="TEntity">The <see cref="IEntityUpdatable"/> type.</typeparam>
        /// <param name="entities">The instances of <see cref="IEntityUpdatable"/>'s.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> (optional).</param>
        /// <returns>The <see cref="Task"/> (void).</returns>
        Task UpdateManyAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
            where TEntity : class, IEntityUpdatable;

        /// <summary>
        /// Updates all instances of <typeparamref name="TEntity"/> with value from <paramref name="update"/>, 
        /// matching the criteria of the passed <paramref name="select"/>.
        /// </summary>
        /// <typeparam name="TEntity">The <see cref="IEntityUpdatable"/> type.</typeparam>
        /// <typeparam name="TCriteria">The <see cref="IQueryCriteria"/> type.</typeparam>
        /// <param name="select">The <see cref="IQueryCriteria"/>.</param>
        /// <param name="update">The <see cref="IEntityUpdatable"/>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> (optional).</param>
        /// <returns>A <see cref="Task"/> (void).</returns>
        Task UpdateManyAsync<TEntity, TCriteria>(TCriteria select, TEntity update, CancellationToken cancellationToken = default)
            where TEntity : class, IEntityUpdatable
            where TCriteria : class, IQueryCriteria, new();

        /// <summary>
        /// Updates all instances matching the passed select <see cref="Expression"/>, 
        /// and updating all values on the update <see cref="Expression"/>.
        /// </summary>
        /// <typeparam name="TEntity">The <see cref="IEntityUpdatable"/> type.</typeparam>
        /// <param name="select">The <see cref="Expression"/> to evaulate entities to be updated.</param>
        /// <param name="update">The <see cref="Expression"/> determining which values on the selected entities to update.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> (optional).</param>
        /// <returns>The <see cref="Task"/> (void).</returns>
        Task UpdateManyAsync<TEntity>(Expression<Func<TEntity, bool>> select, Expression<Func<TEntity, TEntity>> update, CancellationToken cancellationToken = default)
            where TEntity : class, IEntityUpdatable;

        /// <summary>
        /// Adds or Updates the instance of the passed <see cref="IEntityCreatableAndUpdatable"/>.
        /// </summary>
        /// <typeparam name="TEntity">The <see cref="IEntityCreatableAndUpdatable"/> type.</typeparam>
        /// <param name="entity">The instance of <see cref="IEntityCreatableAndUpdatable"/>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> (optional).</param>
        /// <returns>The <see cref="Task{TEntity}"/>.</returns>
        Task<TEntity> AddOrUpdateAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
            where TEntity : class, IEntityCreatableAndUpdatable;

        /// <summary>
        /// Adds or Updates the instances of the passed <see cref="IEntityCreatableAndUpdatable"/>.
        /// </summary>
        /// <typeparam name="TEntity">The <see cref="IEntityCreatableAndUpdatable"/> type.</typeparam>
        /// <param name="entities">The instances of <see cref="IEntityCreatableAndUpdatable"/>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> (optional).</param>
        /// <returns>The <see cref="Task"/> (void).</returns>
        Task AddOrUpdateManyAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
            where TEntity : class, IEntityCreatableAndUpdatable;
        
        /// <summary>
        /// Deletes the instance of the passed <see cref="IEntityDeletable"/>.
        /// </summary>
        /// <typeparam name="TEntity">The <see cref="IEntityDeletable"/> type.</typeparam>
        /// <param name="entity">The instance od <see cref="IEntityDeletable"/>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> (optional).</param>
        /// <returns>A <see cref="Task"/> returning 'void'.</returns>
        Task DeleteAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
            where TEntity : class, IEntityDeletable;

        /// <summary>
        /// Deletes all instances of the passed <see cref="IEntityDeletable"/>'s.
        /// </summary>
        /// <typeparam name="TEntity">The <see cref="IEntityDeletable"/> type.</typeparam>
        /// <param name="entities">The instances of <see cref="IEntityDeletable"/>'s.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> (optional).</param>
        /// <returns>The <see cref="Task"/> (void).</returns>
        Task DeleteManyAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
            where TEntity : class, IEntityDeletable;

        /// <summary>
        /// Deletes all instances of <typeparamref name="TEntity"/>, matching the criterias of the passed <paramref name="critiera"/>.
        /// </summary>
        /// <typeparam name="TEntity">The <see cref="IEntityDeletable"/> type.</typeparam>
        /// <typeparam name="TCriteria">The <see cref="IQueryCriteria"/> type.</typeparam>
        /// <param name="critiera">The <see cref="IQueryCriteria"/>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> (optional).</param>
        /// <returns>A <see cref="Task"/> (void).</returns>
        Task DeleteManyAsync<TEntity, TCriteria>(TCriteria critiera, CancellationToken cancellationToken = default)
            where TEntity : class, IEntityDeletable
            where TCriteria : class, IQueryCriteria, new();

        /// <summary>
        /// Deletes all instances matching the passed select <see cref="Expression"/>.
        /// </summary>
        /// <typeparam name="TEntity">The <see cref="IEntityUpdatable"/> type.</typeparam>
        /// <param name="select">The <see cref="Expression"/> to evaulate entities to be deleted.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> (optional).</param>
        /// <returns>The <see cref="Task"/> (void).</returns>
        Task DeleteManyAsync<TEntity>(Expression<Func<TEntity, bool>> select, CancellationToken cancellationToken = default)
            where TEntity : class, IEntityDeletable;
    }
}