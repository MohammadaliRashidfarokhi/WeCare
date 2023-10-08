namespace PCR.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="LoginModel" />.
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// Gets or sets the UserName.
        /// </summary>
        [Required(ErrorMessage = "Vennligst skriv inn brukernavnet.")]
        [Display(Name = "Brukernavn:")]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the Password.
        /// </summary>
        [Required(ErrorMessage = "Skriv inn passord.")]
        [Display(Name = "Passord:")]
        [UIHint("Password")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the ReturnUrl.
        /// </summary>
        public string ReturnUrl { get; set; } = "/";
    }
}
