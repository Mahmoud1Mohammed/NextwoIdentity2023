using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NextwoIdentity.Models.ViewModels;
using NextwoIdentity2023.Models.ViewModels;
using System.Data;

namespace NextwoIdentity2023.Controllers
{
    [Authorize]

    public class AccountController : Controller
    {
        private  SignInManager<IdentityUser> signInManager;
        private  RoleManager<IdentityRole> roleManager;

        private UserManager<IdentityUser> UserManager { get; }

        public AccountController(UserManager<IdentityUser> _userManager,SignInManager<IdentityUser> _signInManager,RoleManager<IdentityRole>_roleManager)
        {
            UserManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;
        }

        #region Users
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]

        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid) 
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    PhoneNumber = model.Phone

                };
                var result = await UserManager.CreateAsync(user, model.Password!);
                if (result.Succeeded) 
                {
                    return RedirectToAction("Login", "Account");
                }
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError(err.Code, err.Description);

                }
                return View(model);
            }
            return View(model);

            
        }
        [AllowAnonymous]

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]

        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

                var result = await signInManager.PasswordSignInAsync(model.Email!, model.Password!, false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                  
                    ModelState.AddModelError("", "Invalid User Or Password");
                return View(model);
            }
            return View(model);


        }
       
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRole(CreteRoleViewModel model)
        {

            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole
                {
                    Name = model.RoleName
                };
                var result = await roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("RolesList");
                }
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError(err.Code, err.Description);
                }
                return View(model);
            }
            return View(model);
        }
        [Authorize(Roles = "Admin")]

        public IActionResult RolesList()
        {
            return View(roleManager.Roles);
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        #endregion

        [HttpGet]
        public async  Task<IActionResult> EditRole(string id) 
        {
            if (id == null)
            {
                return RedirectToAction("RolesList");
            }
            var role = await roleManager.FindByIdAsync(id);
            if (role == null) 
            {
                return RedirectToAction("RolesList");

            }
            EditRoleViewModel model = new EditRoleViewModel 
            { RoleId = role.Id, RoleName = role.Name };
            foreach (var user in UserManager.Users)
            {
                if (await UserManager.IsInRoleAsync(user , role.Name!))

                {
                    model.Users!.Add(user.UserName!);
                }
            }

            return View(model);
        }
        [HttpPost]

        public async Task<IActionResult> EditRole(EditRoleViewModel modle)
        {
            if (!ModelState.IsValid)
            {
                    var role = await roleManager.FindByIdAsync(modle.RoleId!);
                    if (role == null)
                    {
                        return RedirectToAction(nameof(ErrorPage));

                    }
                    role.Name = modle.RoleName;
                    var result = await roleManager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(RolesList));

                    }
                    foreach (var err in result.Errors)
                    {
                        ModelState.AddModelError(err.Code, err.Description);
                    }
                    return View(modle); 

            }
            return View(modle);

        }
        public IActionResult ErrorPage()
        {
            return View();
        }

        public async Task<IActionResult> ModifiyUsersInRole( string id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(RolesList));

            }
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return RedirectToAction(nameof(ErrorPage));

            }
           List <UserRoleViewModel> models = new List <UserRoleViewModel>();
            foreach (var user in UserManager.Users)
            {
                UserRoleViewModel userRole = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                };
                if (await UserManager.IsInRoleAsync(user, role.Name!))
                {
                    userRole.IsSelected = true;
                }
                else
                {
                    userRole.IsSelected= false;
                }
                models.Add(userRole);
            }
            return View(models);
        }


        [HttpPost]
        public async Task<IActionResult> ModifyUsersInRole(string id, List<UserRoleViewModel> models)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(RolesList));
            }
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return RedirectToAction(nameof(ErrorPage));
            }
            IdentityResult result = new IdentityResult();
            for (int i = 0; i < models.Count; i++)
            {

                var user = await UserManager.FindByIdAsync(models[i].UserId!);
                if (models[i].IsSelected && (!await UserManager.IsInRoleAsync(user!, role.Name!)))
                {
                    result = await UserManager.AddToRoleAsync(user!, role.Name!);
                }
                else if (!models[i].IsSelected && (await UserManager.IsInRoleAsync(user!, role.Name!)))
                {
                    result = await UserManager.RemoveFromRoleAsync(user!, role.Name!);
                }

            }
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(RolesList));
            }
            return View(models);

        }
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");

        }

    }
}
