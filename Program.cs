namespace PCR
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using PCR.Models;
    using System;

    /// <summary>
    /// Defines the <see cref="Program" />.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The Main.
        /// </summary>
        /// <param name="args">The args<see cref="string[]"/>.</param>
        public static void Main(string[] args)
        {
            //var appConfig = new ConfigurationBuilder()
            //  .AddUserSecrets<Program>()
            //  .Build();


            var host = CreateHostBuilder(args).Build();

            IntitializeDatabase(host);
            host.Run();
        }

        /// <summary>
        /// The IntitializeDatabase.
        /// </summary>
        /// <param name="host">The host<see cref="IHost"/>.</param>
        private static void IntitializeDatabase(IHost host)
        {

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    DBinitializer.EnsurePopulated(services);
                    IdentityInitializer.EnsurePopulated(services).Wait();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Ett fel uppstod n?r databasen skulle fyllas med data");
                }
            }

            host.Run();
        }

        /// <summary>
        /// The CreateHostBuilder.
        /// </summary>
        /// <param name="args">The args<see cref="string[]"/>.</param>
        /// <returns>The <see cref="IHostBuilder"/>.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();

                });
    }
}
