using CVHub.Data;
using CVHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace CVHub.Controllers
{
    [Authorize]
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

        [HttpGet]
        public IActionResult CvList()
        {
            var user = _db.Users.Where(u => u.Email == HttpContext.Session.GetString("Email")).FirstOrDefault();
            IEnumerable<Cv> cvs = _db.Cvs.Where(c => c.UserId == user.UserId).ToList();
            return View(cvs); 
        }

        [HttpGet]
        public IActionResult Get(int id)
        {
            Cv cv = _db.Cvs.Find(id);

            //fyller på utbildningar och jobb för objektet som ska visas i view.
            var educations = _db.Educations.Where(e => e.CvId == cv.CvId).ToList();
            var workPlaces = _db.WorkPlaces.Where(w => w.CvId == cv.CvId).ToList();
            
            foreach (Education e in educations)
            {
                cv.Educations.Add(e);
            }
            foreach (Work w in workPlaces)
            {
                cv.WorkPlaces.Add(w);
            }

            //Kollar så det är den inloggade användaren som försöker få access till ett cv.
            var userId = _db.Users.Where(u => u.Email == HttpContext.Session.GetString("Email")).FirstOrDefault().UserId;


            if (cv == null)
            {
                return NotFound();
            }
            else if (cv.User.UserId != userId)
            {
                return Unauthorized();
            }
            else
            {
                return View("Cv", cv);
            }
        }

        [HttpGet]
        public IActionResult Cv(Cv cv)
        {
            return View(cv);
        }
    }
}
