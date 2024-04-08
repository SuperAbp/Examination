using SuperAbp.Exam.ExamManagement.UserExamQuestions;
using SuperAbp.Exam.ExamManagement.UserExams;
using AutoMapper;
using SuperAbp.Exam.ExamManagement.Exams;

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
            CreateMap<UserExamWithExam, UserExamListDto>();
            CreateMap<UserExam, UserExamDetailDto>();
            CreateMap<UserExamCreateDto, UserExam>();

            #endregion 用户考试

            #region 用户考题

            CreateMap<UserExamQuestion, GetUserExamQuestionForEditorOutput>();
            CreateMap<UserExamQuestion, UserExamQuestionListDto>();
            CreateMap<UserExamQuestion, UserExamQuestionDetailDto>();
            CreateMap<UserExamQuestionCreateDto, UserExamQuestion>();
            CreateMap<UserExamQuestionAnswerDto, UserExamQuestion>();

            #endregion 用户考题
        }
    }
}