using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azmoon.Application.Service.Answer.Dto;

using Azmoon.Application.Service.Question.Dto;

using Azmoon.Application.Service.User.Dto;
using Azmoon.Application.Service.Group.Dto;
using Azmoon.Domain.Entities;
using Azmoon.Application.Service.Quiz.Dto;
using Azmoon.Application.Service.WorkPlace.Dto;
using Azmoon.Application.Service.Filter.Dto;
using Azmoon.Application.Service.QuizTemp.Dto;
using Azmoon.Application.Service.Result.Dto;
using Azmoon.Domain.Entities.Surves;
using Azmoon.Application.Service.Survaeis.Survayy;
using Azmoon.Application.Service.Survaeis.SurveyGroups;
using Azmoon.Application.Service.Survaeis.Questiones;
using Azmoon.Application.Service.Survaeis.Answers;
using Azmoon.Application.Service.WorkPlace.Query;
using Azmoon.Common.Useful;
using Azmoon.Domain.Entities.Template;
using Azmoon.Application.Service.JameiatQustion.Dto;

namespace Azmoon.Application.AutoMapper
{
    public class DomainToViewModelProfile : Profile
    {
   
        public DomainToViewModelProfile()
        {
            //  CreateMap<Course, CourseViewModel>();
            CreateMap<User, Application.Service.Filter.Dto.Result>()
                 .ForMember(ds => ds.id,
            src => src.MapFrom(src => src.UserName))
                  .ForMember(ds => ds.text,
            src => src.MapFrom(src => src.FirstName+" "+ src.LastName));
            CreateMap<Group, GetGroupViewModel>();
            CreateMap<WorkPlace, GetWorkPlaceViewModel>().ReverseMap();
            CreateMap<JameiatQustion, AddJameiatQustionDto>().ReverseMap();
            CreateMap<User, UserShowAdminDto>();
            CreateMap<User, RegisterUserDto>();
            CreateMap<User, UpdateUserDto>().ReverseMap(); 
            CreateMap<QuizStartTemp, GetQuizTempDto>().ReverseMap();
            CreateMap<Role, GetRoleDto>();
            CreateMap<Quiz, GetQuizDto>()
                .ForMember(ds => ds.FilterStatus,
            src => src.MapFrom(src => src.QuizFilter!=null?true:false))
                .ForMember(ds => ds.GroupName,
            src => src.MapFrom(src => src.Group != null ? src.Group.Name : ""))
                .ReverseMap();
            CreateMap<Quiz, QuizAssignViewModel>().ReverseMap();
            CreateMap<Quiz, GetQuizDetilesDto>().ReverseMap();
            CreateMap<Question, GetQuestionViewModel>().ReverseMap(); 
            CreateMap<Question, AddQuestionViewModel>().ReverseMap(); 
            CreateMap<Answer, GetAnswerDto>().ReverseMap(); 
            CreateMap<Answer, AddAnswerDto>().ReverseMap(); 
            CreateMap<QuizStartTemp, AddQuizTempDto>().ReverseMap();
            CreateMap<Domain.Entities.Result, QuizReportDro>()
                .ForMember(ds => ds.FullName,
            src => src.MapFrom(src => src.Student != null ? src.Student.FirstName + " " + src.Student.LastName : ""))
                .ForMember(ds => ds.Phone,
            src => src.MapFrom(src => src.Student != null ? src.Student.Phone : ""))
                .ForMember(ds => ds.QuizStart,
            src => src.MapFrom(src => src.RegesterAt != null ? ((DateTime)src.RegesterAt).ToPersianDateStrFarsi() : ""))
                .ForMember(ds => ds.Darsad,
            src => src.MapFrom(src => (src.Points * 100) / src.MaxPoints))
                ;
            CreateMap<Domain.Entities.Result, GetResutQuizDto>()
                .ForMember(ds => ds.UserName,
            src => src.MapFrom(src => src.Student != null ? src.Student.FirstName+" "+ src.Student.LastName : ""))
                .ForMember(ds => ds.PhoneNumber,
            src => src.MapFrom(src => src.Student != null ? src.Student.Phone : ""))
                .ReverseMap();

            CreateMap<Survey, GetSurvayDto>()

        .ForMember(ds => ds.GroupName,
         src => src.MapFrom(src => src.Group != null ? src.Group.Name : ""))
        .ReverseMap();
            CreateMap<Survey, GetSurvayViewModel>()
                    .ForMember(ds => ds.UniqKey,
            src => src.MapFrom(src => src.UniqKey))
                .ReverseMap(); 
            CreateMap<GetSurvayDto, Survey>();
            CreateMap<Survey, AddSurvayDto>().ReverseMap();


            CreateMap<SurveyQuestion, AddQuestionSurvayDto>().ReverseMap();
            CreateMap<SurveyQuestion, GetQuestionSurvayViewModel>().ReverseMap();


            CreateMap<SurveyAnswer, GetAnswerInSurvayViewModel>().ReverseMap();
            CreateMap<SurveyAnswer, AddAnswerSurvayDto>().ReverseMap();
            CreateMap<SurveyAnswer, GetAnswerSurvayViewModel>().ReverseMap();




        }

     
    }
}
