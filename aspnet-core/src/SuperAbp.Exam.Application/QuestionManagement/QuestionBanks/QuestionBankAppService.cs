using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SuperAbp.Exam.QuestionManagement.Questions;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.QuestionManagement.QuestionBanks
{
    public class QuestionBankAppService(
        IQuestionBankRepository questionBankRepository,
        IQuestionRepository questionRepository)
        : ExamAppService, IQuestionBankAppService
    {
        public virtual async Task<ListResultDto<QuestionType>> GetQuestionTypesAsync(Guid id)
        {
            List<QuestionType> questionTypes = await questionRepository.GetQuestionTypesAsync(id);
            return new ListResultDto<QuestionType>(questionTypes);
        }

        public virtual async Task<PagedResultDto<QuestionBankListDto>> GetListAsync(GetQuestionBanksInput input)
        {
            long totalCount = await questionBankRepository.GetCountAsync(input.Title);

            List<QuestionBank> entities = await questionBankRepository
                .GetListAsync(input.Sorting ?? QuestionBankConsts.DefaultSorting, input.SkipCount,
                    input.MaxResultCount, input.Title);

            List<QuestionBankListDto> dtos = ObjectMapper.Map<List<QuestionBank>, List<QuestionBankListDto>>(entities);
            return new PagedResultDto<QuestionBankListDto>(totalCount, dtos);
        }

        public async Task<QuestionBankDetailDto> GetAsync(Guid id)
        {
            QuestionBank questionRepo = await questionBankRepository.GetAsync(id);

            QuestionBankDetailDto dto = ObjectMapper.Map<QuestionBank, QuestionBankDetailDto>(questionRepo);
            return dto;
        }
    }
}