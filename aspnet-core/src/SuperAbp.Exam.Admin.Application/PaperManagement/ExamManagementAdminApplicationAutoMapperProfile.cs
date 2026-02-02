using AutoMapper;
using SuperAbp.Exam.Admin.PaperManagement.PaperQuestionRules;
using SuperAbp.Exam.Admin.PaperManagement.Papers;
using SuperAbp.Exam.PaperManagement.PaperQuestionRules;
using SuperAbp.Exam.PaperManagement.Papers;
using System.Linq;
using static SuperAbp.Exam.Admin.PaperManagement.Papers.PaperCreateOrUpdateDtoBase;
using static SuperAbp.Exam.Admin.PaperManagement.Papers.PaperCreateOrUpdateDtoBase.PaperSectionDto;

namespace SuperAbp.Exam.Admin.PaperManagement
{
    /// <summary>
    /// Mapper映射配置
    /// </summary>
    public class ExamManagementAdminApplicationAutoMapperProfile : Profile
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public ExamManagementAdminApplicationAutoMapperProfile()
        {
            #region 考试

            CreateMap<Paper, GetPaperForEditorOutput>()
                .ForMember(s => s.Sections,
                opt => opt.MapFrom(t => t.PaperSections.OrderBy(s => s.Order)));
            CreateMap<Paper, PaperListDto>();
            CreateMap<PaperCreateDto, Paper>();
            CreateMap<PaperUpdateDto, Paper>();

            #endregion 考试

            #region 考试题库

            CreateMap<PaperSection, PaperSectionDto>()
                 .ForMember(s => s.PaperQuestions,
                opt => opt.MapFrom(t => t.PaperQuestions.OrderBy(s => s.Order)));
            CreateMap<PaperQuestion, PaperQuestionDto>();
            CreateMap<PaperQuestionRule, PaperQuestionRuleDto>();

            CreateMap<PaperQuestionRule, GetPaperQuestionRuleForEditorOutput>();
            CreateMap<PaperQuestionRule, PaperQuestionRuleListDto>();
            CreateMap<PaperQuestionRule, PaperQuestionRuleDetailDto>();
            CreateMap<PaperQuestionRuleCreateOrUpdateDtoBase, PaperQuestionRule>();

            #endregion 考试题库
        }
    }
}