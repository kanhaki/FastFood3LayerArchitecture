using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using GUI.Models;
using System.Linq;

namespace GUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Trang Home/Index
        public IActionResult Index()
        {
            // Dữ liệu bài viết tĩnh
            ViewBag.Blogs = new[]
            {
                new { Id = 1, Title="Cheese Pizza", Category="Pizza", Image="~/images/blog-1.jpg", Summary="Financial experts support ...", Author="Jonathan Smith", Date="Jan 01 2022", Content="Nội dung đầy đủ bài Cheese Pizza..." },
                new { Id = 2, Title="Chicken Strips", Category="Burger", Image="~/images/blog-2.jpg", Summary="Financial experts support ...", Author="Jonathan Smith", Date="Jan 02 2022", Content="Nội dung đầy đủ bài Chicken Strips..." },
                new { Id = 3, Title="Hot Pasta", Category="Chicken", Image="~/images/blog-3.jpg", Summary="Financial experts support ...", Author="Jonathan Smith", Date="Jan 03 2022", Content="Nội dung đầy đủ bài Hot Pasta..." }
            };

            return View();
        }

        // Chi tiết bài viết
        public IActionResult BlogDetails(int id)
        {
            var blogs = (dynamic[])ViewBag.Blogs; // Lấy mảng bài viết
            var blog = blogs.FirstOrDefault(b => b.Id == id);
            if (blog == null) return NotFound();
            return View(blog); // Truyền dữ liệu bài viết cho view Details
        }

        public IActionResult Blog1()
        {
            return View();
        }

        public IActionResult Blog2()
        {
            return View();
        }

        public IActionResult Blog3()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View("~/Views/Login/Index.cshtml");
        }

        public IActionResult Registry()
        {
            return View("~/Views/Registry/Index.cshtml");
        }

        public IActionResult Reservation()
        {
            return View("~/Views/Reservation/Index.cshtml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

//Đã chỉnh sửa
