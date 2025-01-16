using AutoMapper;
using SuperAbp.Exam.Admin.PaperManagement.PaperRepos;
using SuperAbp.Exam.Admin.PaperManagement.Papers;
using SuperAbp.Exam.PaperManagement.PaperRepos;
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

            CreateMap<PaperRepo, GetPaperRepoForEditorOutput>();
            CreateMap<PaperRepo, PaperRepoListDto>();
            CreateMap<PaperRepositoryDetail, PaperRepoListDto>();
            CreateMap<PaperRepo, PaperRepoDetailDto>();
            CreateMap<PaperRepoCreateOrUpdateDtoBase, PaperRepo>();
            CreateMap<PaperRepoCreateDto, PaperRepo>();
            CreateMap<PaperRepoUpdateDto, PaperRepo>();

            #endregion 考试题库
        }
    }
}