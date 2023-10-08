namespace PCR.Models.ViewModels
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Defines the <see cref="UserViewModel" />.
    /// </summary>
    public class UserViewModel : IdentityUser
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        [ForeignKey("FK_AspNetUserRoles_AspNetUsers_UserId")]
        public override string Id { get; set; }

        /// <summary>
        /// Gets or sets the User.
        /// </summary>
        public IdentityUser User { get; set; }

        /// <summary>
        /// Gets or sets the UserName.
        /// </summary>
        [Required(ErrorMessage = "Vänligen fyll i användarenamn.")]
        [Display(Name = "User Name:")]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        [Required(ErrorMessage = "Vänligen fyll i namn.")]
        [Display(Name = "Namn:")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Pass.
        /// </summary>
        [Required(ErrorMessage = "Vänligen fyll i lösenord.")]
        [Display(Name = "Lösenord:")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$", ErrorMessage = "Lösenordet måste bestå av minst 8 tecken (minst en stor bokstav , en liten bokstav och   en siffra) samt minst ett specialtecken")]

        //[UIHint("Password")]
        public string Pass { get; set; }

        /// <summary>
        /// Gets or sets the Email.
        /// </summary>
        [Required(ErrorMessage = "Vänligen fyll i Email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the Roles.
        /// </summary>
        public RoleViewModel[] Roles { get; set; }
    }
}
