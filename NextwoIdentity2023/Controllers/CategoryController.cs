using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NextwoIdentity2023.Data;
using NextwoIdentity2023.Models;
using System.Data;

namespace NextwoIdentity2023.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly NextwoDbContext db;

        public CategoryController(ILogger<HomeController> _logger, NextwoDbContext _db)
        {
            logger = _logger;
            db = _db;
        }
        public IActionResult Index()
        {
            //List<Category> categories = new List<Category>();

            return View(db.Categories);
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();

        }
        [HttpPost]

        public IActionResult Create(Category Category)
        {
            if (ModelState.IsValid)
            {

                db.Categories.Add(Category);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(Category);


        }
        [HttpGet]

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var data = db.Categories.Find(id);
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
            var data = db.Categories.Find(id);
            if (data == null)
            {
                return RedirectToAction("Index");

            }
            return View(data);
        }
        [HttpPost]
        public IActionResult Edit(Category Category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Update(Category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Category);

        }



        [HttpGet]
        [Authorize(Roles = "Admin")]

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var data = db.Categories.Find(id);
            if (data == null)
            {
                return RedirectToAction("Index");

            }
            return View(data);
        }



        [HttpPost]

        public IActionResult Delete(Category Category)
        {
            var data = db.Categories.Find(Category.CategoryId);
            if (data == null)
            {
                return RedirectToAction("Index");
            }
            db.Categories.Remove(data); db.SaveChanges();
            return RedirectToAction("Index");

        }

        //public IActionResult IsExist(Category Category)
        //{
        //    var data = db.Categories.Find(Category.CategoryId);
        //    if (data == null)
        //    {
        //        return RedirectToAction("Index");
        //    }
        //    if (Category.CategoryName == data)
        //    {

        //    }
        //     db.SaveChanges();
        //    return RedirectToAction("Index");

        //}
        //public bool CategoryExists(int id)
        //{
        //    return (db.Categories?.Any(e => e.CategoryId == id)).GetValueOrDefault();
        //}
    }
}
