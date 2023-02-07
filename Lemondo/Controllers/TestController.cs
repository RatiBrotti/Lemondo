using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Lemondo.Controllers
{
    public class TestController : Controller
    {
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (result?.Principal != null)
            {
                // Get the user's email from the claims
                var email = result.Principal.FindFirstValue(ClaimTypes.Email);

                // Check if the user is an administrator
                var isAdmin = await IsUserAdmin(email);

                // Add the "IsAdmin" claim to the user's identity
                var claimsIdentity = result.Principal.Identity as ClaimsIdentity;
                claimsIdentity.AddClaim(new Claim("IsAdmin", isAdmin.ToString()));
            }

            var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
            {
                claim.Issuer,
                claim.OriginalIssuer,
                claim.Type,
                claim.Value

            });
            return Json(claims);
        }

        private async Task<bool> IsUserAdmin(string email)
        {
            

            return false;
        }

    }
}
