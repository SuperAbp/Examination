using SuperAbp.Exam.ExamManagement.UserExams;
using SuperAbp.Exam.ExamManagement.Exams;
using SuperAbp.Exam.QuestionManagement.Questions;
using AutoMapper;
using Volo.Abp.AutoMapper;
using SuperAbp.Exam.QuestionManagement.Questions.QuestionOptions;

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

            CreateMap<UserExamWithRanking, ExamRankingDto>();
            CreateMap<UserExam, UserExamListDto>();
            CreateMap<UserExamWithDetails, UserExamListDto>()
                .ForMember(dest => dest.TotalScore,
                opt => opt.Condition(src => src.Status == 3));

            CreateMap<UserExam, UserExamDetailDto>()
                .Ignore(s => s.EndTime)
                .Ignore(s => s.Sections);
            CreateMap<UserExamCreateDto, UserExam>();

            CreateMap<UserExamSection, UserExamDetailDto.SectionDto>()
                .Ignore(s => s.Questions);
            CreateMap<Question, UserExamDetailDto.SectionDto.QuestionDto>()
                .Ignore(s => s.Right)
                .Ignore(s => s.Options);
            CreateMap<QuestionOption, UserExamDetailDto.SectionDto.QuestionDto.OptionDto>()
                .Ignore(s => s.Right);

            #endregion 用户考试
        }
    }
}