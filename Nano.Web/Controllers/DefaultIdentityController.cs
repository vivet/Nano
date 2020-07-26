using System;
using DynamicExpression.Interfaces;
using Microsoft.Extensions.Logging;
using Nano.Eventing.Interfaces;
using Nano.Models;
using Nano.Models.Interfaces;
using Nano.Repository.Interfaces;
using Nano.Security;

namespace Nano.Web.Controllers
{
    /// <inheritdoc />
    public abstract class DefaultIdentityController<TEntity, TCriteria> : BaseIdentityController<TEntity, Guid, TCriteria>
        where TEntity : DefaultEntityUser, IEntityUpdatable, new()
        where TCriteria : class, IQueryCriteria, new()
    {
        /// <inheritdoc />
        protected DefaultIdentityController(ILogger logger, IRepository repository, IEventing eventing, IdentityManager identityManager) 
            : base(logger, repository, eventing, identityManager)
        {

        }
    }
}