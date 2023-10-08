namespace PCR
{
    using DinkToPdf;
    using DinkToPdf.Contracts;
    using DNTCaptcha.Core;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using PCR.Filters;
    using PCR.Models;
    using PCR.Services;
    using System;

    /// <summary>
    /// Defines the <see cref="Startup" />.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Gets the Configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Defines the _Configuration.
        /// </summary>
        public IConfiguration _Configuration;

        /// <summary>
        /// Defines the iEnvironment.
        /// </summary>
        private IWebHostEnvironment iEnvironment;

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration<see cref="IConfiguration"/>.</param>
        /// <param name="env">The env<see cref="IWebHostEnvironment"/>.</param>
        /// <param name="conf">The conf<see cref="IConfiguration"/>.</param>
        public Startup(IConfiguration configuration, IWebHostEnvironment env, IConfiguration conf)
        {
            iEnvironment = env;
            Configuration = configuration;
            _Configuration = conf;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// The ConfigureServices.
        /// </summary>
        /// <param name="services">The services<see cref="IServiceCollection"/>.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDBContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<AppIdentityDbContext>(options =>
              options.UseSqlServer(Configuration.GetConnectionString("IdentitytConnection")));

            services.AddDbContext<ApplicationDBContext2>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("AuditConnection")));

            services.AddIdentity<IdentityUser, IdentityRole>().AddRoles<IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();

            services.Configure<ApplicationDBContext>(o =>

              // Make sure the identity database is created
              o.Database.Migrate()
            );
            services.Configure<ApplicationDBContext2>(o =>

              // Make sure the identity database is created
              o.Database.Migrate()
            );
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(typeof(AuditFilterAttribute));
            });
            services.Configure<AppIdentityDbContext>(o =>

                // Make sure the identity database is created
                o.Database.Migrate()
            );
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                    options.Cookie.IsEssential = true;
                    //        options.SlidingExpiration = true; // here 1
                    options.ExpireTimeSpan = TimeSpan.FromSeconds(10);// here 2
                });
            services.AddScoped<AuditFilterAttribute>();
            services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);

            //    services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.Configure<AuthMessageSMSSenderOptions>(Configuration);
          
            System.Security.Cryptography.Aes aes = System.Security.Cryptography.Aes.Create();
            aes.GenerateIV();
            aes.GenerateKey();
            services.AddDNTCaptcha(options =>
                options.UseCookieStorageProvider()
                    .ShowThousandsSeparators(false).WithEncryptionKey(aes.Key.ToString())
            );

            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
            });
            services.AddHttpContextAccessor();
            services.AddRazorPages()
                .AddMvcOptions(options =>
                {
                    options.MaxModelValidationErrors = 50;
                    options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(
                        _ => "The field is required.");
                });
           
            services.AddSession(options =>
            {
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.IsEssential = true;
            });

            services.AddTransient<ApplicationDBContext>();
            services.AddTransient<ApplicationDBContext2>();

            services.AddDataProtection();

            services.AddTransient<IPcr, EFPcr>();
            services.AddControllersWithViews();
            services.AddMvc();

            services.AddSession(session =>
            {
                session.Cookie.IsEssential = true;
                session.IdleTimeout = TimeSpan.FromMinutes(5);
            });
            services.Configure<DataProtectionTokenProviderOptions>(opt =>
              opt.TokenLifespan = TimeSpan.FromMinutes(5));

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// The Configure.
        /// </summary>
        /// <param name="app">The app<see cref="IApplicationBuilder"/>.</param>
        /// <param name="env">The env<see cref="IWebHostEnvironment"/>.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {

                app.UseDeveloperExceptionPage();
            }

            else
            {
                app.UseDeveloperExceptionPage();
            }
            //app.UseHttpsRedirection();


            app.UseStatusCodePages();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
