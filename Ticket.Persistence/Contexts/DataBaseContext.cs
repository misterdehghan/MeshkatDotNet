using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Domain.Entities;
using Azmoon.Domain.Entities.PublicRelations.Location;
using Azmoon.Domain.Entities.PublicRelations.Main;
using Azmoon.Domain.Entities.PublicRelations.Main.Media;
using Azmoon.Domain.Entities.PublicRelations.Main.News;
using Azmoon.Domain.Entities.Surves;
using Azmoon.Domain.Entities.Template;
using Azmoon.Persistence.Seeding;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Persistence.Contexts
{
    public class DataBaseContext : IdentityDbContext<User, Role, string>, IDataBaseContext
    {
        public DataBaseContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            //modelBuilder.Entity<UserLogin>().HasKey(p => new { p.ProviderKey, p.LoginProvider });
            //modelBuilder.Entity<UserRole>().HasKey(p => new { p.UserId, p.RoleId });
            //modelBuilder.Entity<UserToken>().HasKey(p => new { p.UserId, p.LoginProvider });
            modelBuilder.Entity<User>().Ignore(p => p.NormalizedEmail);
            modelBuilder.Entity<Answer>().Property(p => p.UserId).IsRequired();



            modelBuilder.Entity<Answer>()
             .HasOne(p => p.Question)
             .WithMany(p => p.Answers)
             .HasConstraintName("FK_Question_AnswerQuestions_onetomany")
             .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<SurveyQuestion>()
                .HasOne(p => p.Survey)
                .WithMany(p => p.SurveyQuestions)
                .HasConstraintName("FK_SurveyQuestion_Surveys_onetomany")
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<SurveyAnswer>()
                .HasOne(p => p.Survey)
                .WithMany(p => p.SurveyAnswers)
                .HasConstraintName("FK_Survey_SurveyAnswers_onetomany")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Quiz>()
                .HasOne(p => p.Passworddd)
                .WithOne(p => p.Quiz)
                .HasForeignKey<Password>(c => c.QuizId);

            modelBuilder.Entity<Quiz>()
                .HasOne(p => p.QuizFilter)
                .WithOne(p => p.Quiz)
                .HasForeignKey<QuizFilter>(c => c.QuizId);

            // تنظیم رابطه یک‌به‌چند
            modelBuilder.Entity<SchedulePeriod>()
                .HasMany(s => s.WeeklyWorkTimes)
                .WithOne(w => w.SchedulePeriod)
                .HasForeignKey(w => w.SchedulePeriodId)
                .OnDelete(DeleteBehavior.Cascade);

            //add default role and user
            initUser(modelBuilder);

            //Data Seed Public Relations
     //  DataSeederPublicRelations.SeedData(modelBuilder);

         //  modelBuilder.TicketSeed();
            base.OnModelCreating(modelBuilder);
        }


        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Question> Qestions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<persons> Persons { get; set; }
        public DbSet<Password> Passwords { get; set; }
        public DbSet<QuizStartTemp> QuizStartTemps { get; set; }
        public DbSet<GroupUser> GroupUsers { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<WorkPlace> WorkPlaces { get; set; }
        public virtual DbSet<QuizFilter> QuizFilters { get; set; }


        //************* survay *******************

        public DbSet<SurveyAnswerDescriptive> SurveyAnswerDescriptives { get; set; }
        public DbSet<SurveyAnswer> SurveyAnswers { get; set; }
        public DbSet<SurveyQuestion> SurveyQuestions { get; set; }
        public DbSet<SurveyPopulation> SurveyPopulations { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<SurvayResultAnswer> SurvayResultAnswers { get; set; }

        //************* Template*******************
        public DbSet<TemplateMain> TemplateMains { get; set; }
        public DbSet<TemplateQuestionAnswer> TemplateQuestionAnswers { get; set; }
        public DbSet<TemplateUserResultAnswer> TemplateUserResultAnswers { get; set; }
        public DbSet<Assessment> Assessments { get; set; }
        public DbSet<UserAccess> UserAccess { get; set; }
        public DbSet<JameiatQustion> jameiatQustions { get; set; }
        public DbSet<TemplateDescriptiveAnswer> TemplateDescriptiveAnswers { get; set; } 
        public DbSet<SchedulePeriod> SchedulePeriods { get; set; } 
        public DbSet<WeeklyWorkTime> WeeklyWorkTimes { get; set; } 
        public DbSet<TraficeUserAccess> TraficeUserAccesses { get; set; }

        #region PublicRelations
        public DbSet<Province> Provinces { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Messenger> Messengers { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<Operator> Operators { get; set; }
        public DbSet<MembersPeriod> MembersPeriods { get; set; }
        public DbSet<MediaPerformance> MediaPerformances { get; set; }
        public DbSet<NewsPerformances> NewsPerformances { get; set; }
        public DbSet<CommunicationPeriod> CommunicationPeriod { get; set; }
        public DbSet<VirtualSpacePeriod> VirtualSpacePeriod { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<LoginLog> LoginLogs { get; set; }
        public DbSet<AttendanceLog> AttendanceLogs { get; set; }
        #endregion

        private void initUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Ignore(p => p.Email);
            modelBuilder.Entity<User>().Ignore(p => p.EmailConfirmed);
            modelBuilder.Entity<User>().Ignore(p => p.NormalizedEmail);
            modelBuilder.Entity<User>().Property(p => p.FirstName).HasColumnType("nvarchar(50)");
            modelBuilder.Entity<User>().Property(p => p.LastName).HasColumnType("nvarchar(50)");
            modelBuilder.Entity<User>().Property(p => p.Phone).HasColumnType("nvarchar(11)");
            modelBuilder.Entity<User>().Property(p => p.UserName).HasColumnType("nvarchar(10)");
        }
    }
}
