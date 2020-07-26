using Microsoft.AspNetCore.Identity;
using Nano.Models.Attributes;

namespace Nano.Models.Interfaces
{
    /// <summary>
    /// Entity User inteface.
    /// </summary>
    /// <typeparam name="TIdentity">The identity type.</typeparam>
    public interface IEntityUser<TIdentity> : IEntityIdentity<TIdentity>
    {
        /// <summary>
        /// Identity User Id.
        /// </summary>
        string IdentityUserId { get; set; }

        /// <summary>
        /// Identity User.
        /// Always included using <see cref="IncludeAttribute"/>.
        /// </summary>
        [Include]
        IdentityUser IdentityUser { get; set; }
    }
}