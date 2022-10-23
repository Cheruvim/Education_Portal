using DateBaseServices.Services;
using EducationPortal.Db.Models;
using EducationPortal.Db.Models.Configs;
using EducationPortal.Db.Services;

namespace DateBaseServices
{
    using Microsoft.EntityFrameworkCore;

    public class DataContext : DbContext
    {
        internal DbSet<User> DbUsers { get; set; }
        internal DbSet<Course> DbCourses { get; set; }
        internal DbSet<UserCourseLinker> UserCourseLinkers { get; set; }

        public UserService Users { get; }
        public CourseService Courses { get; }

        public DataContext()
        {
            Database.EnsureCreated();

            Users = new UserService(this);
            Courses = new CourseService(this);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new CourseConfig());
            modelBuilder.ApplyConfiguration(new UserCourseLinkerConfig());

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=Edu_Portal;User Id=root;Password=root; TrustServerCertificate=True;");
        }
    }

}