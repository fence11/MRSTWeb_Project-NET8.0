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

     }
}
