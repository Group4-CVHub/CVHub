using CVHub.Data;
using CVHub.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CVHub.Controllers
{
    [Authorize, ApiController, Route("Validation")]
    public class ValidationController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ValidationController(ApplicationDbContext db)
        {
            _db = db;
        }


        [AllowAnonymous, HttpGet("SignIn")]
        public IActionResult SignIn()
        {
            return View();
        }

        [AllowAnonymous, HttpGet("FacebookSignIn")]
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

        [HttpGet("FacebookAuthentication")]
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

                User user = new () { FacebookId = facebookID, Email = email, FirstName = fName, LastName = lName };

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
                        //Skapar en session
                        HttpContext.Session.SetString("Email", DbUser.Email);
                        return RedirectToAction("MyPage", "User");
                    }
                    else if (DbUser.FacebookId == null && DbUser.Email != null)
                    {
                        //Skapar en session
                        HttpContext.Session.SetString("Email", DbUser.Email);
                        DbUser.FacebookId = facebookID;
                        _db.SaveChanges();
                        return RedirectToAction("MyPage", "User");
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

        //Google-inlogg, fick det inte att fungera, sparar koden ifall man vill fortsätta. 
        //[AllowAnonymous]
        //public IActionResult GoogleSignIn()
        //{
        //    //Skapar ett objekt med authentication properties och sätter redirectURL till Google validerings sidan.
        //    var properties = new AuthenticationProperties
        //    {
        //        RedirectUri = Url.Action("GoogleAuthentication")
        //    };

        //    //Magic happens... Metoden matchar properties objektet med Googles standard authentication Scheme om det inte matchar skickas användaren vidare till 
        //    //inloggning på Google via Googles API. Svaret skickas sedan in i "GoogleAuthentication" som är satt som redirectURL i properties.
        //    return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        //}

        //public async Task<IActionResult> GoogleAuthentication()
        //{
        //    var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        //    var results = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
        //    {
        //        claim.Value
        //    });

        //    return Json(results);

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
        //}

        [AllowAnonymous]
        [HttpPost("DbAuthentication"), ValidateAntiForgeryToken]
        public IActionResult DbAuthenticate([FromForm]LoginUser user)
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

                //Skapar en session
                HttpContext.Session.SetString("Email", DbUser.Email);

                return Redirect("/User/MyPage");
            }
            else
            {
                return Redirect("/User/Register");
            }
        }

        [HttpGet("LogOut")]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return Redirect("/Home/Index");
        }

    }
}

