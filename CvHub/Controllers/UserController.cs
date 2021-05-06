using CvHub.Models;
using CVHub.Controllers;
using CVHub.Data;
using CVHub.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CvHub.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;

        public UserController(ApplicationDbContext db)
        {
            _db = db;
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(User obj)
        {
            if (ModelState.IsValid)
            {
                _db.Users.Add(obj);
                _db.SaveChanges();

                var serverClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Email, $"{obj.Email}"),
                    new Claim(ClaimTypes.GivenName, $"{obj.FirstName} {obj.LastName}")
                };

                var claimIdentity = new ClaimsIdentity(serverClaims, "serverClaim");
                var userPrincipal = new ClaimsPrincipal(claimIdentity);

                HttpContext.SignInAsync(userPrincipal);

                return RedirectToAction("MyPages");
            }
            else
            {
                return Redirect("/Validation/Register");
            }
        }
        public IActionResult MyPages()
        {
            if (HttpContext.User.Identities.FirstOrDefault().Claims.ElementAt(1).Type.ToString() == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")
            {
                var email = HttpContext.User.Identities.FirstOrDefault().Claims.ElementAt(1).Value.ToString();
                var user = _db.Users.Where(u => u.Email == email).FirstOrDefault();
                return View(user);
            }
            else if(HttpContext.User.Identities.FirstOrDefault().Claims.ElementAt(0).Type.ToString() == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")
            {
                var email = HttpContext.User.Identities.FirstOrDefault().Claims.ElementAt(0).Value.ToString();
                var user = _db.Users.Where(u => u.Email == email).FirstOrDefault();
                return View(user);
            }
            else 
            {
                return View();
            }
        }
    }
}

