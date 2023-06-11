using System.ComponentModel.DataAnnotations;

namespace DressApp.WebUi.Data.Models
{
    public class Sepet
    {
        [Key]
        public int SepetId { get; set; }
        public int UrunSayisi { get; set; }
        public string SepetName { get; set; }
        public string SepetImageUrl { get; set; }
        public double Sepetprice { get; set; }


    }
}
