using SuperAbp.Exam.ExamManagement.UserExamQuestions;
using SuperAbp.Exam.ExamManagement.UserExams;
using AutoMapper;
using SuperAbp.Exam.ExamManagement.Exams;
using SuperAbp.Exam.QuestionManagement.Questions;

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

            CreateMap<Examination, ExamListDto>();
            CreateMap<Examination, ExamDetailDto>();

            #endregion 考试

            #region 用户考试

            CreateMap<UserExam, UserExamListDto>();
            CreateMap<UserExamWithDetails, UserExamListDto>();
            CreateMap<UserExam, UserExamDetailDto>()
                .ForMember(s => s.Questions,
                opt => opt.Ignore());
            CreateMap<UserExamCreateDto, UserExam>();

            CreateMap<Question, UserExamDetailDto.QuestionDto>()
                .ForMember(s => s.Right,
                    opt => opt.Ignore())
                .ForMember(s => s.Options,
                    opt => opt.Ignore());

            #endregion 用户考试
        }
    }
}