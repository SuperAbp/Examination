using AutoMapper;
using SuperAbp.Exam.Admin.ExamManagement.Exams;
using SuperAbp.Exam.ExamManagement.Exams;

namespace SuperAbp.Exam.Admin.ExamManagement
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
            #endregion
        }
    }
}