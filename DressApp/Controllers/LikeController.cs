using DressApp.WebUi.Data.Models;
using DressApp.WebUi.Identity;
using DressApp.WebUi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DressApp.WebUi.Controllers
{
    [Authorize]
    public class LikeController : Controller
    {
        LikeRepository likeRepository=new LikeRepository();
        UserManager<User> _userManager;
        Context db=new Context();
        public LikeController(UserManager<User> userManager)
        {
            _userManager = userManager;

        }

        public IActionResult Index()
        {
           var like= likeRepository.GetLike(_userManager.GetUserId(User));


            return View(new LikeModel()
            {
                LikeId=like.Id,
                
                LikeItems=like.LikeItems.Select(i=> new LikeItemModel()
                {
                    LikeItemId=i.Id,
                    ProductId=i.ProductId,
                    ProductPrice=(double)i.Product.ProductPrice,
                    ProductSmallImage=i.Product.SmallImageUrl,
                    ProductSmallName=i.Product.ProductName,
                    ProductSize=i.Product.ProductSize,
                    Quantity=i.Quantity
                }).ToList()
            });
        }

        [HttpPost]
        public IActionResult AddToLike(int productId, int quantity)
        {
         


            var product=  db.Products.Where(i => i.ProductId == productId).FirstOrDefault();
            if (product != null)
            {
                product.StarNumber+=1;
                db.SaveChanges();
            }


            var userId = _userManager.GetUserId(User);
            likeRepository.AddToLike(userId, productId, quantity);

            TempData["message"] = $"{product.ProductName}";

            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult DeleteFromLike (int productId)
        {
            var product = db.Products.Where(i => i.ProductId == productId).FirstOrDefault();
            var userId = _userManager.GetUserId(User);

            likeRepository.DeleteFromLike(userId, productId);

            TempData["messages"] = $"{product.ProductName}";

            return RedirectToAction("Index");
        }
    }
}
