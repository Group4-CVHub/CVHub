using Microsoft.AspNetCore.Mvc;
using CVHub.Models;
using CVHub.Data;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace CVHub.Controllers
{
    public class TemplateController : Controller
    {
        private readonly ApplicationDbContext _db;

        public TemplateController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Template1()
        {
            return View();
        }

        [HttpGet]
        public IActionResult TemplateForm1()
        {
            CvTemp cv = new();
            return View(cv);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddEducation(CvTemp obj)
        {
            obj.Educations.Add(new Education());
            return View("TemplateForm1", obj);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddWork(CvTemp obj)
        {
            obj.WorkPlaces.Add(new Work());
            return View("TemplateForm1", obj);
        }
    }
}
