using AutoMapper;
using SuperAbp.Exam.Admin.Announcements;
using SuperAbp.Exam.Announcements;

namespace SuperAbp.Exam.Admin;

public class ExamApplicationAdminAutoMapperProfile : Profile
{
    public ExamApplicationAdminAutoMapperProfile()
    {
        /* You can configure your Volo.Abp.AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        CreateMap<Announcement, AnnouncementListDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null));
        CreateMap<Announcement, AnnouncementDetailDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null));
        CreateMap<AnnouncementCategory, AnnouncementCategoryListDto>();
        CreateMap<AnnouncementCategory, AnnouncementCategoryDetailDto>();
    }
}