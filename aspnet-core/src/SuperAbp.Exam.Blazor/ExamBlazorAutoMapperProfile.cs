using SuperAbp.Exam.Blazor.Model;
using SuperAbp.Exam.Blazor.Pages;
using SuperAbp.Exam.ExamManagement.UserExams;
using SuperAbp.Exam.QuestionManagement.Questions;
using static SuperAbp.Exam.Blazor.Model.UserExamSectionViewModel;
using static SuperAbp.Exam.Blazor.Model.UserExamSectionViewModel.UserExamQuestionViewModel;
using AutoMapper;

namespace SuperAbp.Exam.Blazor;

public class ExamBlazorAutoMapperProfile : Profile
{
    public ExamBlazorAutoMapperProfile()
    {
        CreateMap<UserExamDetailDto.SectionDto, UserExamSectionViewModel>();
        CreateMap<UserExamDetailDto.SectionDto.QuestionDto, UserExamQuestionViewModel>();
        CreateMap<UserExamDetailDto.SectionDto.QuestionDto.OptionDto, UserExamQuestionAnswerViewModel>();
        CreateMap<QuestionDetailDto, QuestionViewModel>();
        CreateMap<UserExamQuestionViewModel, QuestionViewModel>();
        CreateMap<UserExamQuestionAnswerViewModel, QuestionAnswerViewModel>();
    }
}