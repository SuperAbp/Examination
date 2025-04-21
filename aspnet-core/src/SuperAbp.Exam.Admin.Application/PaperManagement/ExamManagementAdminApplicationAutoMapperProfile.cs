using AutoMapper;
using SuperAbp.Exam.Admin.PaperManagement.PaperQuestionRules;
using SuperAbp.Exam.Admin.PaperManagement.Papers;
using SuperAbp.Exam.PaperManagement.PaperQuestionRules;
using SuperAbp.Exam.PaperManagement.Papers;

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

            CreateMap<Paper, GetPaperForEditorOutput>();
            CreateMap<Paper, PaperListDto>();
            CreateMap<PaperCreateDto, Paper>();
            CreateMap<PaperUpdateDto, Paper>();

            #endregion 考试

            #region 考试题库

            CreateMap<PaperQuestionRule, GetPaperQuestionRuleForEditorOutput>();
            CreateMap<PaperQuestionRule, PaperQuestionRuleListDto>();
            CreateMap<PaperQuestionRuleWithDetails, PaperQuestionRuleListDto>();
            CreateMap<PaperQuestionRule, PaperQuestionRuleDetailDto>();
            CreateMap<PaperQuestionRuleCreateOrUpdateDtoBase, PaperQuestionRule>();

            #endregion 考试题库
        }
    }
}