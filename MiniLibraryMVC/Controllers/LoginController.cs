using Microsoft.AspNetCore.Mvc;
using MiniLibraryMVC.Services;
using MiniLibraryMVC.Models;

namespace MiniLibraryMVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly LibraryService _library;

        public LoginController(LibraryService library)
        {
            _library = library;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(); // This is your login page
        }

        [HttpPost]
        public IActionResult Login(string userType, string username, string password)
        {
            // Basic input validation
            if (string.IsNullOrEmpty(userType) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "All fields are required.";
                return View("Index");
            }

            // Admin login logic
            if (userType == "Admin")
            {
                if (username == "admin" && password == "admin123")
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    ViewBag.Error = "Invalid admin credentials.";
                    return View("Index");
                }
            }

            // Member login logic
            else if (userType == "Member")
            {
                var member = _library.GetMemberByID(username); // username is ICNumber

                if (member != null && member.Password == password)
                {
                    HttpContext.Session.SetString("MemberIC", member.ICNumber);
                    HttpContext.Session.SetString("MemberName", member.Name);
                    return RedirectToAction("Index", "Member");
                }
                else
                {
                    ViewBag.Error = "Invalid member credentials.";
                    return View("Index");
                }
            }

            ViewBag.Error = "Invalid user type selected.";
            return View("Index");
        }
    }
}
