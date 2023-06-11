using DressApp.WebUi.Data.Models;
using DressApp.WebUi.Identity;
using DressApp.WebUi.Migrations;
using DressApp.WebUi.Repository;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Providers.Entities;
using User = DressApp.WebUi.Identity.User;

namespace DressApp.WebUi.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        CartRepository cartRepository=new CartRepository();
        OrderRepository orderRepository=new OrderRepository();
        private UserManager<User> _usermanager;
        Context db=new Context();

        public CartController(UserManager<User> usermanager)
        {
            _usermanager = usermanager;

        }
    
        public IActionResult Index()
        {
            var cart = cartRepository.GetCart(_usermanager.GetUserId(User));
            return View(new CartModel()
            {
                CartId = cart.Id,
                CartItems = cart.CartItems.Select(i => new CartItemModel()
                {
                    CartItemId = i.Id,
                    ProductId = i.ProductId,
                    Name = i.Product.ProductName,
                    Price = (double)i.Product.ProductPrice,
                    ImageUrl = i.Product.ProductImageUrl,
                    Quantity = i.Quantity,

                }).ToList()
            });
        }

        public IActionResult AddToCart(int productId, int quantity=1)
        {

            var entity=db.Products.Where(i=>i.ProductId==productId).FirstOrDefault();
            entity.StarNumber += 1;
            db.SaveChanges();
            
            var userId = _usermanager.GetUserId(User);
            cartRepository.AddToCart(userId, productId, quantity);
            TempData["message"] = $"{entity.ProductName}";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteFromCart(int productId)
        {
            var entity = db.Products.Where(i => i.ProductId == productId).FirstOrDefault();
            var userId = _usermanager.GetUserId(User);

            cartRepository.DeleteFromCart(userId, productId);

            TempData["messages"] = $"{entity.ProductName}";
            return RedirectToAction("Index");
        }

        public IActionResult Checkout()
        {
            var cart = cartRepository.GetCart(_usermanager.GetUserId(User));

            var orderModel = new OrderModel();

            orderModel.CartModel = new CartModel()
            {
                CartId = cart.Id,
                CartItems = cart.CartItems.Select(i => new CartItemModel()
                {
                    CartItemId = i.Id,
                    ProductId = i.ProductId,
                    Name = i.Product.ProductName,
                    Price = (double)i.Product.ProductPrice,
                    ImageUrl = i.Product.ProductImageUrl,
                    Quantity = i.Quantity,


                }).ToList()
            };

            return View(orderModel);
        }


        [HttpPost]
        public IActionResult Checkout(OrderModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = _usermanager.GetUserId(User);
                var cart = cartRepository.GetCart(userId);
                

               

                model.CartModel = new CartModel()
                {
                    CartId = cart.Id,
                   
                    CartItems = cart.CartItems.Select(i => new CartItemModel()
                    {
                        CartItemId = i.Id,
                        ProductId = i.ProductId,
                        Name = i.Product.ProductName,
                        Price = (double)i.Product.ProductPrice,
                        ImageUrl = i.Product.ProductImageUrl,
                        StarNumber=i.Product.StarNumber,
                        
                        Quantity = i.Quantity


                        
                    }).ToList()
                };
           
               
                
                
                

                

                var payment = PaymentProcess(model);

                if (payment.Status == "success")
                {
                    foreach (var item in model.CartModel.CartItems)
                    {
                       
                       var product= db.Products.Where(i => i.ProductId == item.ProductId).FirstOrDefault();
                        product.StarNumber += 2;
                        db.SaveChanges();
                        
                    }
                    SaveOrder(model, payment, userId);
                    ClearCart(model.CartModel.CartId);

                   
                    
                    
                    return View("Succes");
                }
                else
                {

                    return View();
                }
            }
            return View(model);
       
        }


        public IActionResult GetOrders(string userId)
        {
            var user=_usermanager.GetUserId(User);
            var cart = cartRepository.GetCart(_usermanager.GetUserId(User));
            var orders = orderRepository.GetOrders(user);

            var orderListModel=new List<OrderListModel>();
            OrderListModel _orderListModel;

            foreach (var item in orders)
            {
                _orderListModel = new OrderListModel();

                _orderListModel.OrderNumber= item.OrderNumber;
                _orderListModel.Address= item.Address;
                _orderListModel.OrderId = item.Id;
                _orderListModel.Email= item.Email;
                _orderListModel.City= item.City;
                 _orderListModel.OrderDate= item.OrderDate;
                _orderListModel.Note= item.Note;
                _orderListModel.FirstName= item.FirstName;
                _orderListModel.LastName= item.LastName;
                _orderListModel.Phone= item.Phone;
                _orderListModel.Sayac = 1;
                _orderListModel.OrderCount = db.Orders.Count();




                _orderListModel.OrderItems = item.OrderItems.Select(i => new OrderItemModel()
                {
                    OrderItemId = i.Id,
                    Name = i.Product.ProductName,
                    Price = (double)i.Price,
                    Quantity = i.Quantity,
                    ImageUrl=i.Product.ProductImageUrl
               
                }).ToList();

                _orderListModel.CartModel = new CartModel()
                {
                    CartId = cart.Id,
                    CartItems = cart.CartItems.Select(i => new CartItemModel()
                    {
                        CartItemId = i.Id,
                        ProductId = i.ProductId,
                        Name = i.Product.ProductName,
                        Price = (double)i.Product.ProductPrice,
                        ImageUrl = i.Product.ProductImageUrl,
                        Quantity = i.Quantity
                    }).ToList()
                };
                orderListModel.Add(_orderListModel);


            }
            
    


            return View(orderListModel);
        }


        private void ClearCart(int cartId)
        {
            cartRepository.ClearCart(cartId);
        }

       

        private void SaveOrder(OrderModel model, Payment payment, string userId)
        {
            var order = new Data.Models.Order();

            order.OrderNumber = new Random().Next(111111, 999999).ToString();
            order.OrderState = EnumOrderState.completed;
            order.PaymentType = EnumPaymentType.CreditCard;
            order.PaymentId = payment.PaymentId;
            order.ConversationId = payment.ConversationId;
            order.OrderDate = new DateTime();
            order.FirstName = model.FirstName;
            order.LastName = model.LastName;
            order.UserId = userId;
            order.Address = model.Address;
            order.Phone = model.Phone;
            order.Email = model.Email;
            order.City = model.City;
            order.Note = model.Note;

            order.OrderItems = new List<Data.Models.OrderItem>();

            foreach (var item in model.CartModel.CartItems)
            {
                var orderItem = new Data.Models.OrderItem()
                {
                    Price = item.Price,
                    Quantity = item.Quantity,
                    ProductId = item.ProductId
                };
                order.OrderItems.Add(orderItem);
            }
            orderRepository.Create(order);
        }

       

        private Payment PaymentProcess(OrderModel model)
        {
            Options options = new Options();
            options.ApiKey = "sandbox-JCaQB0rkV1U6vJz0maqIFf8Wystgk9Px";
            options.SecretKey = "sandbox-j8RVrhhyvqrteD5PqAZOy4PCdcZq5lQB";
            options.BaseUrl = "https://sandbox-api.iyzipay.com";

            CreatePaymentRequest request = new CreatePaymentRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = new Random().Next(111111111, 999999999).ToString();
            request.Price = model.CartModel.TotalPrice().ToString();
            request.PaidPrice = model.CartModel.TotalPrice().ToString();
            request.Currency = Currency.TRY.ToString();
            request.Installment = 1;
            request.BasketId = "B67832";
            request.PaymentChannel = PaymentChannel.WEB.ToString();
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();

            PaymentCard paymentCard = new PaymentCard();
            paymentCard.CardHolderName = model.CardName;
            paymentCard.CardNumber = model.CardNumber;
            paymentCard.ExpireMonth = model.ExpirationMonth;
            paymentCard.ExpireYear = model.ExpirationYear;
            paymentCard.Cvc = model.Cvc;
            paymentCard.RegisterCard = 0;
            request.PaymentCard = paymentCard;

            //paymentCard.CardNumber = "5528790000000008";
            //paymentCard.ExpireMonth = "12";
            //paymentCard.ExpireYear = "2030";
            //paymentCard.Cvc = "123";

            Buyer buyer = new Buyer();
            buyer.Id = "BY789";
            buyer.Name = model.FirstName;
            buyer.Surname = model.LastName;
            buyer.GsmNumber = "+905350000000";
            buyer.Email = "email@email.com";
            buyer.IdentityNumber = "74300864791";
            buyer.LastLoginDate = "2015-10-05 12:43:35";
            buyer.RegistrationDate = "2013-04-21 15:12:09";
            buyer.RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            buyer.Ip = "85.34.78.112";
            buyer.City = "Istanbul";
            buyer.Country = "Turkey";
            buyer.ZipCode = "34732";
            request.Buyer = buyer;

            Address shippingAddress = new Address();
            shippingAddress.ContactName = "Jane Doe";
            shippingAddress.City = "Istanbul";
            shippingAddress.Country = "Turkey";
            shippingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            shippingAddress.ZipCode = "34742";
            request.ShippingAddress = shippingAddress;

            Address billingAddress = new Address();
            billingAddress.ContactName = "Jane Doe";
            billingAddress.City = "Istanbul";
            billingAddress.Country = "Turkey";
            billingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            billingAddress.ZipCode = "34742";
            request.BillingAddress = billingAddress;

            List<BasketItem> basketItems = new List<BasketItem>();
            BasketItem basketItem;

            foreach (var item in model.CartModel.CartItems)
            {
                basketItem = new BasketItem();
                basketItem.Id = item.ProductId.ToString();
                basketItem.Name = item.Name;
                basketItem.Category1 = "Kıyafet";
                basketItem.ItemType = BasketItemType.PHYSICAL.ToString();
                basketItem.Price = item.Price.ToString();

                basketItems.Add(basketItem);
            }



            request.BasketItems = basketItems;

            return Payment.Create(request, options);

        }



   



    }
}
