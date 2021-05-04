using CVHub.Data;
using CVHub.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CVHub.Controllers
{
    [Authorize]
    public class ValidationController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ValidationController(ApplicationDbContext db)
        {
            _db = db;
        }

        [AllowAnonymous]
        public IActionResult SignIn()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult FacebookSignIn()
        {
            //Skapar ett objekt med authentication properties och sätter redirectURL till facebook validerings sidan.
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("FacebookAuthentication")
            };

            //Magic happens... Metoden matchar properties objektet med Facebooks standard authentication Scheme om det inte matchar skickas användaren vidare till 
            //inloggning på facebook via facebooks API. Svaret skickas sedan in i "FacebookValidation" som är satt som redirectURL i properties.
            return Challenge(properties, FacebookDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> FacebookAuthentication()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if(result.Succeeded)
            {
                var facebookID = result.Principal.Identities.FirstOrDefault().Claims.ElementAt(0).Value.ToString();
                var email = result.Principal.Identities.FirstOrDefault().Claims.ElementAt(1).Value.ToString();
                var fName = result.Principal.Identities.FirstOrDefault().Claims.ElementAt(3).Value.ToString();
                var lName = result.Principal.Identities.FirstOrDefault().Claims.ElementAt(4).Value.ToString();

                User user = new User { FacebookId = facebookID, Email = email, FirstName = fName, LastName = lName };

                var DbUser = _db.Users.Where(u => u.FacebookId == user.FacebookId).FirstOrDefault();

                if(DbUser != null)
                {
                    return RedirectToAction("SuccesfullLogIn", DbUser);
                }
                else 
                {
                    await LogOut();
                    return Redirect("/User/Register");
                }
            }
            else
            {
                return Redirect("/User/Register");
            }
        }

        [AllowAnonymous]
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult DbAuthenticate(User user)
        {
            var DbUser = _db.Users.Where(u => u.Email == user.Email && u.Password == user.Password).FirstOrDefault();

            if (DbUser != null)
            {
                var serverClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Email, $"{DbUser.Email}"),
                    new Claim(ClaimTypes.GivenName, $"{DbUser.FirstName} {DbUser.LastName}")
                };

                var claimIdentity = new ClaimsIdentity(serverClaims, "serverClaim");
                var userPrincipal = new ClaimsPrincipal(claimIdentity);

                HttpContext.SignInAsync(userPrincipal);
                return View("SuccessfullLogIn", DbUser);
            }
            else
            {
                return Redirect("/User/Register");
            }
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Home/Index");
        }

        public IActionResult SuccessfullLogIn()
        {
            return View();
        }
    }
}

