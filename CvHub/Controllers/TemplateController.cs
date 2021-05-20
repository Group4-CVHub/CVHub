using Microsoft.AspNetCore.Mvc;
using CVHub.Models;

namespace CVHub.Controllers
{
    public class TemplateController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Template1()
        {
            return View();
        }

        [HttpGet]
        public IActionResult TemplateForm()
        {
            var cv = new CvTemp();
            return View(cv);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddEducation(CvTemp obj)
        {
            obj.Educations.Add(new Education());
            return View("TemplateForm", obj);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddWork(CvTemp obj)
        {
            obj.WorkPlaces.Add(new Work());
            return View("TemplateForm", obj);
        }
    }
}
