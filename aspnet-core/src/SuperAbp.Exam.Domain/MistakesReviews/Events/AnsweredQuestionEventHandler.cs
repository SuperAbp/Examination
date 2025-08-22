using SuperAbp.Exam.MistakesReviews;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;

namespace SuperAbp.Exam.MistakesReviews.Events;

public class AnsweredQuestionEventHandler(IMistakesReviewRepository mistakesReviewRepository)
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
            await mistakesReviewRepository.InsertAsync(new MistakesReview(
                Guid.NewGuid(),
                eventData.QuestionId,
                eventData.UserId));
        }
    }
}