using CVHub.Data;
using CVHub.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace CVHub.Controllers
{
    [Authorize, ApiController, Route("User")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;

        public UserController(ApplicationDbContext db)
        {
            _db = db;
        }

        [AllowAnonymous, HttpGet("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create([FromForm]User obj)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _db.Users.Add(obj);
                    _db.SaveChanges();
                    obj.UserId = _db.Users.Where(u => u.Email == obj.Email).FirstOrDefault().UserId;

                    var serverClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Email, $"{obj.Email}"),
                    new Claim(ClaimTypes.GivenName, $"{obj.FirstName} {obj.LastName}")
                };

                    var claimIdentity = new ClaimsIdentity(serverClaims, "serverClaim");
                    var userPrincipal = new ClaimsPrincipal(claimIdentity);

                    HttpContext.SignInAsync(userPrincipal);

                    //Skapar en session
                    HttpContext.Session.SetString("Email", obj.Email);

                    return View("MyPage", obj);
                }
                catch
                {
                    return View("Register");
                }
            }
            else
            {
                return Redirect("/Validation/Register");
            }
        }

        [HttpGet("MyPage")]
        public IActionResult MyPage()
        {
            if (GetOne(null) != null)
            {
                var user = GetOne(null);
                return View(user);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("GetOne")]
        public User GetOne(int? Id)
        {
            if (Id != null)
            {
                var user = _db.Users.Find(Id);
                return user;
            }
            else if (HttpContext.Session.GetString("Email") != null)
            {
                var user = _db.Users.Where(u => u.Email == HttpContext.Session.GetString("Email")).FirstOrDefault();
                return user;
            }
            else
            {
                return null;
            }
        }

        [HttpGet("Update")]
        public IActionResult Update(int? id)
        {
            if (id == null || id == 0)
            {
                User user = _db.Users.Where(u => u.Email == HttpContext.Session.GetString("Email")).FirstOrDefault();
                return View(user);
            }
            else
            {
                User user = _db.Users.Find(id);
                return View(user);
            }
        }

        [HttpPost("Update")]
        public IActionResult Update([FromForm]User user)
        {
            if (ModelState.IsValid)
            {
                _db.Users.Update(user);
                _db.SaveChanges();
                return View("MyPage", user);
            }
            return View("MyPage", user);
        }

        [HttpGet("ChangePassword")]
        public IActionResult ChangePassword(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var user = _db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            var _user = new ChangePasswordUser { UserId = user.UserId, OldPassword = user.Password, NewPassword = user.Password };
            return View(_user);
            
        }

        [HttpPost("ChangePassword")]
        public IActionResult ChangePassword([FromForm]ChangePasswordUser fromFormUser)
        {
            //Tvungen att göra såhär för det gick inte att få med userId från view av någon anledning... 
            User user = _db.Users.Where(u => u.Email == HttpContext.Session.GetString("Email")).FirstOrDefault();
            

            if (user.Password != fromFormUser.OldPassword)
            {
                ViewBag.Message = "Wrong password entered!";
                return View();
            }
            else if (ModelState.IsValid && fromFormUser.OldPassword == user.Password)
            {
                user.Password = fromFormUser.NewPassword;
                _db.Users.Update(user);
                _db.SaveChanges();
                return View("MyPage", user);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}

