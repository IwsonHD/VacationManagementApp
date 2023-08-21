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
        private readonly UserManager<User> _userManager;
        private readonly IHomeService _homeService;
        public HomeController(IHomeService homeService,UserManager<User> userManager)
        {
            _homeService = homeService;
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
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }

            return View(await _userManager.GetUserAsync(User));
        }
        
        public async Task<IActionResult> YourEmployees()
        {
            if (!User.IsInRole("Employer"))
            {
                return RedirectToAction("Index");
            }

            return View(await _homeService.ShowYourEmployees());

        }
    }
}