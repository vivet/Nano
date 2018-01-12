using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EasyNetQ;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Nano.Data;
using Nano.Eventing.Attributes;
using Nano.Eventing.Interfaces;
using Nano.Services.Eventing;

namespace Nano.Services.Data
{
    /// <inheritdoc />
    public class DefaultDbContext : BaseDbContext
    {
        /// <inheritdoc />
        public DefaultDbContext(DbContextOptions options)
            : base(options)
        {

        }

        /// <inheritdoc />
        public override int SaveChanges()
        {
            return this.SaveChanges(true);
        }

        /// <inheritdoc />
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return this.SaveChangesAsync(acceptAllChangesOnSuccess).Result;
        }

        /// <inheritdoc />
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await this.SaveChangesAsync(true, cancellationToken);
        }

        /// <inheritdoc />
        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var entities = this.ChangeTracker
                .Entries()
                .Where(x => x.Entity.GetType().GetAttributes<PublishAttribute>().Any())
                .Select(x => x.Entity);

            return await base
                .SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken)
                .ContinueWith(x =>
                {
                    if (x.IsFaulted || x.IsCanceled)
                        return x;

                    var eventing = this.GetService<IEventing>();

                    if (eventing == null)
                        return x;

                    foreach (var entity in entities)
                    {
                        var key = entity.GetType().Name;
                        var entityEvent = new EntityEvent(entity);

                        eventing
                            .Publish(entityEvent, key); 
                    }

                    return x;
                }, cancellationToken)
                .Result;
        }
    }
}