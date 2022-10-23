using System.Collections.Generic;

namespace EducationPortal.Db.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }

        public List<UserCourseLinker> UserCourseLinkers { get; set; }
    }
}