using CVHub.Data;
using CVHub.Models;
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
            //Här gör man så det skapade objektet får rätt UserId genom att anropa metoden UserController.GetOne
            UserController uc = new UserController(_db);
            obj.UserId = uc.GetOne(null).UserId;

            Cv cv = new(){ 
                AboutMe = obj.AboutMe,
                Educations = obj.Educations,
                Picture = obj.Picture,
                TemplateId = obj.TemplateId,
                Title = obj.Title,
                WorkPlaces = obj.WorkPlaces
            };

            if (ModelState.IsValid)
            {
                //Här läggs objektet till i databasen
                _db.Cvs.Add(cv);
                _db.SaveChanges();

                //Här tar man fram rätt CVid
                //obj.CvId = _db.Cvs.Where(c => c.User.UserId == obj.User.UserId).OrderBy(c => c.CvId).Last().CvId;
                
                ////Här gör man så varje education som hör till Cv:t får rätt foreign key (CvId) och sedan sparar man informationen i databasen.
                //for (int e = 0; e < obj.Educations.Count(); e++)
                //{
                //    obj.Educations[e].CvId = obj.CvId;
                //    _db.Educations.Add(obj.Educations[e]);
                //    _db.SaveChanges();
                //}

                ////Här görs samma fast med workplaces som hör till CV:t
                //for (int w = 0; w < obj.WorkPlaces.Count(); w++)
                //{
                //    obj.WorkPlaces[w].CvId = obj.CvId;
                //    _db.WorkPlaces.Add(obj.WorkPlaces[w]);
                //    _db.SaveChanges();
                //}

                return Redirect("/User/MyPage");
            }
            return View(obj);
        }
    }
}
