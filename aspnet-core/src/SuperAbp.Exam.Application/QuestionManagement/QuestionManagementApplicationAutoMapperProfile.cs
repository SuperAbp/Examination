using AutoMapper;
using SuperAbp.Exam.QuestionManagement.QuestionAnswers;
using SuperAbp.Exam.QuestionManagement.QuestionBanks;
using SuperAbp.Exam.QuestionManagement.Questions;

namespace SuperAbp.Exam.QuestionManagement;

public class QuestionManagementApplicationAutoMapperProfile : Profile
{
    public QuestionManagementApplicationAutoMapperProfile()
    {
        CreateMap<QuestionBank, QuestionBankListDto>();
        CreateMap<QuestionBank, QuestionBankDetailDto>();

        CreateMap<Question, QuestionListDto>();
        CreateMap<Question, QuestionDetailDto>();

        CreateMap<QuestionAnswer, QuestionAnswerListDto>();
    }
}