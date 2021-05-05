using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CvHub.Controllers
{
    public class AboutUsController : Controller
    {
        [Route("AboutUs")]
        [Route("Home/AboutUs/Indext")]
        [Route("AboutUs/Indext/{id?}")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
