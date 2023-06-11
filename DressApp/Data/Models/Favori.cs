using System.ComponentModel.DataAnnotations;

namespace DressApp.WebUi.Data.Models
{
    public class Favori
    {
        [Key]
        public int FavoriId { get; set; }
        public string FavoriName { get; set; }
        public string FavoriImageUrl { get; set; }
        public double FavoriPrice { get; set; }
        public int UrunSayisi { get; set; }
    }
}
