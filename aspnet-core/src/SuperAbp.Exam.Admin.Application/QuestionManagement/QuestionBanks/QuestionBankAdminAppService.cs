using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SuperAbp.Exam.Permissions;
using SuperAbp.Exam.QuestionManagement.QuestionBanks;
using SuperAbp.Exam.QuestionManagement.Questions;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.QuestionManagement.QuestionBanks
{
    [Authorize(ExamPermissions.QuestionBanks.Default)]
    public class QuestionBankAdminAppService(
        QuestionBankManager questionRepoManager,
        IQuestionBankRepository questionBankRepository,
        IQuestionRepository questionRepository)
        : ExamAppService, IQuestionBankAdminAppService
    {
        public virtual async Task<QuestionBankDetailDto> GetAsync(Guid id)
        {
            QuestionBank entity = await questionBankRepository.GetAsync(id);

            return ObjectMapper.Map<QuestionBank, QuestionBankDetailDto>(entity);
        }

        public virtual async Task<QuestionBankCountDto> GetQuestionCountAsync(Guid id)
        {
            var dto = new QuestionBankCountDto
            {
                SingleCount = await questionRepository.GetCountAsync(id, QuestionType.SingleSelect),
                JudgeCount = await questionRepository.GetCountAsync(id, QuestionType.Judge),
                MultiCount = await questionRepository.GetCountAsync(id, QuestionType.MultiSelect),
                BlankCount = await questionRepository.GetCountAsync(id, QuestionType.FillInTheBlanks)
            };
            return dto;
        }

        public virtual async Task<PagedResultDto<QuestionBankListDto>> GetListAsync(GetQuestionBanksInput input)
        {
            long totalCount = await questionBankRepository.GetCountAsync(input.Title);

            var entities = await questionBankRepository
                .GetListAsync(input.Sorting ?? QuestionBankConsts.DefaultSorting, input.SkipCount,
                    input.MaxResultCount, input.Title);

            var dtos = new List<QuestionBankListDto>();
            foreach (var item in entities)
            {
                var dto = ObjectMapper.Map<QuestionBank, QuestionBankListDto>(item);
                dto.SingleCount = await questionRepository.GetCountAsync(item.Id, QuestionType.SingleSelect);
                dto.JudgeCount = await questionRepository.GetCountAsync(item.Id, QuestionType.Judge);
                dto.MultiCount = await questionRepository.GetCountAsync(item.Id, QuestionType.MultiSelect);
                dto.BlankCount = await questionRepository.GetCountAsync(item.Id, QuestionType.FillInTheBlanks);
                dtos.Add(dto);
            }
            return new PagedResultDto<QuestionBankListDto>(totalCount, dtos);
        }

        public virtual async Task<GetQuestionBankForEditorOutput> GetEditorAsync(Guid id)
        {
            QuestionBank entity = await questionBankRepository.GetAsync(id);

            return ObjectMapper.Map<QuestionBank, GetQuestionBankForEditorOutput>(entity);
        }

        [Authorize(ExamPermissions.QuestionBanks.Create)]
        public virtual async Task<QuestionBankListDto> CreateAsync(QuestionBankCreateDto input)
        {
            QuestionBank repository = await questionRepoManager.CreateAsync(input.Title);
            repository.Remark = input.Remark;

            repository = await questionBankRepository.InsertAsync(repository);
            return ObjectMapper.Map<QuestionBank, QuestionBankListDto>(repository);
        }

        [Authorize(ExamPermissions.QuestionBanks.Update)]
        public virtual async Task<QuestionBankListDto> UpdateAsync(Guid id, QuestionBankUpdateDto input)
        {
            QuestionBank repository = await questionBankRepository.GetAsync(id);
            await questionRepoManager.SetTitleAsync(repository, input.Title);
            repository.Remark = input.Remark;
            repository = await questionBankRepository.UpdateAsync(repository);
            return ObjectMapper.Map<QuestionBank, QuestionBankListDto>(repository);
        }

        [Authorize(ExamPermissions.QuestionBanks.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await questionBankRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 规范最大记录数
        /// </summary>
        /// <param name="input">参数</param>
        /// <returns></returns>
        private async Task NormalizeMaxResultCountAsync(PagedAndSortedResultRequestDto input)
        {
            var maxPageSize = (await SettingProvider.GetOrNullAsync(QuestionBankSettings.MaxPageSize))?.To<int>();
            if (maxPageSize.HasValue && input.MaxResultCount > maxPageSize.Value)
            {
                input.MaxResultCount = maxPageSize.Value;
            }
        }
    }
}