using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Users;

namespace SuperAbp.Exam.Favorites;

public class FavoriteAppService(IFavoriteRepository favoriteRepository) : ExamAppService, IFavoriteAppService
{
    public async Task<ListResultDto<FavoriteListDto>> GetListAsync()
    {
        List<Favorite> favorites = await favoriteRepository.GetListAsync(creatorId: CurrentUser.GetId());
        List<FavoriteListDto> dtos = ObjectMapper.Map<List<Favorite>, List<FavoriteListDto>>(favorites);
        return new ListResultDto<FavoriteListDto>(dtos);
    }

    public async Task<int> GetCountAsync()
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