namespace PCR.Models
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Defines the <see cref="AppIdentityDbContext" />.
    /// </summary>
    public class AppIdentityDbContext : IdentityDbContext<IdentityUser>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppIdentityDbContext"/> class.
        /// </summary>
        /// <param name="options">The options<see cref="DbContextOptions{AppIdentityDbContext}"/>.</param>
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {
        }
    }
}
