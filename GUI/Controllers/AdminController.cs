// File: Controllers/AdminController.cs

using Microsoft.AspNetCore.Mvc;

namespace GUI.Controllers
{
    public class AdminController : Controller
    {
        // 1. Dashboard
        public IActionResult Dashboard()
        {
            return View("Dashboard"); // Views/Admin/Dashboard.cshtml
        }

        // 2. Orders
        public IActionResult Orders()
        {
            return View("Orders"); // Views/Admin/Orders.cshtml
        }

        // 3. Menu
        public IActionResult Menu()
        {
            return View("Menu"); // Views/Admin/Menu.cshtml
        }

        // 4. Customers
        public IActionResult Customers()
        {
            return View("Customers"); // Views/Admin/Customers.cshtml
        }

        // 5. Reservations
        public IActionResult Reservations()
        {
            return View("Reservations"); // Views/Admin/Reservations.cshtml
        }

        // 6. Reports
        public IActionResult Reports()
        {
            return View("Reports"); // Views/Admin/Reports.cshtml
        }

        // 7. Settings
        public IActionResult Settings()
        {
            return View("Settings"); // Views/Admin/Settings.cshtml
        }

        // 8. Index redirect
        public IActionResult Index()
        {
            return RedirectToAction("Dashboard");
        }
    }
}
