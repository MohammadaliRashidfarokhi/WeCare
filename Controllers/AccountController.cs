using DNTCaptcha.Core;

namespace PCR.Controllers
{
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using PCR.Models;
    using PCR.Models.ViewModels;
    using PCR.Services;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="AccountController" />.
    /// </summary>
    public class AccountController : Controller
    {
        /// <summary>
        /// Defines the _config.
        /// </summary>
        private readonly IConfiguration _config;

        /// <summary>
        /// Defines the _context.
        /// </summary>
        private readonly ApplicationDBContext2 _context;

        /// <summary>
        /// Defines the _contextU.
        /// </summary>
        private readonly ApplicationDBContext _contextU;

        /// <summary>
        /// Defines the _smsSender.
        /// </summary>
        private readonly ISmsSender _smsSender;

        /// <summary>
        /// Defines the _roleManager; Provides the APIs for managing roles in a persistence store...
        /// </summary>
        private readonly RoleManager<IdentityRole> _roleManager;

        /// <summary>
        /// Defines the _httpContextAccessor.
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Defines the _userManager.
        /// </summary>
        private readonly UserManager<IdentityUser> _userManager;

        /// <summary>
        /// Defines the _ipPcr.
        /// </summary>
        private readonly IPcr _ipPcr;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="userMgr">The userMgr<see cref="UserManager{IdentityUser}"/>.</param>
        /// <param name="context">The context<see cref="ApplicationDBContext2"/>.</param>
        /// <param name="contextU">The contextU<see cref="ApplicationDBContext"/>.</param>
        /// <param name="roleManager">The roleManager<see cref="RoleManager{IdentityRole}"/>.</param>
        /// <param name="smsSender">The smsSender<see cref="ISmsSender"/>.</param>
        /// <param name="httpContextAccessor">The httpContextAccessor<see cref="IHttpContextAccessor"/>.</param>
        /// <param name="signInMgr">The signInMgr<see cref="SignInManager{IdentityUser}"/>.</param>
        /// <param name="config">The config<see cref="IConfiguration"/>.</param>
        /// <param name="pcr">The pcr<see cref="IPcr"/>.</param>
        public AccountController(UserManager<IdentityUser> userMgr, ApplicationDBContext2 context, ApplicationDBContext contextU, RoleManager<IdentityRole> roleManager, ISmsSender smsSender, IHttpContextAccessor httpContextAccessor, SignInManager<IdentityUser> signInMgr, IConfiguration config, IPcr pcr
        )
        {
            _userManager = userMgr;
            _signInManager = signInMgr;
            _roleManager = roleManager;
            _config = config;
            _context = context;
            _contextU = contextU;
            _httpContextAccessor = httpContextAccessor;
            _smsSender = smsSender;
            _ipPcr = pcr;
        }


        /// <summary>
        /// Defines the _signInManager.
        /// </summary>
        private readonly SignInManager<IdentityUser> _signInManager;

        /// <summary>
        /// The Login.
        /// </summary>
        /// <param name="returnUrl">The returnUrl<see cref="string"/>.</param>
        /// <returns>The <see cref="ViewResult"/>.</returns>
        [Authorize]
        [AllowAnonymous]
        public ViewResult Login(string returnUrl)
        {

            return View(new LoginModel
            {
                ReturnUrl = returnUrl
            });
        }

        /// <summary>
        /// The Login.
        /// </summary>
        /// <param name="loginModel">The loginModel<see cref="LoginModel"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpPost]
        [AllowAnonymous]

        [ValidateAntiForgeryToken]
        [ValidateDNTCaptcha(
          ErrorMessage = "Please Enter Valid Captcha",
          CaptchaGeneratorLanguage = Language.English,
          CaptchaGeneratorDisplayMode = DisplayMode.ShowDigits)]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            var user = await _userManager.FindByNameAsync(loginModel.UserName);



            if (ModelState.IsValid)
            {

                if (user != null)
                {

                    TempData["AddPhoneNumber"] = user.UserName;

                    var test = from a in _ipPcr.Users
                        where a.UserName.Equals(user.UserName)
                        select a;
                    var asa = test.Where(a => a.Name != null).FirstOrDefault();
                    if (asa.Src.Equals("True"))
                    {
                        return RedirectToAction("AddPhoneNumber");
                    }

                    else
                    {
                        await AuditLogin(user);
                        await _signInManager.SignOutAsync();
                        if ((await _signInManager.PasswordSignInAsync(user, loginModel.Password, false, false)).Succeeded)
                        {



                            if (await _userManager.IsInRoleAsync(user, "User"))
                            {
                                return Redirect("/Report/Index");
                            }
                            if (await _userManager.IsInRoleAsync(user, "Admin"))
                            {
                                return Redirect("/Admin/Index");
                            }
                            if (await _userManager.IsInRoleAsync(user, "Doctor"))
                            {
                                return Redirect("/Doctor/Index");
                            }
                            if (await _userManager.IsInRoleAsync(user, "SuperUser"))
                            {
                                return Redirect("/Admin/Index");
                            }



                        }


                    }



                }
            }


            ModelState.AddModelError("", "Please check your username or password");
            return View(loginModel);
        }

        /// <summary>
        /// The AuditLogin.
        /// </summary>
        /// <param name="user">The user<see cref="IdentityUser"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task AuditLogin(IdentityUser user)
        {
            var objaudit = new AuditModel();
            //objaudit.RoleId = Convert.ToString(HttpContext.Session.GetInt32(AllSessionKeys.RoleId));
            objaudit.ControllerName = "Account";
            objaudit.ActionName = "Login";

            objaudit.LoggedInAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            if (_httpContextAccessor.HttpContext != null)
                objaudit.IpAddress = Convert.ToString(_httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            objaudit.UserId = Convert.ToString(HttpContext.Session.GetInt32(user.Id));
            objaudit.PageAccessed = "";
            objaudit.UrlReferrer = "";
            objaudit.SessionId = HttpContext.Session.Id;

            _context.Audit.Add(objaudit);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// The PatientRegistration.
        /// </summary>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpGet]
        public async Task<IActionResult> PatientRegistration()
        {
            var allRoles = await _roleManager.Roles.ToListAsync();


            UserViewModel viewModel = new UserViewModel
            {
                Roles = allRoles.Select(x => new RoleViewModel() { Id = x.Id, Name = x.Name, }).ToArray()
            };


            return View(viewModel);
        }

        /// <summary>
        /// The PatientRegistration.
        /// </summary>
        /// <param name="user">The user<see cref="UserViewModel"/>.</param>
        /// <param name="roleName">The roleName<see cref="string"/>.</param>
        /// <param name="GaredenName">The GaredenName<see cref="string"/>.</param>
        /// <param name="UserName">The UserName<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpPost]
        public async Task<IActionResult> PatientRegistration(UserViewModel user, string roleName, string GaredenName, string UserName)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            var allRoles = await _roleManager.Roles.ToListAsync();
            var viewModel = new UserViewModel
            {
                UserName = user.UserName,

                Email = user.Email,
                Pass = user.Pass,
                Name = user.Name /*+ _repository.NewUserId(),*/,
                Roles = allRoles.Select(x => new RoleViewModel() { Id = x.Id, Name = x.Name, }).ToArray(),//Get roles id and name and set it to Roles property.
            };
            roleName ??= "user";

            if (String.IsNullOrEmpty(UserName) || roleName.Equals("Choose role") || String.IsNullOrEmpty(roleName))
            {
                ViewBag.ERR = "Please choose a role!";
                //  ModelState.AddModelError("", "Please choose a role!");
                return RedirectToAction("PatientRegistration");
            }
            else
            {
                var identity = new IdentityUser(viewModel.UserName);

                var s = await _userManager.CreateAsync(identity, viewModel.Pass);

                if (s.Succeeded) //Check that registering succeeded in this case add the new user to Employee table in the Database and add to role.
                {
                    var k = await _userManager.AddToRoleAsync(identity, roleName);//Add it to the roles.
                    if (!k.Succeeded)
                    {
                        ModelState.AddModelError("", "Please choose a role!");
                        return RedirectToAction("Login");
                    }

                    var email = await _userManager.SetEmailAsync(identity, viewModel.Email); //Set user's Email
                    if (!email.Succeeded)
                    {
                        ModelState.AddModelError("", "Please check your email! ");
                        return RedirectToAction("Login");
                    }
                    _ipPcr.AddUser(viewModel.UserName, viewModel.Email, roleName, viewModel.Name);
                    return RedirectToAction("Index", "Home");
                }




            }




            return RedirectToAction("PatientRegistration");
        }

        /// <summary>
        /// The ForgotPassword.
        /// </summary>
        /// <returns>The <see cref="ActionResult"/>.</returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        /// <summary>
        /// The DeleteCookies.
        /// </summary>
        private void DeleteCookies()
        {
            foreach (var cookie in HttpContext.Request.Cookies)
            {
                Response.Cookies.Delete(cookie.Key);
            }
        }

        /// <summary>
        /// The ForgotPassword.
        /// </summary>
        /// <param name="EmailID">The EmailID (user's input) <see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(string EmailID)
        {
            //User variable to find the user that have that (EmailID).
            var user = await _userManager.FindByEmailAsync(EmailID);

            var message = "";

            if (user != null)
            {


                var body = "We are sending you this email because you requested a password reset." +
                           "If you did not request a password reset, you can ignore this email address. Your password will not be changed." +
                           "Click this link to create a new password:";
                // Token a variable that will generate a password reset token that will be set to this user for 5 min (as i have choose).
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                _ipPcr.ChangeUser(user.UserName, "True");
             
               



                // Callback to get the path for the action method and controller, with the token and email and the HttpRequest to be send with the email.
                var callback = Url.Action(nameof(ResetPassword), "Account", new { token, email = user.Email }, Request.Scheme);
                TempData["EM"] = EmailID;
                //A method that will send the email to reset the password.
                SendVerificationLinkEmail(user.Email, callback, "Reset Password", body);


                ViewBag.Message = $"The link has been sent to  {user.Email}.";
                message = $"The link has been sent to  {user.Email}.";
            }
            else
            {

                ModelState.AddModelError("", "Please check your Email");
                return View();
            }
            ViewBag.Message = message;
            return View();
        }

        /// <summary>
        /// The ResetPassword.
        /// </summary>
        /// <param name="token">The token<see cref="string"/>.</param>
        /// <param name="email">The email<see cref="string"/>.</param>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        [AllowAnonymous]
        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            var model = new ResetPasswordModel { Token = token, Email = email };
            return View(model);
        }

        /// <summary>
        /// The SendVerificationLinkEmail.
        /// </summary>
        /// <param name="emailID">The emailID<see cref="string"/>.</param>
        /// <param name="activationCode">The activationCode<see cref="string"/>.</param>
        /// <param name="subject">The subject<see cref="string"/>.</param>
        /// <param name="body">The body<see cref="string"/>.</param>
        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode, string subject, string body)
        {
            var verifyUrl = activationCode;

            TempData["Em"] = emailID;

            var email_sender = _config.GetValue<string>("EmailSender:Email");
            var password_sender = _config.GetValue<string>("EmailSender:Password");

            //fromEmail a variable that connected to MailAddress object witch take the  email sender and the name that will be displayed.
            var fromEmail = new MailAddress(email_sender, "Abcure AB");

            //toEmail will take the received email address.
            var toEmail = new MailAddress(emailID);

            //Set the password for the email sender.
            string fromEmailPassword = password_sender;


            var a = body + verifyUrl;

            //SmtpClient (Simple Mail Transfer Protocol) to send emails.
            var smtp = new SmtpClient
            {
                Host = "smtp.office365.com", //The host for Hotmail
                Port = 587,
                EnableSsl = true,//To  encrypt the connection
                DeliveryMethod = SmtpDeliveryMethod.Network,//Send the email via the network to the SMTP server.
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword) //To  authenticate the client before sending the email.
            };

            var message = new MailMessage(fromEmail, toEmail) { Subject = subject, Body = a, IsBodyHtml = true };

            smtp.Send(message);
        }

        /// <summary>
        /// The ResetPassword.
        /// </summary>
        /// <param name="resetPasswordModel">The resetPasswordModel<see cref="ResetPasswordModel"/>.</param>
        /// <param name="Password">The Password<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPasswordModel, string Password)
        {


            var user = await _userManager.FindByEmailAsync(resetPasswordModel.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Oops, something went wrong");
                RedirectToAction(nameof(ResetPassword));
            }

            //Reset the user's password by using the token that have been sent to the user and the provided new password.
            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordModel.Token, Password);

            if (resetPassResult.Succeeded) return RedirectToAction(nameof(Login));
            else
            {
                ModelState.AddModelError("", "Oops, something went wrong");
                RedirectToAction(nameof(ResetPassword));
            }


            return View();
        }

        /// <summary>
        /// The Settings.
        /// </summary>
        /// <returns>The <see cref="ViewResult"/>.</returns>
        [Authorize]
        public ViewResult Settings()
        {
            return View();
        }

        /// <summary>
        /// The LogOut.
        /// </summary>
        /// <param name="returnUrl">The returnUrl<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{RedirectResult}"/>.</returns>
        [Authorize]
        [AllowAnonymous]
        public async Task<RedirectResult> LogOut(string returnUrl = "/")
        {

            try
            {
                AuditLogout();
                //6.4.
                // Removing Session
                HttpContext.Session.Clear();
                //6.4.
                // Removing Cookies
                CookieOptions option = new CookieOptions { Secure = true, HttpOnly = true };
                if (Request.Cookies[".AspNetCore.Session"] != null)
                {
                    option.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Append(".AspNetCore.Session", "", option);
                }

                if (Request.Cookies["AuthenticationToken"] != null)
                {
                    option.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Append("AuthenticationToken", "", option);
                }



                await _signInManager.SignOutAsync();
                foreach (var cookieKey in Request.Cookies.Keys)
                {
                    Response.Cookies.Delete(cookieKey);
                }

                return Redirect(returnUrl);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
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

        /// <summary>
        /// The AuditLogout.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task AuditLogout()
        {
            var objaudit = new AuditModel();
         
            objaudit.ControllerName = "Portal";
            objaudit.ActionName = "Logout";

            objaudit.LoggedOutAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            if (_httpContextAccessor.HttpContext != null)
                objaudit.IpAddress = Convert.ToString(_httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            // objaudit.UserId = Convert.ToString(HttpContext.Session.GetInt32(user.Id));
            objaudit.PageAccessed = "";
            objaudit.UrlReferrer = "";
            objaudit.SessionId = HttpContext.Session.Id;

            _context.Audit.Add(objaudit);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// The Settings method will help the users to change their   .
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <param name="EmailID">The EmailID<see cref="string"/>.</param>
        /// <param name="Cpassword">The Cpassword<see cref="string"/>.</param>
        /// <param name="CurrentPass">The CurrentPass<see cref="string"/>.</param>
        /// <param name="NewPassword">The NewPassword<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Settings(string id, string EmailID, string Cpassword, string CurrentPass, string NewPassword)
        {

            if (String.Equals(Cpassword, NewPassword))
            {
                var user = await _userManager.FindByNameAsync(_ipPcr.GetName());

                if (user != null)
                {
                    user.Email = EmailID;
                    var pass = await _userManager.ChangePasswordAsync(user, CurrentPass, NewPassword);
                    if (!pass.Succeeded)
                    {
                        ModelState.AddModelError("", "Wrong email or password");
                        return View();
                    }

                    var result = await _userManager.UpdateAsync(user);
                    _ipPcr.UpdateUserInfo(EmailID,NewPassword,Cpassword);
                    if (result.Succeeded && pass.Succeeded)
                        return RedirectToAction("Login");

                }
                else
                {
                    ModelState.AddModelError("", "The user was not found");

                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("", "password does not match!");
                return View();
            }



            return View();
        }

        /// <summary>
        /// The AddPhoneNumber.
        /// </summary>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        public IActionResult AddPhoneNumber()
        {
            return View();
        }
        [HttpGet]
        [Route("/Account/AccessDenied")]
        public ActionResult AccessDenied()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        /// <summary>
        /// The AddPhoneNumber.
        /// </summary>
        /// <param name="model">The model<see cref="AddPhoneNumberViewModel"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var user = await _userManager.FindByNameAsync(TempData["AddPhoneNumber"].ToString()); //_userManager.FindByNameAsync("loginModel.UserName");
            //    var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            else
            {
                TempData["VerifyPhoneNumber"] = user.UserName;
            }
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, model.PhoneNumber);
            await _smsSender.SendSmsAsync(model.PhoneNumber, "Your security code is: " + code);
            return RedirectToAction(nameof(VerifyPhoneNumber), new { PhoneNumber = model.PhoneNumber });
        }

        /// <summary>
        /// The VerifyPhoneNumber.
        /// </summary>
        /// <param name="phoneNumber">The phoneNumber<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpGet]
        public async Task<IActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var user = await _userManager.FindByNameAsync(TempData["VerifyPhoneNumber"].ToString()); //_userManager.FindByNameAsync("loginModel.UserName");
            //await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            else
            {
                TempData["VerifyPhoneNumberTask"] = user.UserName;
            }
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumber);
            // Send an SMS to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        /// <summary>
        /// The VerifyPhoneNumber.
        /// </summary>
        /// <param name="model">The model<see cref="VerifyPhoneNumberViewModel"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByNameAsync(TempData["VerifyPhoneNumberTask"].ToString());//GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.ChangePhoneNumberAsync(user, model.PhoneNumber, model.Code);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _ipPcr.ChangeUser(user.UserName, "False");
                    return RedirectToAction(nameof(Login));
                }
            }
            // If we got this far, something failed, redisplay the form
            ModelState.AddModelError(string.Empty, "Failed to verify phone number");
            return View(model);
        }

        /// <summary>
        /// The GetCurrentUserAsync.
        /// </summary>
        /// <returns>The <see cref="Task{IdentityUser}"/>.</returns>
        private Task<IdentityUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }
    }
}
