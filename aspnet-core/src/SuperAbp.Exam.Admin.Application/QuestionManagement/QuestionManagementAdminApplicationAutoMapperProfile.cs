using SuperAbp.Exam.Admin.QuestionManagement.QuestionAnswers;
using SuperAbp.Exam.Admin.QuestionManagement.Questions;
using AutoMapper;
using SuperAbp.Exam.Admin.QuestionManagement.QuestionBanks;
using SuperAbp.Exam.QuestionManagement.Questions;
using SuperAbp.Exam.QuestionManagement.QuestionAnswers;
using SuperAbp.Exam.QuestionManagement.QuestionBanks;
using SuperAbp.Exam.Admin.KnowledgePoints;
using SuperAbp.Exam.KnowledgePoints;

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
        CreateMap<QuestionBankWithDetails, QuestionListDto>()
            .ForMember(s => s.QuestionType,
                opt => opt.MapFrom(t => t.QuestionType.Value));
        CreateMap<Question, QuestionListDto>();
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

        CreateMap<QuestionBank, GetQuestionBankForEditorOutput>();
        CreateMap<QuestionBank, QuestionBankListDto>();
        CreateMap<QuestionBank, QuestionBankDetailDto>();
        CreateMap<QuestionBankCreateDto, QuestionBank>();
        CreateMap<QuestionBankUpdateDto, QuestionBank>();

        #endregion 题库

        #region 题目分类

        CreateMap<KnowledgePoint, KnowledgePointListDto>();
        CreateMap<KnowledgePoint, GetKnowledgePointForEditorOutput>();

        #endregion 题目分类
    }
}