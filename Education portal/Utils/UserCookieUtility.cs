using System;
using EducationPortal.Db.Security;
using EducationPortal.Db.Services.Models;
using Microsoft.AspNetCore.Http;

namespace Education_portal.Utils
{
    /// <summary>
    /// Утилитарный класс, предоставляющий статические методы для сохранения в куки и получения данных из кук об авторизованном пользователе.
    /// </summary>
    public static class UserCookieUtility
    {
        /// <summary>
        /// Получает логин авторизованного пользователя и значение, показывающее, что пользователь имеет права администратора.
        /// </summary>
        /// <param name="context">Контекст запроса.</param>
        /// <returns>Кортеж значений, содержащий логин авторизованного пользователя и значение, показывающее, что пользователь имеет права администратора.</returns>
        public static (string, string, bool, string) GetSavedUser(HttpContext context)
        {
            if (!context.Request.Cookies.TryGetValue("Site-UserName", out var login))
                return (string.Empty, string.Empty, false, string.Empty);

            if (!context.Request.Cookies.TryGetValue("Site-Token", out var token))
                return (string.Empty, string.Empty, false, string.Empty);

            if (!context.Request.Cookies.TryGetValue("Site-UserId", out string userId))
                return (string.Empty, string.Empty, false, string.Empty);

            if (string.IsNullOrWhiteSpace(login))
                return (string.Empty, string.Empty, false, string.Empty);

            var isAdmin = false;
            if (context.Request.Cookies.TryGetValue("Site-UserAdmin", out var isAdminValue))
                bool.TryParse(isAdminValue, out isAdmin);

            return (userId, login, isAdmin, token);
        }

        /// <summary>
        /// Задает данные об авторизованном пользователе.
        /// </summary>
        /// <param name="context">Контекст запроса.</param>
        /// <param name="login">Логин пользователя.</param>
        /// <param name="isAdmin">Значение, показывающее, что пользователь имеет права администратора.</param>
        public static void SetSavedUser(HttpContext context, int userId, string login, bool isAdmin, string token)
        {
            context.Response.Cookies.Append("Site-UserId", userId.ToString(), new CookieOptions { Expires = DateTime.Now.AddDays(7) });
            context.Response.Cookies.Append("Site-UserName", login, new CookieOptions { Expires = DateTime.Now.AddDays(7) });
            context.Response.Cookies.Append("Site-UserAdmin", isAdmin.ToString(), new CookieOptions { Expires = DateTime.Now.AddDays(7) });
            context.Response.Cookies.Append("Site-Token", token.ToString(), new CookieOptions { Expires = DateTime.Now.AddDays(7) });
        }

        public static AuthorizeModel GetUserInfoFromCookies(HttpContext HttpContext)
        {
            var (userIdStr, login, isAdmin, token) = GetSavedUser(HttpContext);

            var userIdParsed = int.TryParse(userIdStr, out var userId);

            if (!userIdParsed)
                return new AuthorizeModel();

            if (login == null)
                return new AuthorizeModel();

            var tokenCheched = SecurityService.ValidateCurrentToken(token, userId);
            if (!tokenCheched)
                return new AuthorizeModel();

            var result = new AuthorizeModel{IsAdmin = isAdmin, Login = login, Token = token, UserId = userId};
            return result;
        }
    }
}