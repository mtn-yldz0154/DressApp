
using System.Collections.Generic;
using System;
using System.Linq;

namespace DressApp.WebUi.Data.Models
{
    public class OrderListModel
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string UserId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }

        public int Sayac { get; set; }


        public EnumPaymentType PaymentType { get; set; }
        public EnumOrderState OrderState { get; set; }

        public List<OrderItemModel> OrderItems { get; set; }

        public CartModel CartModel { get; set; }


        public double TotalPrice()
        {
            return OrderItems.Sum(i => i.Price * i.Quantity);
        }
        public int OrderCount { get; set; }
        public int CountOrderItems()
        {
            return OrderItems.Count();
        }
    }
}
