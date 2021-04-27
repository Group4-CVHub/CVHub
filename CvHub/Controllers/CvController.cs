using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CvHub.Controllers
{
    public class CvController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
