using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.Survaeis.Results.Dto
{
   public class ReportAmarDto
    {
        public long survayId { get; set; }
        public string survayTitle { get; set; }
        public List<ReportAmarQuestionDto> GetQuestions { get; set; }
    }

    public class ReportSurvayDescDto
        {
        public long survayId { get; set; }
        public string survayTitle { get; set; }
        public List<ReportSurvayDescQuestionDto> GetAnseres { get; set; }
        }
    public class ReportSurvayDescQuestionDto
        {
        public long QuestionId { get; set; }
        public string QuestionTitle { get; set; }
        public string Text { get; set; }
        public string Ip { get; set; }  
        public string WorkPlaceUser { get; set; }

        }
    public class ReportAmarQuestionDto
    {
        public long QuestionId { get; set; }
        public string QuestionTitle { get; set; }
        public long AvrageWeight { get; set; }
        public List<ReportAmarAnswerDto> GetAnswers { get; set; }
 
    }
    public class ReportAmarAnswerDto
    {
        public string Title { get; set; }
        public long Weight { get; set; }
        public int counter { get; set; }
    }
    public class ReportSurvayWorckPlaceDto
        {
        public long Id { get; set; }
        public string WorckPlaceTitle { get; set; }
        public int Nafarat { get; set; }
        public long? ParentId { get; set; }
        }
    public class ReportAmarNewDto
    {
        public long survayId { get; set; }
        public string survayTitle { get; set; }
        public List< string> survayQuestionTitle { get; set; }
        public List<List<string>> GetAnswers { get; set; } = new List<List<string>>();
    }
}
