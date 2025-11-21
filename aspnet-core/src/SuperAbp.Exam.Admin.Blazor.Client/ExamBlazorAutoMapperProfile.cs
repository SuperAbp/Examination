using AutoMapper;
using SuperAbp.Exam.Admin.QuestionManagement.QuestionBanks;

namespace SuperAbp.Exam.Admin.Blazor.Client;

public class ExamBlazorAutoMapperProfile : Profile
{
    public ExamBlazorAutoMapperProfile()
    {
        CreateMap<QuestionBankDetailDto, QuestionBankUpdateDto>();
    }
}