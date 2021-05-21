using CVHub.Data;
using CVHub.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CVHub.Controllers
{
    public class CvController : Controller
    {

        private readonly ApplicationDbContext _db;

        public CvController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CvTemp obj)
        {
            Cv cv = new(){ 
                User = _db.Users.Where(u => u.Email == HttpContext.Session.GetString("Email")).FirstOrDefault(),
                AboutMe = obj.AboutMe,
                Educations = obj.Educations,
                Picture = obj.Picture,
                Template = _db.Templates.Find(1),
                Title = obj.Title,
                WorkPlaces = obj.WorkPlaces
            };

            if (ModelState.IsValid)
            {
                //Här läggs objektet till i databasen
                _db.Cvs.Add(cv);
                _db.SaveChanges();

                return Redirect("/User/MyPage");
            }
            return View(obj);
        }
    }
}
