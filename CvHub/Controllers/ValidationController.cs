using CVHub.Data;
using CVHub.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
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
            //inloggning på facebook via facebooks API. Svaret skickas sedan in i "FacebookAuthentication" som är satt som redirectURL i properties.
            return Challenge(properties, FacebookDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> FacebookAuthentication()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (result.Succeeded)
            {
                //Tar cookiesvaret från facebook och använder claimsen för att hitta om användaren finns eller inte i databasen. FbID och Email är unikt. 
                var facebookID = result.Principal.Identities.FirstOrDefault().Claims.ElementAt(0).Value.ToString();
                var email = result.Principal.Identities.FirstOrDefault().Claims.ElementAt(1).Value.ToString();
                var fName = result.Principal.Identities.FirstOrDefault().Claims.ElementAt(3).Value.ToString();
                var lName = result.Principal.Identities.FirstOrDefault().Claims.ElementAt(4).Value.ToString();

                User user = new User { FacebookId = facebookID, Email = email, FirstName = fName, LastName = lName };

                var DbUser = _db.Users.Where(u => u.Email == user.Email).FirstOrDefault();

                //Nedan kontroll kollar ifall man tidigare loggat in med FB genom att se om det finns ett FbID. Om det inte finns en användare som matchar så
                //loggas cookiesanvändaren ut och man skickas vidare till register fönstret. Om Email matchar men inte FBiD finns så loggas man in och FBiD fylls på.
                if (DbUser == null)
                {
                    await LogOut();
                    return Redirect("/User/Register");
                }
                else
                {
                    if (DbUser.FacebookId != null && DbUser.Email != null)
                    {
                        return RedirectToAction("MyPages", "User");
                    }
                    else if (DbUser.FacebookId == null && DbUser.Email != null)
                    {
                        DbUser.FacebookId = facebookID;
                        _db.SaveChanges();
                        return RedirectToAction("MyPages", "User");
                    }
                    else
                    {
                        await LogOut();
                        return Redirect("/User/Register");
                    }
                }
            }
            else
            {
                return Redirect("/User/Register");
            }
        }

        [AllowAnonymous, Route("signin-google")]
        public IActionResult GoogleSignIn()
        {
            //Skapar ett objekt med authentication properties och sätter redirectURL till Google validerings sidan.
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleAuthentication"),
            };

            //Magic happens... Metoden matchar properties objektet med Googles standard authentication Scheme om det inte matchar skickas användaren vidare till 
            //inloggning på Google via Googles API. Svaret skickas sedan in i "GoogleAuthentication" som är satt som redirectURL i properties.
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> GoogleAuthentication()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var results = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
            {
                claim.Value
            });

            return Json(results);

            //if (result.Succeeded)
            //{
            //    //Tar cookiesvaret från facebook och använder claimsen för att hitta om användaren finns eller inte i databasen. FbID och Email är unikt. 
            //    var facebookID = result.Principal.Identities.FirstOrDefault().Claims.ElementAt(0).Value.ToString();
            //    var email = result.Principal.Identities.FirstOrDefault().Claims.ElementAt(1).Value.ToString();
            //    var fName = result.Principal.Identities.FirstOrDefault().Claims.ElementAt(3).Value.ToString();
            //    var lName = result.Principal.Identities.FirstOrDefault().Claims.ElementAt(4).Value.ToString();

            //    User user = new User { FacebookId = facebookID, Email = email, FirstName = fName, LastName = lName };

            //    var DbUser = _db.Users.Where(u => u.Email == user.Email).FirstOrDefault();

            //    //Nedan kontroll kollar ifall man tidigare loggat in med FB genom att se om det finns ett FbID. Om det inte finns en användare som matchar så
            //    //loggas cookiesanvändaren ut och man skickas vidare till register fönstret. Om Email matchar men inte FBiD finns så loggas man in och FBiD fylls på.
            //    if (DbUser == null)
            //    {
            //        await LogOut();
            //        return Redirect("/User/Register");
            //    }
            //    else
            //    {
            //        if (DbUser.FacebookId != null && DbUser.Email != null)
            //        {
            //            return RedirectToAction("MyPages", "User");
            //        }
            //        else if (DbUser.FacebookId == null && DbUser.Email != null)
            //        {
            //            DbUser.FacebookId = facebookID;
            //            _db.SaveChanges();
            //            return RedirectToAction("MyPages", "User");
            //        }
            //        else
            //        {
            //            await LogOut();
            //            return Redirect("/User/Register");
            //        }
            //    }
            //}
            //else
            //{
            //    return Redirect("/User/Register");
            //}
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

