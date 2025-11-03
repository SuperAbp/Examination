using AutoMapper;
using SuperAbp.Exam.Blazor.Model;
using SuperAbp.Exam.ExamManagement.UserExams;
using static SuperAbp.Exam.Blazor.Model.UserExamSectionViewModel;
using static SuperAbp.Exam.Blazor.Model.UserExamSectionViewModel.UserExamQuestionViewModel;

namespace SuperAbp.Exam.Blazor;

public class ExamBlazorAutoMapperProfile : Profile
{
    public ExamBlazorAutoMapperProfile()
    {
        CreateMap<UserExamDetailDto.SectionDto, UserExamSectionViewModel>();
        CreateMap<UserExamDetailDto.SectionDto.QuestionDto, UserExamQuestionViewModel>();
        CreateMap<UserExamDetailDto.SectionDto.QuestionDto.OptionDto, UserExamQuestionAnswerViewModel>();
    }
}