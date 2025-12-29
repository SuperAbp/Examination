using AutoMapper;
using SuperAbp.Exam.Admin.ExamManagement.Exams;
using SuperAbp.Exam.Admin.ExamManagement.UserExams;
using SuperAbp.Exam.ExamManagement.Exams;
using SuperAbp.Exam.ExamManagement.UserExams;
using SuperAbp.Exam.QuestionManagement.Questions;

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

            CreateMap<UserExamWithUser, ExamUserExamDto>()
               .ForMember(s => s.User,
                    opt => opt.MapFrom(p => p.UserName));
            CreateMap<UserExam, UserExamListDto>();
            CreateMap<UserExam, UserExamDetailDto>()
                .ForMember(s => s.Sections,
                    opt => opt.Ignore())
                .ForMember(s => s.UserName,
                    opt => opt.Ignore())
                .ForMember(s => s.ExamName,
                    opt => opt.Ignore());
            CreateMap<UserExamSection, UserExamDetailDto.SectionDto>()
                .ForMember(s => s.Questions,
                    opt => opt.Ignore());
            CreateMap<Question, UserExamDetailDto.SectionDto.QuestionDto>()
                .ForMember(s => s.Options,
                    opt => opt.Ignore());
        }
    }
}