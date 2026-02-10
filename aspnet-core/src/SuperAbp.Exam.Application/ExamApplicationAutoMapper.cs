using System.Text.RegularExpressions;
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
        CreateMap<Announcement, AnnouncementListDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
            .ForMember(dest => dest.BriefContent, opt => opt.MapFrom(src => GetBriefContent(src.Content)));
        CreateMap<Announcement, AnnouncementDetailDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null));
        CreateMap<AnnouncementCategory, AnnouncementCategoryDto>();

        CreateMap<Training, TrainingListDto>();

        CreateMap<FavoriteWithDetails, FavoriteListDto>();

        CreateMap<MistakeWithDetails, MistakeListDto>();
    }

    private static string GetBriefContent(string htmlContent)
    {
        if (string.IsNullOrWhiteSpace(htmlContent))
        {
            return string.Empty;
        }

        var plainText = Regex.Replace(htmlContent, "<[^>]+>", string.Empty);

        plainText = System.Net.WebUtility.HtmlDecode(plainText);

        plainText = Regex.Replace(plainText, "\\s+", " ").Trim();

        const int maxLength = 120;
        if (plainText.Length <= maxLength)
        {
            return plainText;
        }

        return plainText.Substring(0, maxLength) + "...";
    }
}