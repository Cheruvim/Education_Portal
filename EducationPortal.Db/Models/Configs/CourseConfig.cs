
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationPortal.Db.Models.Configs
{
    public partial class CourseConfig : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.ToTable("Courses");
            builder.HasKey(p => p.CourseId);

            builder.HasMany(p => p.UserCourseLinkers)
                .WithOne(t => t.Course)
                .HasForeignKey(p => p.CourseId)
                .HasPrincipalKey(t => t.CourseId);
        }
    }
}