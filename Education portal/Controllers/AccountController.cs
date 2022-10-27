using System;
using DateBaseServices;
using Education_portal.Utils;
using EducationPortal.Db.Models;
using EducationPortal.Db.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Education_portal.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _db;

        public AccountController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _db = new DataContext();
        }

        /// <summary>
        /// Обрабатывает запрос на переход к главной странице контроллера управления аккаунтами.
        /// </summary>
        /// <returns>Результат выполнения запроса.</returns>
        [HttpGet]
        public IActionResult Index()
        {
            // Выполняет переадресацию на страницу авторизации (метод Login() контроллера управления аккаунтами).
            return RedirectToAction("Login");
        }

        /// <summary>
        /// Обрабатывает запрос на переход к главной странице контроллера управления аккаунтами.
        /// </summary>
        /// <returns>Результат выполнения запроса.</returns>
        [HttpGet]
        public IActionResult MyCourses()
        {
            var currentUser = UserCookieUtility.GetUserInfoFromCookies(HttpContext);
            var courses = _db.Courses.GetCoursesByUserId(currentUser.UserId, currentUser.Token);

            // Выполняет переадресацию на страницу авторизации (метод Login() контроллера управления аккаунтами).
            return View(courses);
        }


        /// <summary>
        /// Обрабатывает запрос на переход к главной странице контроллера управления аккаунтами.
        /// </summary>
        /// <returns>Результат выполнения запроса.</returns>
        [HttpGet]
        public IActionResult Logout()
        {
            UserCookieUtility.SetSavedUser(HttpContext, 0, string.Empty, false, string.Empty);
            // Выполняет переадресацию на страницу авторизации (метод Login() контроллера управления аккаунтами).
            return RedirectToAction("Index", "Home");
        }


        /// <summary>
        /// Обрабатывает запрос на переход к странице авторизации.
        /// </summary>
        /// <returns>Результат выполнения запроса.</returns>
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Обрабатывает запрос на авторизацию.
        /// </summary>
        /// <param name="model">Модель отображения учетных данных пользователя на странице авторизации.</param>
        /// <returns>Результат выполнения запроса.</returns>
        [HttpPost]
        public IActionResult Login(User model)
        {
            var token = new AuthorizeModel();
            // Вызывает проверку соответствия логина и пароля введенных пользователем с сохраненными в БД.
            // Если проверка не пройдена, возвращает страницу ошибки авторизации.
            try{
                token = _db.Users.Authorize(model.Login, model.Password);
            }
            catch (Exception e){
                return RedirectToAction("Login");
            }

            // Сохраняет авторизованного пользователя в куки и выполняет переадресацию
            // на главную страницу приложения (метод Index контроллера главной страницы).
            UserCookieUtility.SetSavedUser(HttpContext, token.UserId, token.Login, token.IsAdmin, token.Token);

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Открывает страницу регистрации пользователя.
        /// </summary>
        /// <returns>Результат выполнения запроса.</returns>
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Обрабатывает запрос на регистрацию пользователя.
        /// </summary>
        /// <param name="model">Модель отображения учетных данных пользователя на странице регистрации пользователя.</param>
        /// <returns>Результат выполнения запроса.</returns>
        [HttpPost]
        public IActionResult Register(User model)
        {
            try{
                if (string.IsNullOrEmpty(model.Login) || string.IsNullOrEmpty(model.Password))
                    throw new Exception();

                _db.Users.AddOrUpdate(model);
            }
            catch (Exception e)
            {
                return RedirectToAction("Register");
            }

            return RedirectToAction("Login");
        }


    }
}