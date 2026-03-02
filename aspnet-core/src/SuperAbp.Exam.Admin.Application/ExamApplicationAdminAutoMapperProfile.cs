using AutoMapper;
using SuperAbp.Exam.Admin.Announcements;
using SuperAbp.Exam.Admin.Notifications;
using SuperAbp.Exam.Announcements;
using SuperAbp.Exam.Notifications;

namespace SuperAbp.Exam.Admin;

public class ExamApplicationAdminAutoMapperProfile : Profile
{
    public ExamApplicationAdminAutoMapperProfile()
    {
        /* You can configure your Volo.Abp.AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        CreateMap<Announcement, AnnouncementListDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
            .ForMember(dest => dest.Sort, opt => opt.MapFrom(src => src.Sort));
        CreateMap<Announcement, AnnouncementDetailDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null));
        CreateMap<AnnouncementCategory, AnnouncementCategoryListDto>();
        CreateMap<AnnouncementCategory, AnnouncementCategoryDetailDto>();

        CreateMap<Notification, NotificationListDto>();
        CreateMap<Notification, NotificationMyListDto>();
    }
}