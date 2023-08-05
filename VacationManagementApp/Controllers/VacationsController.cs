using Microsoft.AspNetCore.Mvc;
using VacationManagementApp.DataBases;

namespace VacationManagementApp.Controllers
{
    public class VacationsController : Controller
    {

        private readonly VacationManagerDbContext _db;

        public VacationsController(VacationManagerDbContext db)
        {
            _db = db;
        }


        public IActionResult Index()
        {
            return View();
        }





    }
}
