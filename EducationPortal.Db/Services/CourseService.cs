using System.Collections.Generic;
using System.Linq;
using EducationPortal.Db.Models;
using EducationPortal.Db.Security;
using Microsoft.EntityFrameworkCore;

namespace DateBaseServices.Services
{
    using Exceptions;
    using System;

    public class CourseService : DefaultService
    {
        internal CourseService(DataContext db) : base(db)
        {
        }

        public void AddCourse(Course course, int userId, string token)
        {
            if (!SecurityService.ValidateCurrentToken(token, userId))
                throw new DbServiceException($"Токен({token}) недействителен.");

            if (course == null)
                throw new DbServiceException("Экземпляр курса не задан.");

            if (userId < 1)
                throw new DbServiceException("Неверный идентификатор пользователя.");

            var user = _db.DbUsers.Find(userId);

            if (!user.IsAdmin)
                throw new DbServiceException("У вас нет прав на добавление курсов.");

            var createdCourse = _db.DbCourses.Add(course);
            _db.SaveChanges();
        }

        public void EditCourse(Course course, int userId, string token)
        {
            if (!SecurityService.ValidateCurrentToken(token, userId))
                throw new DbServiceException($"Токен({token}) недействителен.");

            if (course == null)
                throw new DbServiceException("Экземпляр курса не задан.");

            if (userId < 1)
                throw new DbServiceException("Неверный идентификатор пользователя.");

            var user = _db.DbUsers.Find(userId);

            if (!user.IsAdmin)
                throw new DbServiceException("У вас нет прав на добавление курсов.");

            var currentCourse = _db.DbCourses.Find(course.CourseId);
            if (currentCourse == null)
                throw new DbServiceException("Не удалось найти запись с данным идентификатором для обновления.");

            currentCourse.Name = course.Name;
            currentCourse.Description = course.Description;
            currentCourse.Enable = course.Enable;

            var createdNote = _db.DbCourses.Update(currentCourse);

            _db.SaveChanges();
        }

        public List<Course> GetCoursesByUserId(int userId, string token)
        {
            if (!SecurityService.ValidateCurrentToken(token, userId))
                throw new DbServiceException($"Токен({token}) недействителен.");

            if (userId < 1)
                throw new DbServiceException("Неверный идентификатор пользователя.");

            var courses = _db.UserCourseLinkers
                .Include(l => l.Course)
                .Where(l => l.UserId == userId)
                .Select(l => l.Course).ToList();

            return courses;
        }

        public List<Course> GetAllCourses()
        {
            var courses = _db.DbCourses
                .Where(c => c.Enable)
                .ToList();
            return courses;
        }

        public Course GetCourseByCourseId(int courseId)
        {
            var course = _db.DbCourses
                .FirstOrDefault(c => c.CourseId == courseId);
            return course;
        }


        public void AddUserInCourse(int courseId, int userId, string token)
        {
            if (!_db.Users.UserIsAdminByToken(token))
                throw new DbServiceException($"Токен({token}) недействителен.");

            if (courseId < 1)
                throw new DbServiceException("Неверный идентификатор курса.");

            if (userId < 1)
                throw new DbServiceException("Неверный идентификатор пользователя.");


            ////TODO: здесь будет вызов логики с покупкой курса.

            _db.UserCourseLinkers.Add(new UserCourseLinker { CourseId = courseId, UserId = userId });
            _db.SaveChanges();
        }
    }
}