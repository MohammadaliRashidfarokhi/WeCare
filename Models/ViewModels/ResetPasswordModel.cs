namespace PCR.Models.ViewModels
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Defines the <see cref="ResetPasswordModel" />.
    /// </summary>
    public class ResetPasswordModel
    {
        /// <summary>
        /// Gets or sets the NewPassword.
        /// </summary>
        [Required(ErrorMessage = "Write password.", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword", ErrorMessage = "Passwords does not matched!")]
        public string NewPassword { get; set; }

        /// <summary>
        /// Gets or sets the ConfirmPassword.
        /// </summary>
        [DataType(DataType.Password)]
        [NotMapped]

        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Gets or sets the Email.
        /// </summary>
        [Required(ErrorMessage = "Pleas fill in your E-mail")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the CurrentPass.
        /// </summary>
        public string CurrentPass { get; set; }

        /// <summary>
        /// Gets or sets the User.
        /// </summary>
        public IdentityUser User { get; set; }

        /// <summary>
        /// Gets or sets the Token.
        /// </summary>
        public string Token { get; set; }
    }
}
