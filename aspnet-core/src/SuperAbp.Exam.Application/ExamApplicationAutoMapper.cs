using AutoMapper;
using SuperAbp.Exam.Favorites;
using SuperAbp.Exam.MistakeReviews;
using SuperAbp.Exam.TrainingManagement;

namespace SuperAbp.Exam;

public class ExamApplicationAutoMapper : Profile
{
    public ExamApplicationAutoMapper()
    {
        CreateMap<Training, TrainingListDto>();

        CreateMap<FavoriteWithDetails, FavoriteListDto>();

        CreateMap<MistakeWithDetails, MistakeReviewListDto>();
    }
}