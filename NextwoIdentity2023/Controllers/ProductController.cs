using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NextwoIdentity2023.Data;
using NextwoIdentity2023.Models;
using System.Data;

namespace NextwoIdentity2023.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly NextwoDbContext db;

        public ProductController(ILogger<HomeController> _logger, NextwoDbContext _db)
        {
            logger = _logger;
            db = _db;
        }
        public IActionResult Index()
        {
            return View(db.Products.Include(x => x.Category));
        }

       
        [HttpGet]
        [Authorize(Roles = "Admin")]

        public IActionResult Create()
        {
            ViewBag.AllCat = new SelectList(db.Categories, "CategoryId", "CategoryName");
            return View();

        }
        [HttpPost]

        public IActionResult Create(Product Product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(Product);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(Product);


        }
        [HttpGet]

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var data = db.Products.Find(id);
            if (data == null)
            {
                return RedirectToAction("Index");

            }
            return View(data);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var data = db.Products.Find(id);
            if (data == null)
            {
                return RedirectToAction("Index");

            }
            ViewBag.AllCat = new SelectList(db.Categories, "CategoryId", "CategoryName");

            return View(data);
        }
        [HttpPost]
        public IActionResult Edit(Product Product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Update(Product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Product);

        }



        [HttpGet]
        [Authorize(Roles = "Admin")]

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var data = db.Products.Find(id);
            if (data == null)
            {
                return RedirectToAction("Index");

            }
            return View(data);
        }


   
        [HttpPost]

        public IActionResult Delete(Product Product)
        {
            var data = db.Products.Find(Product.ProductId);
            if (data == null)
            {
                return RedirectToAction("Index");
            }
            db.Products.Remove(data); db.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}
