namespace PCR.Models.ViewModels
{
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="RoleEdit" />.
    /// </summary>
    public class RoleEdit
    {
        /// <summary>
        /// Gets or sets the Role.
        /// </summary>
        public IdentityRole Role { get; set; }

        /// <summary>
        /// Gets or sets the Members.
        /// </summary>
        public IEnumerable<IdentityUser> Members { get; set; }

        /// <summary>
        /// Gets or sets the NonMembers.
        /// </summary>
        public IEnumerable<IdentityUser> NonMembers { get; set; }
    }
}
