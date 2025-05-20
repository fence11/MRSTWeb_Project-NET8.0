using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BigBox_v4.Controllers
{
     [Authorize]
     public class AdminController : Controller
     {
          public IActionResult Dashboard()
          {
               if (!User.HasClaim("IsAdmin", "True"))
                    return Forbid();

               return View();
          }

          // Optional: for quick testing without login
          [AllowAnonymous]
          public async Task<IActionResult> FakeLogin()
          {
               var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "FakeAdmin"),
                new Claim("IsAdmin", "True")
            };

               var identity = new ClaimsIdentity(claims, "Test");
               var principal = new ClaimsPrincipal(identity);

               await HttpContext.SignInAsync("Cookies", principal);

               return RedirectToAction("Dashboard");
          }
     }
}
