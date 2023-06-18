using AutoMapper;
using SuperAbp.Exam.Admin.QuestionManagement.QuestionAnswers;
using SuperAbp.Exam.QuestionManagement.QuestionAnswers;
using SuperAbp.Exam.QuestionManagement.QuestionRepos;
using SuperAbp.Exam.QuestionManagement.Questions;

namespace SuperAbp.Exam.QuestionManagement;

public class QuestionManagementApplicationAutoMapperProfile : Profile
{
    public QuestionManagementApplicationAutoMapperProfile()
    {
        CreateMap<QuestionRepo, QuestionRepoListDto>();
        CreateMap<QuestionRepo, QuestionRepoDetailDto>();

        CreateMap<Question, QuestionListDto>();
        CreateMap<Question, QuestionDetailDto>();

        CreateMap<QuestionAnswer, QuestionAnswerListDto>();
    }
}