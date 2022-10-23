using System.Linq;
using DateBaseServices;
using DateBaseServices.Exceptions;
using DateBaseServices.Services;
using DateBaseServices.Services.Models;
using EducationPortal.Db.Models;
using EducationPortal.Db.Security;

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
            if (!SecurityService.ValidateCurrentToken(token, user.UserId))
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

        public User GetUserById(int userId)
        {
            if (userId < 1)
                throw new DbServiceException("Указан некорректный идентификатор пользователя.");

            var user = _db.DbUsers.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
                throw new DbServiceException("Не удалось найти пользователя с данным идентификатором.");

            return new User {UserId = user.UserId, Login = user.Login, Name = user.Name, IsAdmin = user.IsAdmin};
        }

        public AuthorizeModel Authorize(string login, string password)
        {
            var user = _db.DbUsers.FirstOrDefault(x => x.Login == login && x.Password == password);
            if (user == null)
                throw new DbServiceException("Пользователя с данной парой логин - пароль не найдено.");

            var token =  SecurityService.GenerateToken(user.UserId);
            return new AuthorizeModel {Token = token, UserId = user.UserId, Login = user.Login, IsAdmin = user.IsAdmin};
        }
    }
}