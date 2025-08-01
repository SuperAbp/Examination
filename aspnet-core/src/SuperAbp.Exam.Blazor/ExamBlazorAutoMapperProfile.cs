using AutoMapper;
using SuperAbp.Exam.Blazor.Model;
using SuperAbp.Exam.Blazor.Pages;
using SuperAbp.Exam.ExamManagement.UserExams;
using SuperAbp.Exam.QuestionManagement.Questions;
using SuperAbp.Exam.TrainingManagement;

namespace SuperAbp.Exam.Blazor;

public class ExamBlazorAutoMapperProfile : Profile
{
    public ExamBlazorAutoMapperProfile()
    {
        CreateMap<UserExamDetailDto.QuestionDto.OptionDto, QuestionAnswerViewModel>();
        CreateMap<UserExamDetailDto.QuestionDto, QuestionViewModel>();
        CreateMap<QuestionDetailDto, QuestionViewModel>();
    }
}