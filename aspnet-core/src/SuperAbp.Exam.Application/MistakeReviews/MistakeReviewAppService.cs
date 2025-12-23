using System.Collections.Generic;
using System.Threading.Tasks;
using SuperAbp.Exam.QuestionManagement.Questions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Users;

namespace SuperAbp.Exam.MistakeReviews;

public class MistakeReviewAppService(IMistakeReviewRepository mistakeReviewRepository)
    : ExamAppService, IMistakeReviewAppService
{
    public async Task<PagedResultDto<MistakeReviewListDto>> GetListAsync(GetMistakeReviewsInput input)
    {
        QuestionType? questionType = input.QuestionType.HasValue ? QuestionType.FromValue(input.QuestionType.Value) : null;
        List<MistakeWithDetails> mistakes = await mistakeReviewRepository.GetListAsync(
            sorting: input.Sorting,
            skipCount: input.SkipCount,
            maxResultCount: input.MaxResultCount,
            userId: CurrentUser.GetId(),
            questionContent: input.QuestionContent,
            questionType: questionType
        );
        long totalCount = await mistakeReviewRepository.CountAsync(
            userId: CurrentUser.GetId(),
            questionContent: input.QuestionContent,
            questionType: questionType
        );
        List<MistakeReviewListDto> dtos = ObjectMapper.Map<List<MistakeWithDetails>, List<MistakeReviewListDto>>(mistakes);
        return new PagedResultDto<MistakeReviewListDto>(totalCount, dtos);
    }
}