using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azmoon.Domain.Entities;
using Azmoon.Domain.Entities.Surves;
using Azmoon.Domain.Entities.Template;
using Azmoon.Domain.Entities.PublicRelations.Main.Media;
using Azmoon.Domain.Entities.PublicRelations.Main.News;
using Azmoon.Domain.Entities.PublicRelations.Main;
using Azmoon.Domain.Entities.PublicRelations.Location;

namespace Azmoon.Application.Interfaces.Contexts
{
    public interface IDataBaseContext
    {
        DbSet<Domain.Entities.Role> Roles  { get; set; }
        DbSet<Domain.Entities.User> Users { get; set; }
        DbSet<Domain.Entities.Attachment> Attachments { get; set; }
        DbSet<Domain.Entities.Question> Qestions { get; set; }
        DbSet<Domain.Entities.Answer> Answers { get; set; }
        DbSet<Domain.Entities.Quiz> Quizzes { get; set; }
        DbSet<Domain.Entities.Result> Results { get; set; }
        DbSet<Domain.Entities.Group> Groups { get; set; }
        DbSet<Domain.Entities.persons> Persons { get; set; }
        DbSet<Domain.Entities.Password> Passwords { get; set; }
        DbSet<Domain.Entities.WorkPlace> WorkPlaces { get; set; }
        DbSet<Domain.Entities.QuizFilter> QuizFilters { get; set; }
        DbSet<Domain.Entities.QuizStartTemp> QuizStartTemps { get; set; }
        DbSet<Domain.Entities.GroupUser> GroupUsers { get; set; }
        DbSet<SurveyAnswerDescriptive> SurveyAnswerDescriptives { get; set; }
        DbSet<SurveyAnswer> SurveyAnswers { get; set; }
        DbSet<SurveyQuestion> SurveyQuestions { get; set; }
        DbSet<SurveyPopulation> SurveyPopulations { get; set; }
        DbSet<Survey> Surveys { get; set; }
        DbSet<SurvayResultAnswer> SurvayResultAnswers { get; set; }
        DbSet<TemplateMain> TemplateMains { get; set; }
        DbSet<TemplateQuestionAnswer> TemplateQuestionAnswers { get; set; }
        DbSet<TemplateUserResultAnswer> TemplateUserResultAnswers { get; set; }
        DbSet<Domain.Entities.Template.Assessment> Assessments { get; set; }
        DbSet<UserAccess> UserAccess { get; set; }
        DbSet<JameiatQustion> jameiatQustions { get; set; }  
        DbSet<TemplateDescriptiveAnswer> TemplateDescriptiveAnswers { get; set; }   
        DbSet<LeaveRequest> LeaveRequests { get; set; }  
        DbSet<LoginLog> LoginLogs { get; set; }  
        DbSet<AttendanceLog> AttendanceLogs { get; set; }
        DbSet<SchedulePeriod> SchedulePeriods { get; set; }
        DbSet<WeeklyWorkTime> WeeklyWorkTimes { get; set; }
        DbSet<TraficeUserAccess> TraficeUserAccesses { get; set; }

        #region PublicRelations
        DbSet<Province> Provinces { get; set; }
        DbSet<City> Cities { get; set; }
        DbSet<Messenger> Messengers { get; set; }
        DbSet<Channel> Channels { get; set; }
        DbSet<Operator> Operators { get; set; }
        DbSet<MembersPeriod> MembersPeriods { get; set; }
        DbSet<MediaPerformance> MediaPerformances { get; set; }
        DbSet<NewsPerformances> NewsPerformances { get; set; }
        DbSet<CommunicationPeriod> CommunicationPeriod { get; set; }
        DbSet<VirtualSpacePeriod> VirtualSpacePeriod { get; set; }
        #endregion





        int SaveChanges(bool acceptAllChangesOnSuccess);
        int SaveChanges();
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken());
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());

    }
}
