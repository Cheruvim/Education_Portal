using System.Collections.Generic;

namespace EducationPortal.Db.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Enable { get; set; }

        public List<UserCourseLinker> UserCourseLinkers { get; set; }
    }
}