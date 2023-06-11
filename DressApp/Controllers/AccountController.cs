using DressApp.WebUi.Data.Models;
using DressApp.WebUi.Identity;
using DressApp.WebUi.Repository;
using Iyzipay.Model.V2.Subscription;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DressApp.WebUi.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<Identity.User> _userManager;
        private SignInManager<Identity.User> _signInManager;
        private RoleManager<IdentityRole> _roleManager;
        CartRepository cartRepository=new CartRepository();
        LikeRepository likeRepository=new LikeRepository();
        ProductUserRepository userRepository=new ProductUserRepository();
      

        public AccountController(UserManager<Identity.User> userManager, SignInManager<Identity.User> signInManager,RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
           
        }
    
        public IActionResult Login(string ReturnUrl=null)
        {
            return View(new LoginModel()
            {
                ReturnUrl = ReturnUrl
            });
        }

        [HttpPost]
     
        public async Task<IActionResult> Login(LoginModel model)
		{
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var user=await  _userManager.FindByNameAsync(model.UserName);
            if(user==null)
            {
                ModelState.AddModelError("", "Kullanıcı Bulunamadı");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user,model.Password,true,false);

            if(result.Succeeded)
            {

                TempData["message"] = $"{user.UserName}";

                return Redirect("~/");
            }

			return View(model);
		}

		public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
       
        public async Task< IActionResult> Register(RegisterModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new Identity.User()
            {
                FirstName= model.FirstName,
                LastName= model.LastName,  
                Email= model.Email,
                UserName= model.UserName,
                

            };
            var result=await _userManager.CreateAsync(user,model.Password);

            if(result.Succeeded)
            {
                ////Cart objesi olustur
                ///Like objesi olustur
                userRepository.InitilazerProduct(user.Id);
                cartRepository.InitilazerCart(user.Id);
                likeRepository.InitilazerLike(user.Id);
             

                //generite token
                return RedirectToAction("login", "account");
            }
            ModelState.AddModelError("", "Bilinmeyen Bir Hata Oluştu Tekrar Deneyiniz");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Redirect("~/");
        }

        public IActionResult AccesDenied()
        {
            return View();
        }

      

        //public IActionResult RoleList()
        //{
        //    return View(_roleManager.Roles);
        //}

        //public IActionResult CreateRole()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> CreateRole(RoleModel model)
        //{
        //    if(ModelState.IsValid)
        //    {
        //        var result = await _roleManager.CreateAsync(new IdentityRole(model.RoleName));
        //        if(result.Succeeded)
        //        {
        //            return RedirectToAction("RoleList");
        //        }
        //        else
        //        {
        //            foreach (var error in result.Errors)
        //            {
        //                ModelState.AddModelError("", error.Description);

        //            }
        //        }
        //    }

        //    return View(model);
        //}


        //public async Task< IActionResult> RoleEdit(string id)
        //{
        //    var role = await _roleManager.FindByIdAsync(id);

        //    var members = new List<User>();
        //    var nonmembers = new List<User>();

        //    foreach (var user in _userManager.Users.ToList())
        //    {
        //       if(await _userManager.IsInRoleAsync(user, role.Name))
        //        {
        //            members.Add(user);
        //        }
        //        else
        //        {
        //            nonmembers.Add(user);
        //        }

        //    }
        //    var model = new RoleDetails()
        //    {
        //        Role = role,
        //        Members = members,
        //        NonMembers = nonmembers
        //    };
        //    return View(model);
        //}


        //[HttpPost]
        //public async Task< IActionResult> RoleEdit(RoleEditModel model)
        //{
        //    if(ModelState.IsValid)
        //    {
        //        foreach (var userId in model.IdsToAdd??new string[] {})
        //        {
        //            var user = await _userManager.FindByIdAsync(userId);
        //            if(user!=null)
        //            {
        //                var result = await _userManager.AddToRoleAsync(user, model.RoleName);

        //                if(!result.Succeeded)
        //                {
        //                    foreach (var error in result.Errors)
        //                    {
        //                        ModelState.AddModelError("", error.Description);
        //                    }
        //                }
        //            }
        //        }

        //        foreach (var userId in model.IdsToDelete ?? new string[] { })
        //        {
        //            var user = await _userManager.FindByIdAsync(userId);
        //            if (user != null)
        //            {
        //                var result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);

        //                if (!result.Succeeded)
        //                {
        //                    foreach (var error in result.Errors)
        //                    {
        //                        ModelState.AddModelError("", error.Description);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return Redirect("/account/roleEdit/" + model.RoleId);


        //}


    }
}
