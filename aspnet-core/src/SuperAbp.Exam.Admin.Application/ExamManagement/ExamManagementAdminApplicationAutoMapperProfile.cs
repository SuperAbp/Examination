using AutoMapper;
using SuperAbp.Exam.Admin.ExamManagement.Exams;
using SuperAbp.Exam.ExamManagement.Exams;

namespace SuperAbp.Exam.Admin.ExamManagement
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

            CreateMap<Examination, GetExamForEditorOutput>();
            CreateMap<Examination, ExamListDto>();
            CreateMap<Examination, ExamDetailDto>();
            CreateMap<ExamCreateDto, Examination>();
            CreateMap<ExamUpdateDto, Examination>();

            #endregion 考试
        }
    }
}