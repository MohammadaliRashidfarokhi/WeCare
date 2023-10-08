namespace PCR.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="Report" />.
    /// </summary>
    public class Report
    {
        /// <summary>
        /// Gets or sets the Reportid.
        /// </summary>
        public string Reportid { get; set; }

        /// <summary>
        /// Gets or sets the TestType.
        /// </summary>
        public string TestType { get; set; }

        /// <summary>
        /// Gets or sets the Clinique.
        /// </summary>
        public string Clinique { get; set; }

        /// <summary>
        /// Gets or sets the Patient.
        /// </summary>
        [RegularExpression(@"(\w+)\s+\1", ErrorMessage = "Please enter a valid name")]
        public string Patient { get; set; }

        /// <summary>
        /// Gets or sets the DateOfObservation.
        /// </summary>
        [ValidateDateRange]
        public DateTime DateOfObservation { get; set; }

        /// <summary>
        /// Gets or sets the BirthDate.
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Gets or sets the PassportNumber.
        /// </summary>
        [RegularExpression(@"^[A-Za-z]{1}[0-9]{7}$", ErrorMessage = "The accepted formats are: M1234567, b1234567")]
        public string PassportNumber { get; set; }

        /// <summary>
        /// Gets or sets the Phonenumber.
        /// </summary>
        [RegularExpression(@"^(\+\d{2}[ \-]{0,1}){0,1}(((\({0,1}[ \-]{0,1})0{0,1}\){0,1}[2|3|7|8]{1}\){0,1}[ \-]*(\d{4}[ \-]{0,1}\d{4}))|(1[ \-]{0,1}(300|800|900|902)[ \-]{0,1}((\d{6})|(\d{3}[ \-]{0,1}\d{3})))|(13[ \-]{0,1}([\d \-]{5})|((\({0,1}[ \-]{0,1})0{0,1}\){0,1}4{1}[\d \-]{8,10})))$", ErrorMessage = "Please enter a valid phone number")]
        public string Phonenumber { get; set; }

        /// <summary>
        /// Gets or sets the Email.
        /// </summary>
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid email")]
        [Compare("EmailConf", ErrorMessage = "Emails does not matched!")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the EmailConf.
        /// </summary>
       
        public string EmailConf { get; set; }

        /// <summary>
        /// Gets or sets the Citizenship.
        /// </summary>
        public string Citizenship { get; set; }

        /// <summary>
        /// Gets or sets the PersonalIdentityNumber.
        /// </summary>
        public string PersonalIdentityNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Checked.
        /// </summary>
        public bool Checked { get; set; }

        /// <summary>
        /// Gets or sets the checkedBy.
        /// </summary>
        public string checkedBy { get; set; }

        /// <summary>
        /// Gets or sets the Samples.
        /// </summary>
        public ICollection<Sample> Samples { get; set; }

        /// <summary>
        /// Gets or sets the UserName.
        /// </summary>
        public string UserName { get; set; }
    }
}
