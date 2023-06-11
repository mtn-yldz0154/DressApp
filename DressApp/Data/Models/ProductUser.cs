using System.Collections.Generic;

namespace DressApp.WebUi.Data.Models
{
    public class ProductUser
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public List<ProductUserItem> productUserItems { get; set; }
    }
}
