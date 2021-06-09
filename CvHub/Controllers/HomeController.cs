using CVHub.Data;
using CVHub.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CVHub.Controllers
{

    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }


        public IActionResult Index()
        {
            //Gör så det alltid ska finnas en testanvändare som testerna kan köras mot.
            if (_db.Users.Where(u => u.Email == "test@test.com").FirstOrDefault() == null)
            {
                var testUser = new User
                {
                    Email = "test@test.com",
                    Password = "123456",
                    FirstName = "test",
                    LastName = "test",
                    Country = "test",
                    City = "test",
                    PostalCode = "test",
                    PhoneNumber = "1234567891"
                };
                _db.Templates.Add(new Template { Description = "Minimalist" });
                _db.Add(testUser);
                _db.SaveChanges();
            }
            if(_db.Cvs.Where(c => c.Title == "test" && c.AboutMe == "test").FirstOrDefault() == null)
            { 
                var testCv = new Cv 
                { 
                    AboutMe = "test", 
                    Title = "test", 
                    User = _db.Users.Find(1),
                    Template = _db.Templates.Find(1), 
                    TemplateId = 1
                };
                var testEdu = new Education 
                { 
                    EducationStart = DateTime.Now, 
                    EducationStop = DateTime.Now, 
                    Name = "test", 
                    Degree = "test" 
                };
                var testWork = new Work 
                { 
                    Description = "test",
                    Name = "test", 
                    WorkStart = DateTime.Now, 
                    WorkStop = DateTime.Now 
                };
                testEdu.Cv = testCv;
                testWork.Cv = testCv;

                List<Education> educations = new();
                List<Work> workPlaces = new();
                educations.Add(testEdu);
                workPlaces.Add(testWork);

                testCv.Educations = educations;
                testCv.WorkPlaces = workPlaces;

                _db.Educations.Add(testEdu);
                _db.WorkPlaces.Add(testWork);
                _db.Add(testCv);
                _db.SaveChanges();
            }
            //Ett av E2E testerna skapar en ny användare. Denna ser till så den tas bort efter. 
            if (_db.Users.Where(u => u.Email == "test2@test2.com").FirstOrDefault() != null)
            {
                var newUser = _db.Users.Where(u => u.Email == "test2@test2.com").FirstOrDefault();
                _db.Users.Remove(newUser);
                _db.SaveChanges();
            }
            return View();
        }


        public IActionResult AboutUs()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
