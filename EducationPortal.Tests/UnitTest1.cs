using DateBaseServices;
using EducationPortal.Db.Models;
using EducationPortal.Db.Security;
using NUnit.Framework;

namespace EducationPortal.Tests
{
    public class Tests
    {
        private DataContext _db;
        private string _token;

        [SetUp]
        public void Setup()
        {
            _db = new DataContext();
            _token = SecurityService.GenerateToken(1);

        }

       [Test]
        public void AddUser()
        {
            var user = new User
            {
                IsAdmin = true,
                Login = "1",
                Password = "1",
                Name = "1"
            };
            _db.Users.AddOrUpdate(user);

        }

        [Test]
        public void AddCourse()
        {
            var course = new Course
            {
                Name = "3",
                Description = "Third course",
                Enable = true
            };

            _db.Courses.AddCourse(course, 1, _token);
        }

        [Test]
        public void EditCourse()
        {
            var course = new Course
            {
                CourseId = 2,
                Name = "2",
                Description = "Second course",
                Enable = true
            };

            _db.Courses.EditCourse(course, 1, _token);
        }

        [Test]
        public void AddUserInCourse()
        {
            var course = new Course
            {
                CourseId = 2,
                Name = "2",
                Description = "Second course",
                Enable = true
            };

            _db.Courses.AddUserInCourse(1, 2, _token);
        }

    }
}