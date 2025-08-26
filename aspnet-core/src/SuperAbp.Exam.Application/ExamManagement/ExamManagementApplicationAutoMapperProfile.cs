using SuperAbp.Exam.ExamManagement.UserExamQuestions;
using SuperAbp.Exam.ExamManagement.UserExams;
using AutoMapper;
using SuperAbp.Exam.ExamManagement.Exams;
using SuperAbp.Exam.QuestionManagement.Questions;
using Volo.Abp.AutoMapper;

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
            CreateMap<UserExamWithDetails, UserExamListDto>()
                .ForMember(dest => dest.TotalScore,
                opt => opt.Condition(src => src.Status == 3));

            CreateMap<UserExam, UserExamDetailDto>()
                .Ignore(s => s.EndTime)
                .Ignore(s => s.Questions);
            CreateMap<UserExamCreateDto, UserExam>();

            CreateMap<Question, UserExamDetailDto.QuestionDto>()
                .Ignore(s => s.Right)
                .Ignore(s => s.Options);

            #endregion 用户考试
        }
    }
}