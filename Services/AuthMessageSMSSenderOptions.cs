namespace PCR.Services
{
    /// <summary>
    /// Defines the <see cref="AuthMessageSMSSenderOptions" />.
    /// </summary>
    public class AuthMessageSMSSenderOptions
    {
        /// <summary>
        /// Gets or sets the SID.
        /// </summary>
        public string SID { get; set; }

        /// <summary>
        /// Gets or sets the AuthToken.
        /// </summary>
        public string AuthToken { get; set; }

        /// <summary>
        /// Gets or sets the SendNumber.
        /// </summary>
        public string SendNumber { get; set; }
    }
}
