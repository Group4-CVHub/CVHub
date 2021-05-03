using CvHub.Models;
using CVHub.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CvHub.Controllers
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
                RedirectUri = Url.Action("Validation")
            };

            //Magic happens... Metoden matchar properties objektet med Facebooks standard authentication Scheme om det inte matchar skickas användaren vidare till 
            //inloggning på facebook via facebooks API. Svaret skickas sedan in i "FacebookValidation" som är satt som redirectURL i properties.
            return Challenge(properties, FacebookDefaults.AuthenticationScheme);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(FacebookDefaults.AuthenticationScheme);
            return Redirect("/Home/Index");
        }

        public async Task<IActionResult> Validation()
        {
            //Detta är Http svaret från facebook som är lagrat som Cookies i webbläsaren. 
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            //additional magic... Detta tar Http svaret och delar upp svarets olika delar och skjuter in dem i listan results.
            var results = result.Principal.Identities.FirstOrDefault().Claims.ToList();

            if (ValidateUser(results) == 1)
            {
                return Accepted();
            }
            else
            {
                var properties = new AuthenticationProperties();
                properties.ExpiresUtc = System.DateTime.UtcNow;
                return BadRequest();
            }
        }

        private int ValidateUser(List<Claim> results)
        {
            User user = new User();

            //Här letade jag upp rätt element med hjälp av ElementAt och sedan gjorde jag om det till en sträng.
            //Sedan tar jag bort tecken i början av strängen så bara det unika FacebookID finns kvar som man sedan kan använda för att söka i databasen.
            user.FacebookId = int.Parse(results.ElementAt(0).ToString().Remove(0, 70));


            var DbObject = _db.Users
                            .Where(u => u.FacebookId == user.FacebookId)
                            .FirstOrDefault<User>();

            if (DbObject == null)
            {
                return 0;
            }
            else
            {
                if (DbObject.ValidationToken == 0)
                {
                    DbObject.ValidationToken = 1;
                    return 1;
                }
                else
                {
                    return 1;
                }
            }
        }
    }
}

