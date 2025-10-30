using Application.Services.Location.Province;
using Application.Services.Period.Communication;
using Application.Services.UserService;
using AspNetCoreRateLimit;
using AutoMapper;
using Azmoon.Application.Container;
using Azmoon.Application.Interfaces;
using Azmoon.Application.Interfaces.Assessment;
using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Application.Interfaces.Facad;
using Azmoon.Application.Interfaces.Group;
using Azmoon.Application.Interfaces.QuizTemp;
using Azmoon.Application.Interfaces.User;
using Azmoon.Application.Interfaces.WorkPlace;
using Azmoon.Application.Service.Answer.Command;
using Azmoon.Application.Service.Assessment;
using Azmoon.Application.Service.Facad;
using Azmoon.Application.Service.Group.Query;
using Azmoon.Application.Service.JameiatQustion.Command;
using Azmoon.Application.Service.Login.Command;
using Azmoon.Application.Service.PublicRelations.CAPTCHA;
using Azmoon.Application.Service.PublicRelations.ChannelServices;
using Azmoon.Application.Service.PublicRelations.DashboardServices;
using Azmoon.Application.Service.PublicRelations.Location.City;
using Azmoon.Application.Service.PublicRelations.Location.Province;
using Azmoon.Application.Service.PublicRelations.MediaPerformances;
using Azmoon.Application.Service.PublicRelations.MembersServices;
using Azmoon.Application.Service.PublicRelations.MessengerServices;
using Azmoon.Application.Service.PublicRelations.NewsPerformancesServices;
using Azmoon.Application.Service.PublicRelations.OperatorServices;
using Azmoon.Application.Service.PublicRelations.Period.Communication;
using Azmoon.Application.Service.PublicRelations.Period.VirtualSpace;
using Azmoon.Application.Service.QuizTemp.Command;
using Azmoon.Application.Service.QuizTemp.Query;
using Azmoon.Application.Service.Result.Query;
using Azmoon.Application.Service.Role.Command;
using Azmoon.Application.Service.Survaeis.Answers;
using Azmoon.Application.Service.Survaeis.Questiones;
using Azmoon.Application.Service.Survaeis.Results;
using Azmoon.Application.Service.Survaeis.Survayy;
using Azmoon.Application.Service.User.Command;
using Azmoon.Application.Service.UserAccess.Query;
using Azmoon.Application.Service.WorkPlace.Query;
using Azmoon.Application.Service.WorkTime;
using Azmoon.Common.FileWork;
using Azmoon.Domain.Entities;
using Azmoon.Persistence.Contexts;
using Azmoon.Persistence.Helper;
using Azmoon.Persistence.Seeding;
using DNTCaptcha.Core;
using DotNet.RateLimiter;
using EndPoint.Site.Areas.PublicRelations.Helpers;
using EndPoint.Site.Configurations;
using EndPoint.Site.Helper.ActionFilter;
using EndPoint.Site.Helper.Hangfire;
using EndPoint.Site.Useful.Automapper;
using EndPoint.Site.Useful.Filter;
using EndPoint.Site.Useful.IOC;
using EndPoint.Site.Useful.Ultimite;
using Hangfire;
using Hangfire.SqlServer;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EndPoint.Site
{
    public class Startup
    {
        public static string WebRootPath { get; private set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            services.AddControllersWithViews();

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });
            services.AddMvc(options =>
            {
                //options.Filters.Add(typeof(SetViewDataFilter));
            });
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);//You can set Time   
                options.Cookie.HttpOnly = true; // امنیت بیشتر برای کوکیmd
                options.Cookie.IsEssential = true; // ضروری برای کارکرد اپلیکیشنmd

            });


            services.AddIdentity<User, Role>()
               .AddEntityFrameworkStores<DataBaseContext>()
               .AddDefaultTokenProviders()
               .AddRoles<Role>()
               .AddErrorDescriber<CustomIdentityError>()
               .AddPasswordValidator<MyPasswordValidator>();
            services.AddScoped<IUserClaimsPrincipalFactory<User>, CustomUserClaimsPrincipalFactory>();
            services.Configure<IdentityOptions>(option =>
            {
                //UserSetting
                //option.User.AllowedUserNameCharacters = "abcd123";
                option.User.RequireUniqueEmail = true;

                //Password Setting
                option.Password.RequireDigit = false;
                option.Password.RequireLowercase = false;
                option.Password.RequireNonAlphanumeric = false;//!@#$%^&*()_+
                option.Password.RequireUppercase = false;
                option.Password.RequiredLength = 6;
                option.Password.RequiredUniqueChars = 1;

                //Lokout Setting
                option.Lockout.MaxFailedAccessAttempts = 4;
                option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);

                //SignIn Setting
                option.SignIn.RequireConfirmedAccount = false;
                option.SignIn.RequireConfirmedEmail = false;
                option.SignIn.RequireConfirmedPhoneNumber = false;

            });


            services.AddDbContext<DataBaseContext>(
             p => p.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IDataBaseContext, DataBaseContext>();

            //services Inject
            services.AddInjectRoleAplication();
            services.AddInjectWorkPlaceAplication();
            // services.AddIOCUserInject();
            //services.AddIOCRezultQuizInject();
            //services.AddIOCQuizInject();
            //services.AddIOCQuizFilterInject();
            //services.AddIOCQuestionInject();
            //services.AddIOCGroupInject();
            //services.AddIOCAnswersInject();
            //**************************
            services.RegisterAutoMapper();
            services.AddDNTCaptcha(options =>
             options.UseCookieStorageProvider()
            .ShowThousandsSeparators(true)
             );
            //services.AddInitRateLimit(Configuration);

            services.AddScoped<Azmoon.Common.FileWork.IFileProvider, Azmoon.Common.FileWork.FileProvider>();
            services.AddScoped<IRegisterUser, RegisterUser>();
            services.AddTransient<IGetQuizTemp, GetQuizTemp>();
            services.AddTransient<IGetChildrenWorkPlace, GetChildrenWorkPlacees>();
            services.AddTransient<IGetChildrenGroup, GetChildrenGroup>();
            services.AddTransient<IAddQuizStartTemp, AddQuizStartTemp>();
            services.AddScoped<IGetWorkplacFirstToEndParent, GetWorkplacFirstToEndParent>();
            services.AddTransient<ICrudAnswerSurvay, CrudAnswerSurvay>();
            services.AddTransient<ICrudQuestionSurvay, CrudQuestionSurvay>();
            services.AddTransient<ICrudSurvayService, CrudSurvayService>();
            services.AddTransient<IReportChart, ReportChart>();
            services.AddTransient<IGetKarnameh, GetKarnameh>();

            //FacadeInject *********************
            services.AddScoped<IGroupFacad, GroupFacad>();
            services.AddScoped<IWorkPlaceFacad, WorkPlaceFacad>();
            services.AddScoped<IUserFacad, UserFacad>();
            services.AddScoped<IRoleFacad, RoleFacad>();
            services.AddScoped<IQuestionFacad, QuestionFacad>();
            services.AddScoped<IAnswerFacad, AnswerFacad>();
            services.AddScoped<IQuizFacad, QuizFacad>();
            services.AddScoped<IResultQuizFacad, ResultQuizFacad>();
            services.AddScoped<IQuizFilterFacad, QuizFilterFacad>();

            //*****************************
            services.AddScoped<IGeyActiveSurvey, GeyActiveSurvey>();
            services.AddScoped<IGetStartSurvay, GetStartSurvay>();
            services.AddTransient<IAddResultSurvay, AddResultSurvay>();
            services.AddTransient<IWriteAssessment, WriteAssessment>();
            services.AddTransient<IGetAssessment, GetAssessment>();
            services.AddTransient<IUserAccessService, UserAccessService>();
            services.AddTransient<IAddJameiatQustion, AddJameiatQustion>();
            services.AddTransient<IGetAssessmentUserAccessWorkPalce, GetAssessmentUserAccessWorkPalce>();
            services.AddTransient<IStarClientAssessment, StarClientAssessment>();
            services.AddTransient<IAddUserAnswerInAssessment, AddUserAnswerInAssessment>();
            services.AddTransient<IGetResultAssesment, GetResultAssesment>();     
            services.AddTransient<ILoginCRUD, LoginCRUD>();      
            services.AddTransient<IWorkTimeService, WorkTimeService>();
            ApplicationConfigureServiceContainer.AddServices(services);


            #region Public Relations Service
            services.AddTransient<IGetProvinceService, GetProvinceService>();
            services.AddTransient<IGetCityService, GetCityService>();
            services.AddTransient<IGetMessengerForDropDownService, GetMessengerForDropDownService>();
            services.AddTransient<IGetMessengerService, GetMessengerService>();
            services.AddTransient<IAddMessengerService, AddMessengerService>();
            services.AddTransient<IEditMessengerService, EditMessengerService>();
            services.AddTransient<IRemoveMessengerService, RemoveMessengerService>();
            services.AddTransient<IGetProvinceNameById, GetProvinceNameById>();
            services.AddTransient<IGetCityNameById, GetCityNameById>();
            services.AddTransient<IAddChannelServices, AddChannelServices>();
            services.AddTransient<IGetOperatorForDropDownService, GetOperatorForDropDownService>();
            services.AddTransient<IGetMassengerNameById, GetMassengerNameById>();
            services.AddTransient<IGetOperatorNameById, GetOperatorNameById>();
            services.AddTransient<IGetListChannelServices, GetListChannelServices>();
            services.AddTransient<IAddVirtualSpacePeriodService, AddVirtualSpacePeriodService>();
            services.AddTransient<IGetListVirtualSpacePeriodService, GetListVirtualSpacePeriodService>();
            services.AddTransient<IAddMessengerService, AddMessengerService>();
            services.AddTransient<IGetActiveVirtualSpacePeriodService, GetActiveVirtualSpacePeriodService>();
            services.AddTransient<IAddNewMembrsService, AddNewMembrsService>();
            services.AddTransient<IGetStatusVirtualSpacePeriodService, GetStatisticalStatus>();
            services.AddTransient<IGetLastMembersService, GetLastMembersService>();
            services.AddTransient<IGetOperatorServices, GetOperatorServices>();
            services.AddTransient<IGetListMembersService, GetListMembersService>();
            services.AddTransient<IChengeStatusUserService, ChengeStatusUserService>();
            services.AddTransient<IGetOperatorListServices, GetOperatorListServices>();
            services.AddTransient<IFindChannelService, FindChannelService>();
            services.AddTransient<IEditChannelService, EditChannelService>();
            services.AddTransient<IDeleteChannelService, DeleteChannelService>();
            services.AddTransient<IGetDetailVirtualSpacePeriodService, GetDetailVirtualSpacePeriodService>();
            services.AddTransient<IEditVirtualSpacePeriodService, EditVirtualSpacePeriodService>();
            services.AddTransient<IChannelAndGroupStatisticsServices, ChannelAndGroupStatisticsServices>();
            services.AddTransient<IGetTotalMembersFromMessengersService, GetTotalMembersFromMessengersService>();
            services.AddTransient<IAddMediaPerformancesService, AddMediaPerformancesService>();
            services.AddTransient<IGetListMediaPerformancesService, GetListMediaPerformancesService>();
            services.AddTransient<IAddNewsPerformancesService, AddNewsPerformancesService>();
            services.AddTransient<IGetListNewsPerformancesService, GetListNewsPerformancesService>();
            services.AddTransient<IAddCommunicationPeriodService, AddCommunicationPeriodService>();
            services.AddTransient<IGetListCommunicationPeriodService, GetListCommunicationPeriodService>();
            services.AddTransient<IFindActiveCommunicationPeriodService, FindActiveCommunicationPeriodService>();
            services.AddTransient<IGetStatusRegistrationOperatorNewsService, GetStatusRegistrationOperatorNewsService>();
            services.AddTransient<IGetStatusRegistrationOperatorMediaService, GetStatusRegistrationOperatorMediaService>();
            services.AddTransient<IGetInsertTimeForDropDownServices, GetInsertTimeForDropDownServices>();
            services.AddTransient<IGetYearsForDropDownServices, GetYearsForDropDownServices>();
            services.AddTransient<IGetNameByIdCommunicationPeriodService, GetNameByIdCommunicationPeriodService>();
            services.AddTransient<IGetNormalizedNameByNameService, GetNormalizedNameByNameService>();
            services.AddTransient<IGetNameByNormalizedNameService, GetNameByNormalizedNameService>();
            services.AddTransient<IDeleteCommunicationPeriodService, DeleteCommunicationPeriodService>();
            services.AddTransient<IDeleteVirtualSpacePeriodService, DeleteVirtualSpacePeriodService>();
            services.AddTransient<IEditCommunicationPeriodService, EditCommunicationPeriodService>();
            services.AddTransient<IDetailsCommunicationPeriodService, DetailsCommunicationPeriodService>();
            services.AddTransient<IChangeOfStatusNewsPerformancesService, ChangeOfStatusNewsPerformancesService>();
            services.AddTransient<IGetStatusNewsPerformances, GetStatusNewsPerformances>();
            services.AddTransient<IDeleteNewsPerformancesService, DeleteNewsPerformancesService>();
            services.AddTransient<IGetDetailNewsPerformanceService, GetDetailNewsPerformanceService>();
            services.AddTransient<IEditNewsPerformancesService, EditNewsPerformancesService>();
            services.AddTransient<IGetDataForViewBagMediaPerformancesService, GetDataForViewBagMediaPerformancesService>();
            services.AddTransient<IChangeOfStatusMediaPerformancesService, ChangeOfStatusMediaPerformancesService>();
            services.AddTransient<IGetStatusMediaPerformances, GetStatusMediaPerformances>();
            services.AddTransient<IDeleteMediaPerformancesService, DeleteMediaPerformancesService>();
            services.AddTransient<IGetDetailMediaPerformanceService, GetDetailMediaPerformanceService>();
            services.AddTransient<IEditMediaPerformancesService, EditMediaPerformancesService>();
            services.AddTransient<IReviewingMembersBasedOnTheStatisticalPeriod, ReviewingMembersBasedOnTheStatisticalPeriod>();
            services.AddTransient<IMemberFlow, MemberFlow>();
            services.AddTransient<IGetStatusVirtualSpacePeriodForDashboardService, GetStatusVirtualSpacePeriodForDashboardService>();
            services.AddTransient<ICaptchaService, CaptchaService>();
            services.AddScoped<IUserClaimsPrincipalFactory<User>, ApplicationUserClaimsPrincipalFactory>();
            services.AddScoped<IGetSubjectForDropDownService, GetSubjectForDropDownService>();

            #endregion
            // services.AddMediatR(typeof(Startup));

            //services.AddMediatR(Assembly.GetExecutingAssembly());
            /// filter 
            services.AddScoped<LayoutFilter>();
            services.AddRateLimitService(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            //Seed data on application startup
            //using (var serviceScope = app.ApplicationServices.CreateScope())
            //    {
            //    var dbContext = serviceScope.ServiceProvider.GetRequiredService<DataBaseContext>();

            //    dbContext.Database.Migrate();

            //    new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            //    }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseFastReport();

            app.UseRouting();
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();


            // HangFire Dashboard
            // app.UseHangfireDashboard();
            //app.UseHangfireDashboard("/hangfire", new DashboardOptions
            //{
            //    Authorization = new[] { new HangfireAuthorizationFilter() }
            //});
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                //endpoints.MapHangfireDashboard();
            });
            WebRootPath = env.WebRootPath;
        }

    }
}
