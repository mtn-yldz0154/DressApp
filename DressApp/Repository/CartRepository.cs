using DressApp.WebUi.Data.Models;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DressApp.WebUi.Repository
{
    public class CartRepository : GenericRepository<Cart>
    {

        public void InitilazerCart(string userId)
        {
            using (var context = new Context())
            {

                context.Add(new Cart { UserId = userId });
                context.SaveChanges();

            }
        }

        public Cart GetCart(string userId)
        {
            using (var context = new Context())
            {

                return context.Carts.Include(i => i.CartItems).ThenInclude(i => i.Product).FirstOrDefault(i => i.UserId == userId);

            }
        }

        public void AddToCart(string userId, int productId, int quantity)
        {
            using (var context = new Context())
            {
                var cart = GetCart(userId);

                if (cart != null)
                {
                    var index = cart.CartItems.FindIndex(i => i.ProductId == productId);

                    if (index < 0)
                    {
                        cart.CartItems.Add(new CartItem
                        {
                            CartId = cart.Id,
                            ProductId = productId,
                            Quantity = quantity

                        });
                    }
                    else
                    {
                        cart.CartItems[index].Quantity += quantity;
                    }

                     TUpdate(cart);
                    context.SaveChanges();

                }

            }



        }

        public override void TUpdate(Cart entity)
        {
            using(var context = new Context())
            {
                context.Carts.Update(entity);

                context.SaveChanges();

            }

        }

        public void DeleteFromCart(string userId,int productId)
        {
            using (var context = new Context())
            {
                var cart= GetCart(userId);
                if (cart != null)
                {
                    //var cmd = @"delete from CartItems where CartId=@p0 and ProductId=@p1";
                    //context.Database.ExecuteSqlRaw(cmd, cart.Id, productId);
                    var entity=cart.CartItems.Where(i => i.CartId==cart.Id && i.ProductId==productId).FirstOrDefault();
                    context.CartItems.Remove(entity);
                    context.SaveChanges();
                    
                }
              
            }

        }


        public void ClearCart(int cartId)
        {
           using (var context = new Context()) 
            {
                var cmd = @"delete from CartItems where CartId=@p0";
                context.Database.ExecuteSqlRaw(cmd, cartId);

                context.SaveChanges();

            }


        }

    }

   
}
