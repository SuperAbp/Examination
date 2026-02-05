using AutoMapper;
using SuperAbp.Exam.Announcements;
using SuperAbp.Exam.Favorites;
using SuperAbp.Exam.Mistakes;
using SuperAbp.Exam.TrainingManagement;

namespace SuperAbp.Exam;

public class ExamApplicationAutoMapper : Profile
{
    public ExamApplicationAutoMapper()
    {
        CreateMap<Announcement, AnnouncementDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null));
        CreateMap<AnnouncementCategory, AnnouncementCategoryDto>();

        CreateMap<Training, TrainingListDto>();

        CreateMap<FavoriteWithDetails, FavoriteListDto>();

        CreateMap<MistakeWithDetails, MistakeListDto>();
    }
}