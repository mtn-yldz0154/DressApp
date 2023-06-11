using DressApp.WebUi.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace DressApp.WebUi.Repository
{
    public class SepetRepository:GenericRepository<Sepet>
    {
        public void  UpdatePrice(Sepet x)
        {
            using (var context = new Context())
            {
                var entity = context.Sepets.Where(a=>a.UrunSayisi==x.UrunSayisi).ToList();
                foreach (var item in entity)
                {
                    item.Sepetprice = x.Sepetprice;
                }

                
            }
        }



    }
}
