namespace PCR.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="AuditModel" />.
    /// </summary>
    public class AuditModel
    {
        /// <summary>
        /// Gets or sets the AuditId.
        /// </summary>
        [Key]
        public int AuditId { get; set; }

        /// <summary>
        /// Gets or sets the ControllerName.
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// Gets or sets the ActionName.
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// Gets or sets the RoleId.
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// Gets or sets the IpAddress.
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the VisitedAt.
        /// </summary>
        public string VisitedAt { get; set; }

        /// <summary>
        /// Gets or sets the LoggedInAt.
        /// </summary>
        public string LoggedInAt { get; set; }

        /// <summary>
        /// Gets or sets the LoggedOutAt.
        /// </summary>
        public string LoggedOutAt { get; set; }

        /// <summary>
        /// Gets or sets the LoginStatus.
        /// </summary>
        public string LoginStatus { get; set; }

        /// <summary>
        /// Gets or sets the PageAccessed.
        /// </summary>
        public string PageAccessed { get; set; }

        /// <summary>
        /// Gets or sets the SessionId.
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets the UrlReferrer.
        /// </summary>
        public string UrlReferrer { get; set; }

        /// <summary>
        /// Gets or sets the UserId.
        /// </summary>
        public string UserId { get; set; }
    }
}
