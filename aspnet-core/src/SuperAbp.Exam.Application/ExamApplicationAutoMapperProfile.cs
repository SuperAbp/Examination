using AutoMapper;
using SuperAbp.Exam.Favorites;
using SuperAbp.Exam.QuestionManagement.QuestionRepos;
using SuperAbp.Exam.TrainingManagement;

namespace SuperAbp.Exam;

public class ExamApplicationAutoMapperProfile : Profile
{
    public ExamApplicationAutoMapperProfile()
    {
        CreateMap<Training, TrainingListDto>();

        CreateMap<FavoriteWithDetails, FavoriteListDto>();
    }
}