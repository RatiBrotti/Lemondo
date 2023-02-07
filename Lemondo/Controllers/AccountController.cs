using Lemondo.DbClasses;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Google;
using Newtonsoft.Json;
using System.Text;

namespace Lemondo.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            User user = new User
            {
                FirstName = result.Principal.FindFirstValue(ClaimTypes.GivenName),
                LastName = result.Principal.FindFirstValue(ClaimTypes.Surname),
                Email = result.Principal.FindFirstValue(ClaimTypes.Email)
            };
            var client = new HttpClient();
            var userJSon = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            
            var createUserResponse = await client.PostAsync("https://localhost:7084/api/User", userJSon);
            if (createUserResponse.IsSuccessStatusCode)
            {
                var statusMessage = await createUserResponse.Content.ReadAsStringAsync();
                if (statusMessage == "Already Exists") 
                {
                    return RedirectToAction("UserPage", "Home");
                } 
                else
                {
                    return RedirectToAction("WelcomePage", "Home");
                }
            }
            else
            {
                return Content("Error, Something went wrong!!!");
            }
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return Content("ki");
        }
    }
}
