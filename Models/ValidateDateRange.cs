namespace PCR.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="ValidateDateRange" />.
    /// </summary>
    public class ValidateDateRange : ValidationAttribute
    {
        /// <summary>
        /// The IsValid.
        /// </summary>
        /// <param name="value">The value<see cref="object"/>.</param>
        /// <param name="validationContext">The validationContext<see cref="ValidationContext"/>.</param>
        /// <returns>The <see cref="ValidationResult"/>.</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime dt = (DateTime)value;
            // check if time greater than 10:00 pm and less than 07:00 am
            if (dt.TimeOfDay > new TimeSpan(22, 0, 0) || dt.TimeOfDay < new TimeSpan(7, 0, 0) || dt < DateTime.Now)
            {
                return new ValidationResult("Time must be between 7:00 AM and 10:00 PM and date must be today or later");
            }

            // check if date is less than todays date 


            //if (dt <= DateTime.Now.AddDays(-5).AddHours(-3))
            //{
            //    return ValidationResult.Success;
            //}
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}
