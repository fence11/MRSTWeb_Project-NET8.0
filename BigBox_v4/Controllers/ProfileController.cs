using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BigBox_v4.Data;
using System.Security.Claims;

namespace BigBox_v4.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly ApplicationDBContext _context;

        public ProfileController(ApplicationDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Login", "Account");

            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
                return NotFound();

            return View(user);
        }
    }
}
