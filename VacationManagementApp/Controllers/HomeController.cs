using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VacationManagementApp.Models;
using System.Linq;
using VacationManagementApp.Interfaces;

namespace VacationManagementApp.Controllers
{
    public class HomeController : Controller
    {

        private readonly IHomeService _homeService;
        public HomeController(IHomeService homeService,ILogger<HomeController> logger, UserManager<User> userManager)
        {
            _homeService = homeService;
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
            var userData = await _homeService.ShowYourData();
            if (userData != null)
            {
                return View(userData);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> YourEmployees()
        {
            if (User.IsInRole("Employer"))
            {
                return View(await _homeService.ShowYourEmployees());
            }
            else
            {
                return RedirectToAction("Index"); 
            }
        }
    }
}