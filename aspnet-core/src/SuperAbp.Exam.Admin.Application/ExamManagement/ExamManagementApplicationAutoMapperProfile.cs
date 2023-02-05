using SuperAbp.Exam.Admin.ExamManagement.ExamRepos;
using SuperAbp.Exam.Admin.ExamManagement.Exams;
using AutoMapper;
using SuperAbp.Exam.ExamManagement.Exams;
using SuperAbp.Exam.ExamManagement.ExamRepos;

namespace SuperAbp.Exam.ExamManagement
{
    /// <summary>
    /// Mapper映射配置
    /// </summary>
    public class ExamManagementApplicationAutoMapperProfile : Profile
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public ExamManagementApplicationAutoMapperProfile()
        {
            #region 考试

            CreateMap<Examing, GetExamingForEditorOutput>();
            CreateMap<Examing, ExamingListDto>();
            CreateMap<Examing, ExamingDetailDto>();
            CreateMap<ExamingCreateDto, Examing>();
            CreateMap<ExamingUpdateDto, Examing>();

            #endregion 考试

            #region 考试题库

            CreateMap<ExamingRepo, GetExamingRepoForEditorOutput>();
            CreateMap<ExamingRepo, ExamingRepoListDto>();
            CreateMap<ExamingRepositoryDetail, ExamingRepoListDto>();
            CreateMap<ExamingRepo, ExamingRepoDetailDto>();
            CreateMap<ExamingRepoCreateOrUpdateDtoBase, ExamingRepo>();
            CreateMap<ExamingRepoCreateDto, ExamingRepo>();
            CreateMap<ExamingRepoUpdateDto, ExamingRepo>();

            #endregion 考试题库
        }
    }
}