using CvHub.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CvHub.Controllers
{

    public class UserController : Controller
    {
        [AllowAnonymous]
        public IActionResult SignIn()
        {
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

            User user = new User();
            //Detta tar Http svaret och gör om det till ett Json svar
            var results = result.Principal.Identities.FirstOrDefault().Claims.ToList();

            //Här letade jag upp rätt element med hjälp av ElementAt och sedan gjorde jag om det till en sträng.
            user.FirstName = results.ElementAt(3).ToString();
            user.LastName = results.ElementAt(4).ToString();

            //Här tar jag bort täcken i början av strängen så bara för och efternamn finns kvar som man sedan kan använda för att söka i databasen. 
            user.FirstName = user.FirstName.Remove(0, 65);
            user.LastName = user.LastName.Remove(0, 63);

            return Json(user.FirstName + " " + user.LastName);
        }
    }
}

