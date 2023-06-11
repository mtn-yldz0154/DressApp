using DressApp.WebUi.Data.Models;
using DressApp.WebUi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using X.PagedList;

namespace DressApp.WebUi.Controllers
{
    [Authorize(Roles = "admin")]

    public class ProductController : Controller
    {
        ProductRepository productRepository = new ProductRepository();
        SepetRepository sepetRepository = new SepetRepository();
        Context db = new Context();

        public IActionResult Index()
        {
            var kullanici = User.Identity.Name;
            ViewBag.Login = kullanici;


            var values = productRepository.TList("Rayon");

            return View(values);
        }

        [HttpGet]
        public IActionResult ProductAdd()
        {

            List<SelectListItem> values = (from i in db.Rayons.ToList()
                                           select new SelectListItem
                                           {
                                               Text = i.RayonName,
                                               Value = i.RayonId.ToString()
                                           }).ToList();
            ViewBag.Values = values;

            return View();
        }
        [HttpPost]
        public IActionResult ProductAdd(Product product, IFormFile file,IFormFile files)
        {

            product.ProductImageUrl = file.FileName;
            product.SmallImageUrl = files.FileName;
            productRepository.TAdd(product);
            return RedirectToAction("Index", "Product");
        }

        public IActionResult ProductDelete(int id)
        {
            var kullanici = User.Identity.Name;
            ViewBag.Login = kullanici;
            productRepository.TDelete(new Product { ProductId = id });
            return RedirectToAction("Index", "Product");
        }

        [HttpGet]
        public IActionResult ProductUpdate(int id)
        {
         

            List<SelectListItem> values = (from y in db.Rayons.ToList()
                                           select new SelectListItem
                                           {
                                               Text = y.RayonName,
                                               Value = y.RayonId.ToString()
                                           }).ToList();
            ViewBag.Values = values;
            var x = productRepository.TGet(id);

            Product product = new Product()
            {
                ProductId = x.ProductId,
                RayonId = x.RayonId,
                ProductDescription = x.ProductDescription,
                ProductImageUrl = x.ProductImageUrl,
                ProductName = x.ProductName,
                ProductPrice = x.ProductPrice,
                ProductSize = x.ProductSize,
                ProductStock = x.ProductStock,
                Color= x.Color,
                LongProductName= x.LongProductName,
                Ozellik = x.Ozellik,
                SalePrice= x.SalePrice,
                SmallImageUrl= x.SmallImageUrl,
                Kreaksiyon= x.Kreaksiyon
            };
         

            return View(product);
        }

        [HttpPost]
        public IActionResult ProductUpdate(Product product, IFormFile file, IFormFile files)
        {


            var entity = productRepository.TGet(product.ProductId);

            entity.ProductStock = product.ProductStock;
            entity.ProductName = product.ProductName;
            entity.ProductPrice = product.ProductPrice;
            entity.ProductSize = product.ProductSize;
            entity.ProductDescription = product.ProductDescription;
            entity.ProductId = product.ProductId;
            entity.RayonId = product.RayonId;
            entity.Ozellik=product.Ozellik;
            entity.Color= product.Color;
            entity.SalePrice= product.SalePrice;
            entity.Kreaksiyon= product.Kreaksiyon;


            if (file != null)
            {
                entity.ProductImageUrl = file.FileName;
            }
            if (files != null)
            {
                entity.SmallImageUrl = files.FileName;
            }


            productRepository.TUpdate(entity);

            


            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var kullanici = User.Identity.Name;
            ViewBag.Login = kullanici;

            var urun = db.Products.Where(a => a.ProductId == id).
                Include(i => i.Rayon).FirstOrDefault();



            return View(urun);
        }

    }
}
