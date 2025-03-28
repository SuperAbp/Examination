using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using SuperAbp.Exam.Permissions;
using SuperAbp.Exam.QuestionManagement.QuestionRepos;
using SuperAbp.Exam.QuestionManagement.Questions;

namespace SuperAbp.Exam.Admin.QuestionManagement.QuestionRepos
{
    [Authorize(ExamPermissions.QuestionRepositories.Default)]
    public class QuestionRepoAdminAppService(
        QuestionRepositoryManager questionRepoManager,
        IQuestionRepoRepository questionRepoRepository,
        IQuestionRepository questionRepository)
        : ExamAppService, IQuestionRepoAdminAppService
    {
        public virtual async Task<QuestionRepoDetailDto> GetAsync(Guid id)
        {
            QuestionRepo entity = await questionRepoRepository.GetAsync(id);

            return ObjectMapper.Map<QuestionRepo, QuestionRepoDetailDto>(entity);
        }

        public virtual async Task<QuestionRepoCountDto> GetQuestionCountAsync(Guid id)
        {
            var dto = new QuestionRepoCountDto
            {
                SingleCount = await questionRepository.GetCountAsync(id, QuestionType.SingleSelect),
                JudgeCount = await questionRepository.GetCountAsync(id, QuestionType.Judge),
                MultiCount = await questionRepository.GetCountAsync(id, QuestionType.MultiSelect),
                BlankCount = await questionRepository.GetCountAsync(id, QuestionType.FillInTheBlanks)
            };
            return dto;
        }

        public virtual async Task<PagedResultDto<QuestionRepoListDto>> GetListAsync(GetQuestionReposInput input)
        {
            long totalCount = await questionRepoRepository.GetCountAsync(input.Title);

            var entities = await questionRepoRepository
                .GetListAsync(input.Sorting ?? QuestionRepoConsts.DefaultSorting, input.SkipCount,
                    input.MaxResultCount, input.Title);

            var dtos = new List<QuestionRepoListDto>();
            foreach (var item in entities)
            {
                var dto = ObjectMapper.Map<QuestionRepo, QuestionRepoListDto>(item);
                dto.SingleCount = await questionRepository.GetCountAsync(item.Id, QuestionType.SingleSelect);
                dto.JudgeCount = await questionRepository.GetCountAsync(item.Id, QuestionType.Judge);
                dto.MultiCount = await questionRepository.GetCountAsync(item.Id, QuestionType.MultiSelect);
                dto.BlankCount = await questionRepository.GetCountAsync(item.Id, QuestionType.FillInTheBlanks);
                dtos.Add(dto);
            }
            return new PagedResultDto<QuestionRepoListDto>(totalCount, dtos);
        }

        public virtual async Task<GetQuestionRepoForEditorOutput> GetEditorAsync(Guid id)
        {
            QuestionRepo entity = await questionRepoRepository.GetAsync(id);

            return ObjectMapper.Map<QuestionRepo, GetQuestionRepoForEditorOutput>(entity);
        }

        [Authorize(ExamPermissions.QuestionRepositories.Create)]
        public virtual async Task<QuestionRepoListDto> CreateAsync(QuestionRepoCreateDto input)
        {
            QuestionRepo repository = await questionRepoManager.CreateAsync(input.Title);
            repository.Remark = input.Remark;

            repository = await questionRepoRepository.InsertAsync(repository);
            return ObjectMapper.Map<QuestionRepo, QuestionRepoListDto>(repository);
        }

        [Authorize(ExamPermissions.QuestionRepositories.Update)]
        public virtual async Task<QuestionRepoListDto> UpdateAsync(Guid id, QuestionRepoUpdateDto input)
        {
            QuestionRepo repository = await questionRepoRepository.GetAsync(id);
            await questionRepoManager.SetTitleAsync(repository, input.Title);
            repository.Remark = input.Remark;
            repository = await questionRepoRepository.UpdateAsync(repository);
            return ObjectMapper.Map<QuestionRepo, QuestionRepoListDto>(repository);
        }

        [Authorize(ExamPermissions.QuestionRepositories.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await questionRepoRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 规范最大记录数
        /// </summary>
        /// <param name="input">参数</param>
        /// <returns></returns>
        private async Task NormalizeMaxResultCountAsync(PagedAndSortedResultRequestDto input)
        {
            var maxPageSize = (await SettingProvider.GetOrNullAsync(QuestionRepoSettings.MaxPageSize))?.To<int>();
            if (maxPageSize.HasValue && input.MaxResultCount > maxPageSize.Value)
            {
                input.MaxResultCount = maxPageSize.Value;
            }
        }
    }
}