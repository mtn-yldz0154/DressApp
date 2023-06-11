using DressApp.WebUi.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DressApp.WebUi.Repository
{
    public class LikeRepository:GenericRepository<Like>
    {
        public void InitilazerLike(string userId)
        {
            using (var context = new Context())
            {

                context.Add(new Like { UserId = userId });
                context.SaveChanges();

            }
        }

        public Like GetLike(string userId)
        {
            using (var context = new Context())
            {

                return context.Likes.Include(i => i.LikeItems).ThenInclude(i => i.Product).FirstOrDefault(i => i.UserId == userId);

            }
        }

        public void AddToLike(string userId, int productId, int quantity)
        {
            using (var context = new Context())
            {
                var like = GetLike(userId);

                if (like != null)
                {
                    var index = like.LikeItems.FindIndex(i => i.ProductId == productId);

                    if (index < 0)
                    {
                        like.LikeItems.Add(new LikeItem
                        {
                            LikeId = like.Id,
                            ProductId = productId,
                            Quantity = quantity

                        });
                    }
                    else
                    {
                        like.LikeItems[index].Quantity += quantity;
                    }

                    TUpdate(like);
                    context.SaveChanges();

                }

            }



        }

        public override void TUpdate(Like entity)
        {
            using (var context = new Context())
            {
                context.Likes.Update(entity);

                context.SaveChanges();

            }

        }

        public void DeleteFromLike(string userId, int productId)
        {
            using (var context = new Context())
            {
                var like = GetLike(userId);
                if (like != null)
                {
                    //var cmd = @"delete from LikeItems where LikeId=@p0 and ProductId=@p1";
                    //context.Database.ExecuteSqlRaw(cmd, like.Id, productId);
                    var entity = like.LikeItems.Where(i => i.LikeId == like.Id && i.ProductId == productId).FirstOrDefault();
                    context.LikeItems.Remove(entity);
                    context.SaveChanges();

                }

            }

        }


        //public void ClearCart(int cartId)
        //{
        //    using (var context = new Context())
        //    {
        //        var cmd = @"delete from CartItems where CartId=@p0";
        //        context.Database.ExecuteSqlRaw(cmd, cartId);

        //        context.SaveChanges();

        //    }


        //}




    }
}
