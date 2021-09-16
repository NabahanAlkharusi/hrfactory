using HumanResourceHelth.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.DataAccess
{
    public class HumanResourceContext : DbContext
    {
        public DbSet<SurveyType> SurveyTypes { get; set; }
        public DbSet<Survey> Surveys{ get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<SurveyResult> SurveyResults { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Indicator> Indicators { get; set; }
        public DbSet<Industry> Industries { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Section> Sections{ get; set; }

        public DbSet<PluginRequest> PluginRequests { get; set; }
        public DbSet<TechRequest> TechRequests { get; set; }
        public DbSet<DoctorRequest> DoctorRequests { get; set; }

     //   public DbSet<Course> Courses { get; set; }
        public DbSet<CourseRate> CourseRates { get; set; }
        public DbSet<UserCourse> UserCourses { get; set; }
        public DbSet<UserWatchVideo> UserWatchVideos { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<UserPlan> UserPlans { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<DocFile> Documents { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Attachments> Attachments { get; set; }
        public DbSet<Course> Course { get; set; }


        public DbSet<UserReview> UserReview { get; set; }
        public DbSet<UserCourseView> UserCourseView { get; set; }
        public DbSet<Departments> Departments { get; set; }
        public DbSet<Pyments> Pyments { get; set; }
        public DbSet<Specialties> Specialties { get; set; }
        public DbSet<HrSetting> HrSetting { get; set; }
        public DbSet<Coupons> coupons { get; set; }
        public DbSet<IntroVedio> introVedios { get; set; }

        public HumanResourceContext() : base("HumanResourceContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<HumanResourceContext, Migrations.Configuration>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
        public string GetConstr()
        {
            return Database.Connection.ConnectionString;
        }

        public System.Data.Entity.DbSet<HumanResourceHelth.Model.Trainers> Trainers { get; set; }
    }
}
