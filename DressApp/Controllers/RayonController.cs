using DressApp.WebUi.Data.Models;
using DressApp.WebUi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DressApp.WebUi.Controllers
{
    [Authorize(Roles = "admin")]
    public class RayonController : Controller
    {
        RayonRepository rayonRepository = new RayonRepository();
        Context db=new Context();
        public IActionResult Index()
        {
            var kullanici = User.Identity.Name;
            ViewBag.Login = kullanici;

            return View(rayonRepository.TList());
        }

        [HttpGet]
        public IActionResult RayonAdd()
        {
            var kullanici = User.Identity.Name;
            ViewBag.Login = kullanici;

            return View();
        }

        [HttpPost]
        public IActionResult RayonAdd(Rayon rayon)
        {
            var kullanici = User.Identity.Name;
            ViewBag.Login = kullanici;

            if (!ModelState.IsValid)
            {
                return View("RayonAdd");
            }
            rayonRepository.TAdd(rayon);
            return RedirectToAction("Index");
        }

        [HttpGet]   
        public IActionResult RayonUpdate(int id)
        {
            var kullanici = User.Identity.Name;
            ViewBag.Login = kullanici;

            var x=rayonRepository.TGet(id);
            Rayon rayon = new Rayon()
            {

                RayonName = x.RayonName,
                RayonDescription = x.RayonDescription,
                RayonId = x.RayonId
            };

            return View(rayon);
        }
        [HttpPost]
        public IActionResult RayonUpdate(Rayon rayon)
        {
            var kullanici = User.Identity.Name;
            ViewBag.Login = kullanici;

            var entity=rayonRepository.TGet(rayon.RayonId);

            entity.RayonName= rayon.RayonName;
            entity.RayonDescription= rayon.RayonDescription;
          
            rayonRepository.TUpdate(entity);
            return RedirectToAction("Index", "Rayon");
        }

        public IActionResult RayonDelete(int id)
        {
            var kullanici = User.Identity.Name;
            ViewBag.Login = kullanici;

            var entity=rayonRepository.TGet(id);

            rayonRepository.TDelete(entity);

            return RedirectToAction("Index", "Rayon");



        }

        public IActionResult Details(int id )
        {
            var kullanici = User.Identity.Name;
            ViewBag.Login = kullanici;

            var products=db.Products.Where(i=>i.RayonId==id)
                .ToList();

            var entity = db.Rayons.Where(a => a.RayonId == id).Select(x => x.RayonName).FirstOrDefault();
            ViewBag.entity = entity;
            return View(products);
        }
    }
}
