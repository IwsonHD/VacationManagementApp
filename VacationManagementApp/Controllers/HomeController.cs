using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VacationManagementApp.Models;
using System.Linq;

namespace VacationManagementApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<User> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult HowToUse() 
        {
            return View();    
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> YourData()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userModel = await _userManager.GetUserAsync(User);
                if(userModel != null)
                {
                    return View(userModel);
                }
                return NotFound();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> YourEmployees()
        {
            if (User.IsInRole("Employer"))
            {
                var you = await _userManager.GetUserAsync(User);

                IEnumerable<Employee> yourEmployees = _userManager.Users
                    .OfType<Employee>()
                    .Where(e => e.EmployersEmail == you.Email)
                    .ToList();
                return View(yourEmployees);
            }
            else
            {
                return RedirectToAction("Index"); 
            }
        }
    }
}