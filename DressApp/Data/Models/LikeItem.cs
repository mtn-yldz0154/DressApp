namespace DressApp.WebUi.Data.Models
{
    public class LikeItem
    {


        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int LikeId { get; set; }
        public Like Like { get; set; }

        public int Quantity { get; set; }


    }
}
