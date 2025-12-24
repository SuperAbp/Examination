using System.Collections.Generic;
using System.Threading.Tasks;
using SuperAbp.Exam.QuestionManagement.Questions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Users;

namespace SuperAbp.Exam.Mistakes;

public class MistakeAppService(IMistakeRepository mistakeRepository)
    : ExamAppService, IMistakeAppService
{
    public async Task<PagedResultDto<MistakeListDto>> GetListAsync(GetMistakesInput input)
    {
        QuestionType? questionType = input.QuestionType.HasValue ? QuestionType.FromValue(input.QuestionType.Value) : null;
        List<MistakeWithDetails> mistakes = await mistakeRepository.GetListAsync(
            sorting: input.Sorting,
            skipCount: input.SkipCount,
            maxResultCount: input.MaxResultCount,
            userId: CurrentUser.GetId(),
            questionContent: input.QuestionContent,
            questionType: questionType
        );
        long totalCount = await mistakeRepository.CountAsync(
            userId: CurrentUser.GetId(),
            questionContent: input.QuestionContent,
            questionType: questionType
        );
        List<MistakeListDto> dtos = ObjectMapper.Map<List<MistakeWithDetails>, List<MistakeListDto>>(mistakes);
        return new PagedResultDto<MistakeListDto>(totalCount, dtos);
    }
}