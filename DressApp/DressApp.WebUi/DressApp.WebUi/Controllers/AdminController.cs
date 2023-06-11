using DressApp.WebUi.Data.Models;
using DressApp.WebUi.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DressApp.WebUi.Controllers
{
    [Authorize(Roles="admin")]
    public class AdminController : Controller
    {

        private UserManager<Identity.User> _userManager;
        private SignInManager<Identity.User> _signInManager;
        private RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<Identity.User> userManager, SignInManager<Identity.User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult RoleList()
        {
            return View(_roleManager.Roles);
        }

        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(model.RoleName));
                if (result.Succeeded)
                {
                    return RedirectToAction("RoleList");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);

                    }
                }
            }
            return View(model);
        }


        public async Task<IActionResult> RoleEdit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            var members = new List<User>();
            var nonmembers = new List<User>();

            foreach (var user in _userManager.Users.ToList())
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    members.Add(user);
                }
                else
                {
                    nonmembers.Add(user);
                }

            }
            var model = new RoleDetails()
            {
                Role = role,
                Members = members,
                NonMembers = nonmembers
            };
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> RoleEdit(RoleEditModel model)
        {
            if (ModelState.IsValid)
            {
                foreach (var userId in model.IdsToAdd ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        var result = await _userManager.AddToRoleAsync(user, model.RoleName);

                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }

                foreach (var userId in model.IdsToDelete ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        var result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);

                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }
            }
            return Redirect("/Admin/roleEdit/" + model.RoleId);


        }


        public IActionResult Users()
        {
            return View(_userManager.Users);
        }


        public async Task< IActionResult> UserEdit(string id)
        {
            var user=await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var selectedRole=await _userManager.GetRolesAsync(user);
                var roles=_roleManager.Roles.Select(i=>i.Name);

                ViewBag.Roles = roles;

                return View(new UserDetailModel()
                {
                    UserId= user.Id,
                    UserName=user.UserName,
                    Email=user.Email,
                    FirstName=user.FirstName,
                    LastName=user.LastName,
                    SelectedRoles=selectedRole
                });

            }
            return Redirect("~/admin/userList");
        }

        [HttpPost]
        public async Task<IActionResult> UserEdit(UserDetailModel model, string[] selectedRoles)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if(user!=null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.UserName = model.UserName;
                    user.Email = model.Email;

                    var result = await _userManager.UpdateAsync(user);

                    if(result.Succeeded)
                    {
                        var userRole = await _userManager.GetRolesAsync(user);
                        selectedRoles = selectedRoles ?? new string[] { };
                        await _userManager.AddToRolesAsync(user,selectedRoles.Except(userRole).ToArray<string>());
                        await _userManager.RemoveFromRolesAsync(user, userRole.Except(selectedRoles).ToArray<string>());

                        return Redirect("/admin/Users/");
                    }

                    return Redirect("/admin/Users/");
                }

                return View(model);
              

            }


            return View(model);
        }
    }
}
