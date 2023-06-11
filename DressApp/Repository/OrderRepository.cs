using DressApp.WebUi.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DressApp.WebUi.Repository
{
    public class OrderRepository:GenericRepository<Order>
    {

      public void Create(Order entity)
        {

            TAdd(entity);
        }

     public List<Order> GetOrders(string userId)
        {
            using (var context = new Context())
            {
                var orders = context.Orders.Include(i => i.OrderItems)
                    .ThenInclude(i => i.Product).
                    AsQueryable();


                if(!string.IsNullOrEmpty(userId))
                {
                    orders=orders.Where(i=>i.UserId == userId);
                }

                return orders.ToList();

            }


        }


    }
}
