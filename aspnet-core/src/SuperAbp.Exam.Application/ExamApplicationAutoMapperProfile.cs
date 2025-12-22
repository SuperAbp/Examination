using AutoMapper;
using SuperAbp.Exam.Favorites;
using SuperAbp.Exam.MistakesReviews;
using SuperAbp.Exam.TrainingManagement;

namespace SuperAbp.Exam;

public class ExamApplicationAutoMapperProfile : Profile
{
    public ExamApplicationAutoMapperProfile()
    {
        CreateMap<Training, TrainingListDto>();

        CreateMap<FavoriteWithDetails, FavoriteListDto>();

        CreateMap<MistakeWithDetails, MistakesReviewListDto>();
    }
}