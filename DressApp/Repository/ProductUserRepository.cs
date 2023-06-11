using DressApp.WebUi.Data.Models;

namespace DressApp.WebUi.Repository
{
    public class ProductUserRepository:GenericRepository<ProductUser>
    {
        public void InitilazerProduct(string userId)
        {
            using (var context = new Context())
            {

                context.Add(new ProductUser { UserId = userId });
                context.SaveChanges();

            }
        }
    }
}
