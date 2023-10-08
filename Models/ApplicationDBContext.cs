namespace PCR.Models
{
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Defines the <see cref="ApplicationDBContext" />.
    /// </summary>
    public class ApplicationDBContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDBContext"/> class.
        /// </summary>
        /// <param name="options">The options<see cref="DbContextOptions{ApplicationDBContext}"/>.</param>
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the Users.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Gets or sets the Reports.
        /// </summary>
        public DbSet<Report> Reports { get; set; }

        /// <summary>
        /// Gets or sets the Samples.
        /// </summary>
        public DbSet<Sample> Samples { get; set; }

        /// <summary>
        /// Gets or sets the Clinics.
        /// </summary>
        public DbSet<Clinic> Clinics { get; set; }
    }
}
