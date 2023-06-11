namespace DressApp.WebUi.Data.Models
{
    public class ProductUserItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string LongProductName { get; set; }
        public string ProductDescription { get; set; }
        public double ProductPrice { get; set; }
        public int ProductStock { get; set; }
        public string ProductSize { get; set; }
        public string ProductImageUrl { get; set; }
        public string SmallImageUrl { get; set; }
        public bool FavoriAppovred { get; set; }
        public bool SepetAppovred { get; set; }
        public bool LikeApovred { get; set; }

        public int StarNumber { get; set; }

        public int RayonId { get; set; }
        public virtual Rayon Rayon { get; set; }
    }
}
