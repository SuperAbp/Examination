using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SuperAbp.Exam.MistakesReviews;

public interface IMistakesReviewAppService : IApplicationService
{
    Task<PagedResultDto<MistakesReviewListDto>> GetListAsync(GetMistakesReviewInput input);
}