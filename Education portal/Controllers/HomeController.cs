using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DateBaseServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Education_portal.Models;
using Microsoft.EntityFrameworkCore;

namespace Education_portal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _db;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _db = new DataContext();
        }

        public IActionResult Index()
        {
            var courses = _db.Courses.GetAllCourses();
            return View(courses);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}