namespace PCR.Models.ViewModels
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="RoleViewModel" />.
    /// </summary>
    public class RoleViewModel
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Selected.
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// Gets or sets the Roles.
        /// </summary>
        public IEnumerable<string> Roles { get; set; }
    }
}
