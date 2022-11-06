using System.Diagnostics;
using DateBaseServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Education_portal.Models;
using Education_portal.Utils;

namespace Education_portal.Controllers
{
    public class CourseController : Controller
    {
        private readonly ILogger<CourseController> _logger;
        private readonly DataContext _db;

        public CourseController(ILogger<CourseController> logger)
        {
            _logger = logger;
            _db = new DataContext();
        }

        [HttpGet]
        public IActionResult Index()
        {
            var user = UserCookieUtility.GetUserInfoFromCookies(HttpContext);
            var courses = _db.Courses.GetAllCourses();
            var coursesForUser = _db.Courses.GetCoursesByUserId(user.UserId, user.Token);
            return View((courses, coursesForUser));
        }

        [HttpGet]
        public IActionResult Course(int courseId)
        {
            var user = UserCookieUtility.GetUserInfoFromCookies(HttpContext);
            var coursesForUser = _db.Courses.GetCoursesByUserId(user.UserId, user.Token);

            var course = _db.Courses.GetCourseByCourseId(courseId);

            var bought = coursesForUser.Contains(course);

            return View((course, bought));
        }

        [HttpGet]
        public IActionResult BoughtCourse(int courseId)
        {
            var user = UserCookieUtility.GetUserInfoFromCookies(HttpContext);
            var coursesForUser = _db.Courses.GetCoursesByUserId(user.UserId, user.Token);

            var course = _db.Courses.GetCourseByCourseId(courseId);

            var bought = coursesForUser.Contains(course);

            if (!bought)
            {
                return RedirectToAction("BuyCourse", courseId);
            }

            return View(course);
        }

        [HttpGet]
        public IActionResult BuyCourse(int courseId)
        {
            var user = UserCookieUtility.GetUserInfoFromCookies(HttpContext);
            var coursesForUser = _db.Courses.GetCoursesByUserId(user.UserId, user.Token);

            var course = _db.Courses.GetCourseByCourseId(courseId);

            var bought = coursesForUser.Contains(course);

            if (bought)
            {
                return RedirectToAction("BoughtCourse", courseId);
            }

            return View(course);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}