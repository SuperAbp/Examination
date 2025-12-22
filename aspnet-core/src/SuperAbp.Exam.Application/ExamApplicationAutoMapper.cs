using AutoMapper;
using SuperAbp.Exam.Favorites;
using SuperAbp.Exam.MistakesReviews;
using SuperAbp.Exam.TrainingManagement;

namespace SuperAbp.Exam;

public class ExamApplicationAutoMapper : Profile
{
    public ExamApplicationAutoMapper()
    {
        CreateMap<Training, TrainingListDto>();

        CreateMap<FavoriteWithDetails, FavoriteListDto>();

        CreateMap<MistakeWithDetails, MistakesReviewListDto>();
    }
}