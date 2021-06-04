using CVHub.Data;
using CVHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;

namespace CVHub.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        
        public IActionResult Index()
        {
            //Gör så det alltid ska finnas en test användare som testerna kan köras mot.
            if (_db.Users.Where(u => u.Email == "test@test.com").FirstOrDefault() == null)
            {
                var testUser = new User { Email = "test@test.com", Password = "123456", FirstName = "test", LastName = "test", Country = "test", City = "test", State = "test", PhoneNumber = "1234567891" };
                _db.Add(testUser);
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
