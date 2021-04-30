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
                RedirectUri = Url.Action("FacebookValidation")
            };

            //Magic happens... Metoden matchar properties objektet med Facebooks standard authentication Scheme om det inte matchar skickas användaren vidare till 
            //inloggning på facebook via facebooks API. Svaret skickas sedan in i "FacebookValidation" som är satt som redirectURL i properties.
            return Challenge(properties, FacebookDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> FacebookValidation()
        {
            //Detta är Http svaret från facebook som är lagrat som Cookies i webbläsaren. 
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            //additional magic... Detta tar Http svaret och delar upp svarets olika delar och skjuter in dem i listan results.
            var results = result.Principal.Identities.FirstOrDefault().Claims.ToList();
            
            if (ValidateUser(results) == 1)
            {
                return View("SignIn", ValidateUser(results));
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
            //Sedan tar jag bort täcken i början av strängen så bara för och efternamn finns kvar som man sedan kan använda för att söka i databasen.
            //Till sist så görs alla karraktärer till små bokstäver för att generallisera resultet. 
            user.FirstName = results.ElementAt(3).ToString().Remove(0, 65).ToLower();
            user.LastName = results.ElementAt(4).ToString().Remove(0, 63).ToLower();

            var DbObject = _db.Users
                            .Where(u => u.FirstName.ToLower() == user.FirstName && u.LastName.ToLower() == user.LastName)
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

