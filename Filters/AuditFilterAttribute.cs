namespace PCR.Filters
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Headers;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using PCR.Models;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="AuditFilterAttribute" />.
    /// </summary>
    public class AuditFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Defines the context.
        /// </summary>
        private readonly ApplicationDBContext2 context;

        /// <summary>
        /// Defines the _httpContextAccessor.
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Defines the _roleManager.
        /// </summary>
        private readonly RoleManager<IdentityRole> _roleManager;

        /// <summary>
        /// Defines the _userManager.
        /// </summary>
        private readonly UserManager<IdentityUser> _userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditFilterAttribute"/> class.
        /// </summary>
        /// <param name="_context">The _context<see cref="ApplicationDBContext2"/>.</param>
        /// <param name="httpContextAccessor">The httpContextAccessor<see cref="IHttpContextAccessor"/>.</param>
        /// <param name="userMgr">The userMgr<see cref="UserManager{IdentityUser}"/>.</param>
        /// <param name="roleManager">The roleManager<see cref="RoleManager{IdentityRole}"/>.</param>
        public AuditFilterAttribute(ApplicationDBContext2 _context, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userMgr, RoleManager<IdentityRole> roleManager)
        {
            context = _context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userMgr;
            _roleManager = roleManager;
        }

        /// <summary>
        /// The OnActionExecutionAsync.
        /// </summary>
        /// <param name="filterContext">The filterContext<see cref="ActionExecutingContext"/>.</param>
        /// <param name="next">The next<see cref="ActionExecutionDelegate"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public override async Task OnActionExecutionAsync(
            ActionExecutingContext filterContext,
            ActionExecutionDelegate next)
        {
            try
            {
                var objaudit = new AuditModel(); // Getting Action Name 
                var controllerName = ((ControllerBase)filterContext.Controller)
                    .ControllerContext.ActionDescriptor.ControllerName;

                var actionName = ((ControllerBase)filterContext.Controller)
                    .ControllerContext.ActionDescriptor.ActionName;

                var actionDescriptorRouteValues = ((ControllerBase)filterContext.Controller)
                    .ControllerContext.ActionDescriptor.RouteValues;

                var request = filterContext.HttpContext.Request;
                if (GetName() != null)
                {
                    var user = await _userManager.FindByNameAsync(GetName());
                    if (user != null && user.Id != null)
                    {
                        objaudit.UserId = user.Id;
                        var roles = await _userManager.GetRolesAsync(user);
                        if (roles != null)
                        {
                            objaudit.RoleId = roles.ElementAt(0);
                        }

                    }


                }
                else
                {
                    objaudit.UserId = "unknown";
                    objaudit.RoleId = "unknown";
                }







                objaudit.VisitedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                objaudit.SessionId = filterContext.HttpContext.Session.Id; ; // Application SessionID // User IPAddress 
                if (_httpContextAccessor.HttpContext != null)
                    objaudit.IpAddress = Convert.ToString(_httpContextAccessor.HttpContext.Connection.RemoteIpAddress);

                objaudit.PageAccessed = Convert.ToString(filterContext.HttpContext.Request.Path); // URL User Requested 

                objaudit.LoginStatus = "A";
                objaudit.ControllerName = controllerName; // ControllerName 
                objaudit.ActionName = actionName;
                var props = objaudit.GetType().GetProperties();
                foreach (var prop in props)
                {
                    var d = prop.GetValue(objaudit, null);
                    if (d == null)
                    {
                        d = "unknown";
                    }
                }

                RequestHeaders header = request.GetTypedHeaders();
                Uri uriReferer = header.Referer;

                if (uriReferer != null)
                {
                    objaudit.UrlReferrer = header.Referer.AbsoluteUri;
                }

                context.Audit.Add(objaudit);
                await context.SaveChangesAsync();

            }
            finally
            {
                await base.OnActionExecutionAsync(filterContext, next);
            }
        }

        /// <summary>
        /// The GetName.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetName()
        {
            return _httpContextAccessor.HttpContext.User.Identity.Name;
        }
    }
}
