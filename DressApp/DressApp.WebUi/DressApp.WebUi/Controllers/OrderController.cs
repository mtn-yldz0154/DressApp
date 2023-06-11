using DressApp.WebUi.Data.Models;
using DressApp.WebUi.Identity;
using DressApp.WebUi.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DressApp.WebUi.Controllers
{
    public class OrderController : Controller
    {
        CartRepository cartRepository = new CartRepository();
        OrderRepository orderRepository = new OrderRepository();
        private UserManager<User> _usermanager;

        public OrderController(UserManager<User> usermanager)
        {
            _usermanager = usermanager;

        }
        public IActionResult Checkout()
        {
            //var cart = cartRepository.GetCart(_usermanager.GetUserId(User));

            //var orderModel = new OrderModel();

            //orderModel.CartModel = new DressApp.WebUi.Data.Models.CartModel()
            //{
            //    CartId = cart.Id,
            //    CartItems = cart.CartItems.Select(i => new CartItemModel()
            //    {
            //        CartItemId = i.Id,
            //        ProductId = i.ProductId,
            //        Name = i.Product.ProductName,
            //        Price = (double)i.Product.ProductPrice,
            //        ImageUrl = i.Product.ProductImageUrl,
            //        Quantity = i.Quantity,


            //    }).ToList()
            //};

            return View();
        }
    }
}
