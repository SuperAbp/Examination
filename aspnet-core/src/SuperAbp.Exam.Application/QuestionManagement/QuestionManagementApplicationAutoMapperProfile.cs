using AutoMapper;
using SuperAbp.Exam.QuestionManagement.QuestionRepos;

namespace SuperAbp.Exam.QuestionManagement;

public class QuestionManagementApplicationAutoMapperProfile : Profile
{
    public QuestionManagementApplicationAutoMapperProfile()
    {
        CreateMap<QuestionRepo, QuestionRepoListDto>();
        CreateMap<QuestionRepo, QuestionRepoDetailDto>();
    }
}