namespace PCR.Models.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="RoleModification" />.
    /// </summary>
    public class RoleModification
    {
        /// <summary>
        /// Gets or sets the RoleName.
        /// </summary>
        [Required]
        public string RoleName { get; set; }

        /// <summary>
        /// Gets or sets the RoleId.
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// Gets or sets the AddIds.
        /// </summary>
        public string[] AddIds { get; set; }

        /// <summary>
        /// Gets or sets the DeleteIds.
        /// </summary>
        public string[] DeleteIds { get; set; }
    }
}
