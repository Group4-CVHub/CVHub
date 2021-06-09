using CVHub.Data;
using CVHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CVHub.Controllers
{
    [Authorize, ApiController, Route("Template")]
    public class TemplateController : Controller
    {
        private readonly ApplicationDbContext _db;

        public TemplateController(ApplicationDbContext db)
        {
            _db = db;
        }

        [AllowAnonymous, HttpGet("Index")]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous, HttpGet("Template1")]
        public IActionResult Template1()
        {
            return View();
        }

        [HttpGet("TemplateForm1")]
        public IActionResult TemplateForm1()
        {
            CvTemp cv = new();
            return View(cv);
        }

        [HttpPost("AddEducation"), ValidateAntiForgeryToken]
        public IActionResult AddEducation([FromForm] CvTemp obj)
        {
            obj.Educations.Add(new Education());
            return View("TemplateForm1", obj);
        }

        [HttpPost("AddWork"), ValidateAntiForgeryToken]
        public IActionResult AddWork([FromForm] CvTemp obj)
        {
            obj.WorkPlaces.Add(new Work());
            return View("TemplateForm1", obj);
        }
    }
}
