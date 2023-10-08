namespace PCR.Models
{
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Defines the <see cref="ApplicationDBContext2" />.
    /// </summary>
    public class ApplicationDBContext2 : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDBContext2"/> class.
        /// </summary>
        /// <param name="options">The options<see cref="DbContextOptions{ApplicationDBContext2}"/>.</param>
        public ApplicationDBContext2(DbContextOptions<ApplicationDBContext2> options) : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the Audit.
        /// </summary>
        public DbSet<AuditModel> Audit { get; set; }
    }
}
