using System.Collections.Generic;
using System.Threading.Tasks;
using SuperAbp.Exam.QuestionManagement.Questions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Users;

namespace SuperAbp.Exam.MistakesReviews;

public class MistakesReviewAppService(IMistakesReviewRepository mistakesReviewRepository)
    : ExamAppService, IMistakesReviewAppService
{
    public async Task<PagedResultDto<MistakesReviewListDto>> GetListAsync(GetMistakesReviewInput input)
    {
        QuestionType? questionType = input.QuestionType.HasValue ? QuestionType.FromValue(input.QuestionType.Value) : null;
        List<MistakeWithDetails> mistakes = await mistakesReviewRepository.GetListAsync(
            sorting: input.Sorting,
            skipCount: input.SkipCount,
            maxResultCount: input.MaxResultCount,
            userId: CurrentUser.GetId(),
            questionContent: input.QuestionContent,
            questionType: questionType
        );
        long totalCount = await mistakesReviewRepository.CountAsync(
            userId: CurrentUser.GetId(),
            questionContent: input.QuestionContent,
            questionType: questionType
        );
        List<MistakesReviewListDto> dtos = ObjectMapper.Map<List<MistakeWithDetails>, List<MistakesReviewListDto>>(mistakes);
        return new PagedResultDto<MistakesReviewListDto>(totalCount, dtos);
    }
}