using CvHub.Models;
using CVHub.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace CvHub.Controllers
{

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
            //Magic happens... Systemet kollar om man är inloggad, annars skickar den vidare till "FacebookValidation".
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("FacebookValidation")
            };

            return Challenge(properties, FacebookDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> FacebookValidation()
        {
            //Detta är Http svaret från facebook som är lagrat i Cookies i webbläsaren. 
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            //Detta tar Http svaret och gör om det till Json
            var results = result.Principal.Identities.FirstOrDefault().Claims.ToList();

            User user = new User();

            //Här letade jag upp rätt element med hjälp av ElementAt och sedan gjorde jag om det till en sträng.
            //Sedan tar jag bort täcken i början av strängen så bara för och efternamn finns kvar som man sedan kan använda för att söka i databasen.
            //Till sist så görs alla karraktärer till små bokstäver för att generallisera resultet. 
            user.FirstName = results.ElementAt(3).ToString().Remove(0, 65).ToLower();
            user.LastName = results.ElementAt(4).ToString().Remove(0, 63).ToLower();

            return Json(Validation(user));

        }

        public int Validation(User user)
        {

            var DbObject = _db.Users
                            .Where(u => u.FirstName.ToLower() == user.FirstName && u.LastName.ToLower() == user.LastName)
                            .FirstOrDefault<User>();

            if (DbObject == null)
            {
                return 0;
            }
            else
            {
                DbObject.ValidationToken = 1;
                return 1;
            }
        }
    }
}

