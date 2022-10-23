
namespace EducationPortal.Db.Models
{
    public class UserCourseLinker
    {
        public int LinkerId { get; set; }
        public int CourseId { get; set; }
        public int UserId { get; set; }

        public User User { get; set; }
        public Course Course { get; set; }
    }
}