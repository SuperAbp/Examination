using AutoMapper;
using SuperAbp.Exam.QuestionManagement.QuestionBanks;
using SuperAbp.Exam.QuestionManagement.Questions;
using SuperAbp.Exam.QuestionManagement.Questions.QuestionOptions;

namespace SuperAbp.Exam.QuestionManagement;

public class QuestionManagementApplicationAutoMapperProfile : Profile
{
    public QuestionManagementApplicationAutoMapperProfile()
    {
        CreateMap<QuestionBank, QuestionBankListDto>();
        CreateMap<QuestionBank, QuestionBankDetailDto>();

        CreateMap<Question, QuestionListDto>();
        CreateMap<Question, QuestionDetailDto>();

        CreateMap<QuestionOption, QuestionOptionDto>();
    }
}