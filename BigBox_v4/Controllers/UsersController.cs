using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BigBox_v4.Data;
using System.Linq;

namespace BigBox_v4.Controllers
{
     [Authorize]
     public class UsersController : Controller
     {
          private readonly ApplicationDBContext _context;

          public UsersController(ApplicationDBContext context)
          {
               _context = context;
          }

          public IActionResult Index()
          {
               if (!User.HasClaim("IsAdmin", "True"))
                    return Forbid();

               var users = _context.Users.ToList();
               return View(users);
          }
     }
}
