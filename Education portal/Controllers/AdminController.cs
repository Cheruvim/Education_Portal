using DateBaseServices;
using Education_portal.Utils;
using EducationPortal.Db.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Education_portal.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _db;

        public AdminController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _db = new DataContext();
        }

        [HttpGet]
        public IActionResult Index()
        {
            var user = UserCookieUtility.GetUserInfoFromCookies(HttpContext);
            if (!_db.Users.UserIsAdminByToken(user.Token))
                return RedirectToAction("Index", "Home");

            var courses = _db.Courses.GetAllCoursesWithNotEnabled(user.Token);
            var users = _db.Users.GetAllUsers(user.Token);

            return View((users, courses));
        }

        private IActionResult AddCourse(string courseTitle, string courseDescription, bool enable)
        {
            var user = UserCookieUtility.GetUserInfoFromCookies(HttpContext);
            var course = new Course
            {
                Description = courseDescription,
                Name = courseTitle,
                Enable = enable
            };

            _db.Courses.AddCourse(course, user.UserId, user.Token);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EditCourse(int courseId, string courseTitle, string courseDescription, bool enable = false)
        {
            if (courseId == -1)
            {
                AddCourse(courseTitle, courseDescription, enable);
                return RedirectToAction("Index");
            }

            var user = UserCookieUtility.GetUserInfoFromCookies(HttpContext);
            var course = new Course
            {
                CourseId = courseId,
                Description = courseDescription,
                Name = courseTitle,
                Enable = enable
            };

            _db.Courses.EditCourse(course, user.UserId, user.Token);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AddUserInCourse(int userId, int courseId)
        {
            var user = UserCookieUtility.GetUserInfoFromCookies(HttpContext);
            _db.Courses.AddUserInCourse(courseId, userId, user.Token);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RemoveUserFromCourse(int userId, int courseId)
        {
            var user = UserCookieUtility.GetUserInfoFromCookies(HttpContext);
            _db.Courses.RemoveUserFromCourse(courseId, userId, user.Token);

            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult GetAllUserInfoByUserId(int userId)
        {
            var token = UserCookieUtility.GetUserInfoFromCookies(HttpContext);
            var user = _db.Users.GetUserById(userId, token.Token);

            return View(user);
        }


    }
}