using Lemondo.DbClasses;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Google;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Lemondo.ClientClass;

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
                    var entityUser = GetUser(user.Email);
                    return Content(entityUser.Result.ToString());
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
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public IActionResult UserPage()
        {

            return View();
        }

        [Authorize]
        public async Task<IActionResult> Books()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7084/api/");
                var response = await client.GetAsync("Book");

                if (response.IsSuccessStatusCode)
                {
                    var books = JsonConvert.DeserializeObject<List<BookResponse>>(await response.Content.ReadAsStringAsync());

                    return View(books);
                }
                else
                {
                    return View("Error");
                }
            }
        }


        [Authorize]
        public IActionResult Authors()
        {

            return View();
        }

        [Authorize]
        public IActionResult Users()
        {

            return View();
        }
        public async Task<UserResponse> GetUser(string email)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"https://localhost:7084/api/User/find/{email}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<UserResponse>(json);
                }
                else
                {
                    return null;
                }
            }
        }



    }
}
