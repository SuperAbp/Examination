using SuperAbp.Exam.Admin.QuestionManagement.Questions;
using AutoMapper;
using SuperAbp.Exam.Admin.QuestionManagement.QuestionBanks;
using SuperAbp.Exam.QuestionManagement.Questions;
using SuperAbp.Exam.QuestionManagement.QuestionBanks;
using SuperAbp.Exam.Admin.KnowledgePoints;
using SuperAbp.Exam.KnowledgePoints;
using SuperAbp.Exam.QuestionManagement.Questions.QuestionAnswers;

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
        CreateMap<QuestionWithDetails, QuestionListDto>()
            .ForMember(s => s.QuestionType,
                opt => opt.MapFrom(t => t.QuestionType.Value));
        CreateMap<Question, QuestionListDto>();
        CreateMap<QuestionCreateDto, Question>();
        CreateMap<QuestionUpdateDto, Question>();

        CreateMap<QuestionAnswer, QuestionAnswerDto>();

        #endregion 问题

        #region 题库

        CreateMap<QuestionBank, GetQuestionBankForEditorOutput>();
        CreateMap<QuestionBank, QuestionBankListDto>();
        CreateMap<QuestionBank, QuestionBankDetailDto>();
        CreateMap<QuestionBankCreateDto, QuestionBank>();
        CreateMap<QuestionBankUpdateDto, QuestionBank>();

        #endregion 题库

        #region 题目分类

        CreateMap<KnowledgePoint, KnowledgePointNodeDto>();
        CreateMap<KnowledgePoint, GetKnowledgePointForEditorOutput>();

        #endregion 题目分类
    }
}