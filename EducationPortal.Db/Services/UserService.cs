using System.Collections.Generic;
using System.Linq;
using DateBaseServices;
using DateBaseServices.Exceptions;
using DateBaseServices.Services;
using EducationPortal.Db.Models;
using EducationPortal.Db.Security;
using EducationPortal.Db.Services.Models;

namespace EducationPortal.Db.Services
{
    public class UserService : DefaultService
    {
        internal UserService(DataContext db) : base(db)
        {
        }

        public void AddOrUpdate(User user, string token = null)
        {
            if (user.UserId < 1)
            {
                AddUser(user);
            }
            else
            {
                UpdateUser(user, token);
            }
        }

        private void AddUser(User user)
        {
            if (string.IsNullOrEmpty(user.Password))
                throw new DbServiceException("Введите пароль.");

            if (_db.DbUsers.FirstOrDefault(u => u.Login == user.Login) != null)
                throw new DbServiceException("Пользователь с данным логином уже существует.");

            _db.DbUsers.Add(user);
            _db.SaveChanges();
        }

        private void UpdateUser(User user, string token)
        {
            var userId = SecurityService.GetUserIdFromToken(token);
            var isAdmin = _db.DbUsers.FirstOrDefault(u => u.UserId == userId);

            if (!(userId == user.UserId || isAdmin is { IsAdmin: true }))
                throw new DbServiceException($"Токен({token}) недействителен.");

            var currentUser = _db.DbUsers.FirstOrDefault(u => u.UserId == user.UserId && u.Login == user.Login);

            if (currentUser == null)
                throw new DbServiceException("Пользователя с данным id не существует.");

            if (!string.IsNullOrEmpty(user.Password))
                currentUser.Password = user.Password;

            if (!string.IsNullOrEmpty(user.Name))
                currentUser.Name = user.Name;

            _db.DbUsers.Update(currentUser);
            _db.SaveChanges();
        }

        public (User user, List<Course> courses) GetUserById(int userId, string token)
        {
            if(!UserIsAdminByToken(token))
                throw new DbServiceException("Нет прав на просмотр информации о пользователе.");

            if (userId < 1)
                throw new DbServiceException("Указан некорректный идентификатор пользователя.");

            var usertemp = _db.DbUsers.FirstOrDefault(u => u.UserId == userId);
            if (usertemp == null)
                throw new DbServiceException("Не удалось найти пользователя с данным идентификатором.");

            var user = new User
                { UserId = usertemp.UserId, Login = usertemp.Login, Name = usertemp.Name, IsAdmin = usertemp.IsAdmin };
            var courses =_db.Courses.GetCoursesByUserId(userId, token);

            return (user, courses);
        }

        public AuthorizeModel Authorize(string login, string password)
        {
            var user = _db.DbUsers.FirstOrDefault(x => x.Login == login && x.Password == password);
            if (user == null)
                throw new DbServiceException("Пользователя с данной парой логин - пароль не найдено.");

            var token =  SecurityService.GenerateToken(user.UserId);
            return new AuthorizeModel {Token = token, UserId = user.UserId, Login = user.Login, IsAdmin = user.IsAdmin};
        }

        public bool UserIsAdminByToken(string token)
        {
            var userId = SecurityService.GetUserIdFromToken(token);
            var isAdmin = _db.DbUsers.FirstOrDefault(u => u.UserId == userId);

            return isAdmin is { IsAdmin: true };
        }

        public List<User> GetAllUsers(string token)
        {
            if(!UserIsAdminByToken(token))
                throw new DbServiceException("Нет прав на просмотр информации о пользователе.");

            var users = _db.DbUsers.ToList();

            return users;
        }

    }
}