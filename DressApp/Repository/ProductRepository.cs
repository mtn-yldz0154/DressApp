using DressApp.WebUi.Data.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DressApp.WebUi.Repository
{
    public class ProductRepository:GenericRepository<Product>
    {
        
        public List<Product> Search(string q)
        {
            using (var context = new Context())
            {
                var product=context.Products.Include(a=>a.Rayon).Where(i=>i.ProductName.Contains(q)).ToList();

                return product.ToList();
            }
        }

    }
}
