using System.Collections.Generic;
using System.Linq;

namespace DressApp.WebUi.Data.Models
{
    public class LikeModel
    {
        public int LikeId { get; set; }
        public List<LikeItemModel> LikeItems { get; set; }

        public double TotalPrice()
        {

            return LikeItems.Sum(i => i.Quantity * i.ProductPrice);

        }

        public int Count()
        {
            return LikeItems.Count();
        }



    }

    public class LikeItemModel
    {

        public int LikeItemId { get; set; }
        public int ProductId { get; set; }
        public string ProductSmallName { get; set; }
        public string ProductSmallImage { get; set; }
        public double ProductPrice { get; set; }
        public string ProductSize { get; set; }
        public int Quantity { get; set; }


    }
}
