using SuperAbp.Exam.Mistakes;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;

namespace SuperAbp.Exam.MistakesReviews.Events;

public class AnsweredQuestionEventHandler(IMistakeRepository mistakesReviewRepository)
    : ILocalEventHandler<AnsweredQuestionEvent>, ITransientDependency
{
    public async Task HandleEventAsync(AnsweredQuestionEvent eventData)
    {
        if (eventData.Right)
        {
            return;
        }

        var existingReview = await mistakesReviewRepository.FindAsync(mr =>
                mr.QuestionId == eventData.QuestionId &&
                mr.UserId == eventData.UserId);

        if (existingReview != null)
        {
            existingReview.ErrorCount++;
            await mistakesReviewRepository.UpdateAsync(existingReview);
        }
        else
        {
            await mistakesReviewRepository.InsertAsync(new Mistake(
                Guid.NewGuid(),
                eventData.QuestionId,
                eventData.UserId));
        }
    }
}