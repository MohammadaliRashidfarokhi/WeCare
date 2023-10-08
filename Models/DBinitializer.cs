namespace PCR.Models
{
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="DBinitializer" />.
    /// </summary>
    public class DBinitializer
    {
        /// <summary>
        /// The EnsurePopulated.
        /// </summary>
        /// <param name="services">The services<see cref="IServiceProvider"/>.</param>
        public static void EnsurePopulated(IServiceProvider services)
        {
            var context = services.GetRequiredService<ApplicationDBContext>();
            if (!context.Users.Any())
            {

                context.Users.AddRange(
                    new User { Userid = "1", Name = "Saad", UserName = "Saad20", Email = "Saadsyr3@gmail.com", Role = "Admin",Src = "False" },
                    new User { Userid = "2", Name = "Abdo", UserName = "Abdo20", Email = "abdobston6106@yahoo.com", Role = "User", Src = "False" },
                    new User { Userid = "3", Name = "Dr. Saad", UserName = "SaadDoc", Email = "Saedaldin.ou@gmail.com", Role = "Doctor", Clicicid = "1", Src = "False" },

                    new User { Userid = "4", Name = "Dr. Anna", UserName = "AnnaDoc", Email = "anna@gmail.com", Role = "Doctor", Clicicid = "1", Src = "False" },

                    new User { Userid = "5", Name = "Dr. Emma", UserName = "EmmaDoc", Email = "emma@gmail.com", Role = "Doctor", Clicicid = "1", Src = "False" },

                    new User { Userid = "6", Name = "Dr. Mikael", UserName = "MikaelDoc", Email = "mikael@gmail.com", Role = "Doctor", Clicicid = "1", Src = "False" }
                );
                context.SaveChanges(); //spara tabellen
            }
            if (!context.Clinics.Any())
            {
                context.Clinics.AddRange(
            new Clinic { ClinicID = "1", ClinicName = "WeCare" }

                );
                context.SaveChanges(); //spara tabellen
            }
        }
    }
}
