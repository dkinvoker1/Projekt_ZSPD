using System.Data.Entity;

using Microsoft.AspNet.Identity.EntityFramework;

using ZSPD.Domain.Models.EntityModels;
using ZSPD.Domain.Models.EntityModels.Accounts;

namespace ZSPD.Domain.Models
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<Survey> Surveys { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<Grade> Grades { get; set; }
        public virtual DbSet<SubjectGraph> SubjectGraphs { get; set; }

        public DbSet<Psychologist> Psychologists { get; set; }
        public DbSet<Student> Students { get; set; }

        public ApplicationDbContext()
            : base("ZSPD_DB", throwIfV1Schema: false)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Survey>().HasMany(x => x.Questions).WithMany();
            modelBuilder.Entity<CompletedSurvey>().HasMany(x => x.Answers).WithRequired();


            modelBuilder.Entity<Psychologist>().ToTable("Psychologists");
            modelBuilder.Entity<Student>().ToTable("Users");
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
