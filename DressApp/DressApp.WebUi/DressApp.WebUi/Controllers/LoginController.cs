using DressApp.WebUi.Data.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Providers.Entities;


namespace DressApp.WebUi.Controllers
{

    public class LoginController : Controller
    {

      

        Context db = new Context();
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
  

            return View();
        }
        [HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> Index(Admin admin)
		{
            var adm = db.Admins.FirstOrDefault(i => i.AdminUserName == admin.AdminUserName && i.AdminPassword == admin.AdminPassword);
            

            if (adm != null)
            {
             

                var cleims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,admin.AdminUserName)
                };

                var useridentity = new ClaimsIdentity(cleims, "Login");
                ClaimsPrincipal principal = new ClaimsPrincipal(useridentity);
                await HttpContext.SignInAsync(principal);
                return RedirectToAction("Index", "Rayon");

            }

          
            return View();
		}
    
   


        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }
	}
}
