namespace PCR.Models
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IdentityInitializer" />.
    /// </summary>
    public class IdentityInitializer
    {
        /// <summary>
        /// The EnsurePopulated.
        /// </summary>
        /// <param name="services">The services<see cref="IServiceProvider"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public static async Task EnsurePopulated(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            await CreatRoles(roleManager);
            await CreateUsers(userManager);
        }

        /// <summary>
        /// The CreatRoles.
        /// </summary>
        /// <param name="rManager">The rManager<see cref="RoleManager{IdentityRole}"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private static async Task CreatRoles(RoleManager<IdentityRole> rManager)
        {
            if (!await rManager.RoleExistsAsync("Admin"))
            {
                await rManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!await rManager.RoleExistsAsync("User"))
            {
                await rManager.CreateAsync(new IdentityRole("User"));
            }

            if (!await rManager.RoleExistsAsync("Doctor"))
            {
                await rManager.CreateAsync(new IdentityRole("Doctor"));
            }
        }

        /// <summary>
        /// The CreateUsers.
        /// </summary>
        /// <param name="uManager">The uManager<see cref="UserManager{IdentityUser}"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private static async Task CreateUsers(UserManager<IdentityUser> uManager)
        {

            IdentityUser Sa100 = new IdentityUser("Saadou");
            Sa100.Email = "Saadsyr3@gmail.com";
            Sa100.NormalizedEmail = "Saadsyr3@gmail.com";
            await uManager.CreateAsync(Sa100, "Saad001!!");
            await uManager.AddToRoleAsync(Sa100, "Admin");


            IdentityUser SaadDoc100 = new IdentityUser("SaadDoc");
            SaadDoc100.Email = "Saedaldin.ou@gmail.com";
            SaadDoc100.NormalizedEmail = "Saedaldin.ou@gmail.com";
            await uManager.CreateAsync(SaadDoc100, "Saad001!!");
            await uManager.AddToRoleAsync(SaadDoc100, "Doctor");


            IdentityUser SAnnaDoc100 = new IdentityUser("AnnaDoc");
            SAnnaDoc100.Email = "anna@gmail.com";
            SAnnaDoc100.NormalizedEmail = "anna@gmail.com";
            await uManager.CreateAsync(SAnnaDoc100, "Saad001!!");
            await uManager.AddToRoleAsync(SAnnaDoc100, "Doctor");

            IdentityUser EmmaDoc100 = new IdentityUser("EmmaDoc");
            EmmaDoc100.Email = "emma@gmail.com";
            EmmaDoc100.NormalizedEmail = "emma@gmail.com";
            await uManager.CreateAsync(EmmaDoc100, "Saad001!!");
            await uManager.AddToRoleAsync(EmmaDoc100, "Doctor");


            IdentityUser MikaelDoc100 = new IdentityUser("MikaelDoc");
            MikaelDoc100.Email = "Mikael@gmail.com";
            MikaelDoc100.NormalizedEmail = "Mikael@gmail.com";
            await uManager.CreateAsync(MikaelDoc100, "Saad001!!");
            await uManager.AddToRoleAsync(MikaelDoc100, "Doctor");


            IdentityUser Mi100 = new IdentityUser("Abdoou");
            Mi100.Email = "abdobston6106@yahoo.com";
            Mi100.NormalizedEmail = "abdobston6106@yahoo.com";
            await uManager.CreateAsync(Mi100, "Abdo002!!");
            await uManager.AddToRoleAsync(Mi100, "User");
        }
    }
}
