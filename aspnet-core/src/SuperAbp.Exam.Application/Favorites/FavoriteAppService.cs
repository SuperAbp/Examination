using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Users;

namespace SuperAbp.Exam.Favorites;

public class FavoriteAppService(IFavoriteRepository favoriteRepository) : ExamAppService, IFavoriteAppService
{
    public async Task<PagedResultDto<FavoriteListDto>> GetListAsync(GetFavoritesInput input)
    {
        List<FavoriteWithDetails> favorites = await favoriteRepository.GetListAsync(input.Sorting ?? FavoriteConsts.DefaultSorting, input.SkipCount,
            input.MaxResultCount, creatorId: CurrentUser.GetId(), input.QuestionName);
        long totalCount = await favoriteRepository.CountAsync(CurrentUser.GetId(), input.QuestionName);
        List<FavoriteListDto> dtos = ObjectMapper.Map<List<FavoriteWithDetails>, List<FavoriteListDto>>(favorites);
        return new PagedResultDto<FavoriteListDto>(totalCount, dtos);
    }

    public async Task<long> GetCountAsync()
    {
        return await favoriteRepository.CountAsync(CurrentUser.GetId());
    }

    public async Task<bool> GetByQuestionIdAsync(Guid questionId)
    {
        return await favoriteRepository.ExistsAsync(CurrentUser.GetId(), questionId);
    }

    public async Task CreateAsync(Guid questionId)
    {
        if (await favoriteRepository.ExistsAsync(CurrentUser.GetId(), questionId))
        {
            return;
        }

        Favorite favorite = new Favorite(GuidGenerator.Create(), questionId, CurrentUser.GetId(), Clock.Now);
        await favoriteRepository.InsertAsync(favorite);
    }

    public async Task DeleteAsync(Guid questionId)
    {
        await favoriteRepository.DeleteAsync(questionId);
    }
}