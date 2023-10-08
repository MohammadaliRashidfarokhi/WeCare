namespace PCR.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.DataProtection;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using PCR.Models;
    using PCR.Models.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="AdminController" />.
    /// </summary>
    [Authorize(Roles = "Admin,SuperUser")]

    public class AdminController : Controller
    {
        /// <summary>
        /// Defines the _userManager which is provides the APIs for managing user in a persistence store...
        /// </summary>
        private readonly UserManager<IdentityUser> _userManager;

        /// <summary>
        /// Defines the _repository.
        /// </summary>
        private readonly IPcr _repository;

        /// <summary>
        /// Defines the _roleManager; Provides the APIs for managing roles in a persistence store...
        /// </summary>
        private readonly RoleManager<IdentityRole> _roleManager;

        /// <summary>
        /// Defines the _protector.
        /// </summary>
        private readonly IDataProtector _protector;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminController"/> class.
        /// </summary>
        /// <param name="userManager">The userManager<see cref="UserManager{IdentityUser}"/>.</param>
        /// <param name="roleManager">The roleManager<see cref="RoleManager{IdentityRole}"/>.</param>
        /// <param name="pcr">The pcr<see cref="IPcr"/>.</param>
        public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IPcr pcr)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _repository = pcr;
        }

        /// <summary>
        /// The Index.
        /// </summary>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        public IActionResult Index()
        {

            return View(_userManager.Users);
        }

        /// <summary>
        /// The CreateRole.
        /// </summary>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        public IActionResult CreateRole() => View();

        /// <summary>
        /// The CreateRole.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateRole([Required] string name)
        {

            if (ModelState.IsValid)
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    Errors(result);
            }
            return View(name);
        }

        /// <summary>
        /// The Roles.
        /// </summary>
        /// <returns>The <see cref="ViewResult"/>.</returns>
        public ViewResult Roles() => View(_roleManager.Roles);

        /// <summary>
        /// The Update.
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        public async Task<IActionResult> Update(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            List<IdentityUser> members = new List<IdentityUser>();
            List<IdentityUser> nonMembers = new List<IdentityUser>();
            var userList = await _userManager.Users.ToListAsync();
            foreach (var user in userList)
            {
                var list = await _userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
                list.Add(user);
            }
            return View(new RoleEdit
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            });
        }

        /// <summary>
        /// The Update.
        /// </summary>
        /// <param name="model">The model<see cref="RoleModification"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpPost]
        public async Task<IActionResult> Update(RoleModification model)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result;
                foreach (string userId in model.AddIds ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await _userManager.AddToRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                            Errors(result);
                    }
                }
                foreach (string userId in model.DeleteIds ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                            Errors(result);
                    }
                }
            }

            if (ModelState.IsValid)
                return RedirectToAction(nameof(Index));
            else
                return await Update(model.RoleId);
        }

        /// <summary>
        /// The CreateUser.
        /// </summary>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpGet]
        public async Task<IActionResult> CreateUser()
        {
            var allRoles = await _roleManager.Roles.ToListAsync();


            UserViewModel viewModel = new UserViewModel
            {
                Roles = allRoles.Select(x => new RoleViewModel() { Id = x.Id, Name = x.Name, }).ToArray()
            };


            return View(viewModel);
        }

        /// <summary>
        /// The CreateUser.
        /// </summary>
        /// <param name="user">The user<see cref="UserViewModel"/>.</param>
        /// <param name="roleName">The roleName<see cref="string"/>.</param>
        /// <param name="GaredenName">The GaredenName<see cref="string"/>.</param>
        /// <param name="UserName">The UserName<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateUser(UserViewModel user, string roleName, string GaredenName, string UserName)
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

            if (String.IsNullOrEmpty(UserName) || roleName.Equals("Choose role") || String.IsNullOrEmpty(roleName))
            {
                ViewBag.ERR = "Please choose a role!";
                //  ModelState.AddModelError("", "Please choose a role!");
                return RedirectToAction("CreateUser");
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
                        return RedirectToAction("CreateUser");
                    }

                    var email = await _userManager.SetEmailAsync(identity, viewModel.Email); //Set user's Email
                    if (!email.Succeeded)
                    {
                        ModelState.AddModelError("", "Please check your email! ");
                        return RedirectToAction("CreateUser");
                    }
                    _repository.AddUser(viewModel.UserName, viewModel.Email, roleName, viewModel.Name);
                    return RedirectToAction("Index", "Home");
                }




            }




            return RedirectToAction("CreateUser");
        }

        /// <summary>
        /// The DeleteRole.
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        public async Task<IActionResult> DeleteRole(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    Errors(result);
            }
            else
                ModelState.AddModelError("", "No role found");
            return View("Index");
        }

        /// <summary>
        /// The Delete.
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    Errors(result);
            }
            else
                ModelState.AddModelError("", "User Not Found");
            return View("Index", _userManager.Users);
        }

        /// <summary>
        /// The Errors.
        /// </summary>
        /// <param name="result">The result<see cref="IdentityResult"/>.</param>
        private void Errors(IdentityResult result)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
    }
}
