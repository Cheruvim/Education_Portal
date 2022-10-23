
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationPortal.Db.Models.Configs
{
    public class UserCourseLinkerConfig : IEntityTypeConfiguration<UserCourseLinker>
    {
        public void Configure(EntityTypeBuilder<UserCourseLinker> builder)
        {
            builder.ToTable("UserCourseLinker");
            builder.HasKey(p => p.LinkerId);

            builder.HasOne(p => p.Course)
                .WithMany(t => t.UserCourseLinkers)
                .HasForeignKey(p => p.CourseId)
                .HasPrincipalKey(t => t.CourseId);

            builder.HasOne(p => p.User)
                .WithMany(t => t.UserCourseLinkers)
                .HasForeignKey(p => p.UserId)
                .HasPrincipalKey(t => t.UserId);


        }
    }
}