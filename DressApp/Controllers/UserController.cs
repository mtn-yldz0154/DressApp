using DressApp.WebUi.Data.Models;
using DressApp.WebUi.Repository;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Threading.Tasks;
using X.PagedList;


namespace DressApp.WebUi.Controllers
{
	public class UserController : Controller
	{
		ProductRepository productRepository = new ProductRepository();
		SepetRepository sepetRepository = new SepetRepository();
		FavoriRepository favoriRepository = new FavoriRepository();
		Context db = new Context();

		//[AllowAnonymous]
		//[HttpGet]
		//public IActionResult Index()
		//{

		//	return View();
		//}
		//[HttpPost]
		//[AllowAnonymous]
		//public async Task<IActionResult> Index(User user)
		//{
		//	var usr = db.Users.FirstOrDefault(i => i.UserUserName == user.UserUserName && i.UserPassword == user.UserPassword);



		//	if (usr != null)
		//	{


		//		var cleims = new List<Claim>
		//		{
		//			new Claim(ClaimTypes.Name,user.UserUserName)
		//		};

		//		var useridentity = new ClaimsIdentity(cleims, "Login");
		//		ClaimsPrincipal principal = new ClaimsPrincipal(useridentity);
		//		await HttpContext.SignInAsync(principal);
		//		return RedirectToAction("Page", "User");

		//	}


		//	return View();
		//}




		//[HttpGet]
		//public async Task<IActionResult> LogOut()
		//{
		//	await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
		//	return RedirectToAction("Index", "User");
		//}

		public IActionResult Page(string q, int page=1)
		{
			if (!string.IsNullOrEmpty(q))
			{
				return View(productRepository.List(x => x.ProductName == q).ToPagedList(page, 9));
			}


			var kullanici = User.Identity.Name;
			ViewBag.Login = kullanici;

			var entity = db.Products.Where(x=>x.HomeIsAppovred==true).Include(i => i.Rayon).ToList();
			var productLikeModel = new List<ProductLikeModel>();
			ProductLikeModel productLikeModel1;

			foreach (var item in entity)
			{
				productLikeModel1 = new ProductLikeModel();

				productLikeModel1.ProductId = item.ProductId;
				productLikeModel1.ProductImageUrl = item.ProductImageUrl;
				productLikeModel1.StarNumber = item.StarNumber;
				productLikeModel1.RayonId = item.RayonId;
				productLikeModel1.ProductDescription = item.ProductDescription;
				productLikeModel1.ProductName = item.ProductName;
				productLikeModel1.SmallImageUrl = item.SmallImageUrl;
				productLikeModel1.LongProductName = item.LongProductName;
				productLikeModel1.ProductSize = item.ProductSize;
				productLikeModel1.ProductPrice = item.ProductPrice;
				

				productLikeModel.Add(productLikeModel1);


			}



			return View(productLikeModel.ToPagedList(page, 9));
		}

		public IActionResult Details(int id)
		{



			var entity = db.Products.Where(i => i.ProductId == id).Include(a => a.Rayon).FirstOrDefault();

			var products = db.Products.Where(i => i.ProductName.Contains(entity.ProductName) && i.ProductId!=entity.ProductId && i.RayonId==entity.RayonId).ToList();
			var onerilen = db.Products.Where(x => x.RayonId == entity.RayonId && x.ProductId!=entity.ProductId).ToList();

            var productModel = new ProductModel()
			{
				FavoriAppovred = entity.FavoriAppovred,
				ProductDescription = entity.ProductDescription,
				ProductName = entity.ProductName,
				ProductId = entity.ProductId,
				ProductImageUrl = entity.ProductImageUrl,
				ProductPrice = entity.ProductPrice,
				ProductSize = entity.ProductSize,
				ProductStock = entity.ProductStock,
				RayonId = entity.RayonId,
				SepetAppovred = entity.SepetAppovred,
				StarNumber = entity.StarNumber,
				LongProductName=entity.LongProductName,
				SmallImageUrl= entity.SmallImageUrl,
				Color= entity.Color,
				SalePrice= entity.SalePrice,
				Ozellik= entity.Ozellik,
				Kreaksiyon= entity.Kreaksiyon,
				Products =products,
				Onerilen=onerilen

			};

			return View(productModel);
		}

		public IActionResult ProductList(int id)
		{

                var entity = db.Products.Where(i => i.RayonId == id).ToList();
                var x = db.Rayons.Where(a => a.RayonId == id).Select(x => x.RayonName).FirstOrDefault();
                ViewBag.entity = x;

			 
           

            return View(entity);
		}

        public IActionResult SearchList(string q)
        {

            var entity = db.Products.Include(i=>i.Rayon).ToList();
          
            var product=new List<ProductSearchModel>();
           

            if (!string.IsNullOrEmpty(q))
            {
                

                entity = db.Products.Where(z => z.ProductName.Contains(q)).ToList();

				foreach (var item in entity)
				{
					  var search= new ProductSearchModel();
					
                    search.StarNumber = item.StarNumber;
					search.FavoriAppovred = item.FavoriAppovred;
					search.ProductPrice = item.ProductPrice;
					search.ProductName = item.ProductName;
					search.ProductId = item.ProductId;
					search.LongProductName = item.LongProductName;
					search.ProductImageUrl= item.ProductImageUrl;
					search.SmallImageUrl= item.SmallImageUrl;
					search.SepetAppovred= item.SepetAppovred;
					search.RayonId = item.RayonId;
					search.ProductSize = item.ProductSize;
					search.ProductStock= item.ProductStock;
					search.q= q;

					product.Add(search);

				}
            }



            return View(product);
        }

		public IActionResult Favoris()
		{
			var product = db.Products.Include(i => i.Rayon).Where(a=>a.StarNumber>60).ToList();


			return View(product);

		}

		public IActionResult Winter()
		{
			var product=db.Products.Include(i=>i.Rayon).Where(a=>a.Kreaksiyon=="Kış").ToList();

			return View(product);
		}
        public IActionResult Summer()
        {
            var product = db.Products.Include(i => i.Rayon).Where(a => a.Kreaksiyon == "Yaz").ToList();

            return View(product);
        }

		public IActionResult SaleProduct()
		{
			var product=db.Products.Include(i=>i.Rayon).Where(a=>a.SalePrice>0).ToList();

			return View(product);
		}






    }
}