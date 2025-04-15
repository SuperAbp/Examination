using SuperAbp.Exam.Admin.QuestionManagement.QuestionRepos;
using SuperAbp.Exam.Admin.QuestionManagement.QuestionAnswers;
using SuperAbp.Exam.Admin.QuestionManagement.Questions;
using AutoMapper;
using SuperAbp.Exam.QuestionManagement.Questions;
using SuperAbp.Exam.QuestionManagement.QuestionAnswers;
using SuperAbp.Exam.QuestionManagement.QuestionRepos;

namespace SuperAbp.Exam.Admin.QuestionManagement;

/// <summary>
/// Mapper映射配置
/// </summary>
public class QuestionManagementAdminApplicationAutoMapperProfile : Profile
{
    /// <summary>
    /// .ctor
    /// </summary>
    public QuestionManagementAdminApplicationAutoMapperProfile()
    {
        #region 问题

        CreateMap<Question, GetQuestionForEditorOutput>();
        CreateMap<QuestionRepositoryWithDetails, QuestionListDto>()
            .ForMember(s => s.QuestionType,
                opt => opt.MapFrom(t => t.QuestionType.Value));
        CreateMap<Question, QuestionListDto>();
        CreateMap<Question, QuestionDetailDto>();
        CreateMap<QuestionCreateDto, Question>();
        CreateMap<QuestionUpdateDto, Question>();

        #endregion 问题

        #region 答案

        CreateMap<QuestionAnswer, GetQuestionAnswerForEditorOutput>();
        CreateMap<QuestionAnswer, QuestionAnswerListDto>();
        CreateMap<QuestionAnswer, QuestionAnswerDetailDto>();
        CreateMap<QuestionAnswerCreateDto, QuestionAnswer>();
        CreateMap<QuestionAnswerUpdateDto, QuestionAnswer>();

        #endregion 答案

        #region 题库

        CreateMap<QuestionRepo, GetQuestionRepoForEditorOutput>();
        CreateMap<QuestionRepo, QuestionRepoListDto>();
        CreateMap<QuestionRepo, QuestionRepoDetailDto>();
        CreateMap<QuestionRepoCreateDto, QuestionRepo>();
        CreateMap<QuestionRepoUpdateDto, QuestionRepo>();

        #endregion 题库
    }
}