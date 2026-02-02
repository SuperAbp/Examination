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
        QuestionBankManager questionBankManager,
        IQuestionBankRepository questionBankRepository,
        IQuestionRepository questionRepository)
        : ExamAppService, IQuestionBankAdminAppService
    {
        protected QuestionBankManager QuestionRepoManager { get; } = questionRepoManager;
        protected QuestionBankManager QuestionBankManager { get; } = questionBankManager;
        protected IQuestionBankRepository QuestionBankRepository { get; } = questionBankRepository;
        protected IQuestionRepository QuestionRepository { get; } = questionRepository;

        public virtual async Task<QuestionBankDetailDto> GetAsync(Guid id)
        {
            QuestionBank entity = await QuestionBankRepository.GetAsync(id);

            return ObjectMapper.Map<QuestionBank, QuestionBankDetailDto>(entity);
        }

        public virtual async Task<PagedResultDto<QuestionBankListDto>> GetListAsync(GetQuestionBanksInput input)
        {
            long totalCount = await QuestionBankRepository.GetCountAsync(input.Title);

            var entities = await QuestionBankRepository
                .GetListAsync(input.Sorting ?? QuestionBankConsts.DefaultSorting, input.SkipCount,
                    input.MaxResultCount, input.Title);

            var dtos = new List<QuestionBankListDto>();
            foreach (var item in entities)
            {
                var dto = ObjectMapper.Map<QuestionBank, QuestionBankListDto>(item);
                dto.SingleCount = await QuestionRepository.GetCountAsync(questionBankIds: new List<Guid> { item.Id }, questionType: QuestionType.SingleSelect);
                dto.JudgeCount = await QuestionRepository.GetCountAsync(questionBankIds: new List<Guid> { item.Id }, questionType: QuestionType.Judge);
                dto.MultiCount = await QuestionRepository.GetCountAsync(questionBankIds: new List<Guid> { item.Id }, questionType: QuestionType.MultiSelect);
                dto.BlankCount = await QuestionRepository.GetCountAsync(questionBankIds: new List<Guid> { item.Id }, questionType: QuestionType.FillInTheBlanks);
                dtos.Add(dto);
            }
            return new PagedResultDto<QuestionBankListDto>(totalCount, dtos);
        }

        public virtual async Task<GetQuestionBankForEditorOutput> GetEditorAsync(Guid id)
        {
            QuestionBank entity = await QuestionBankRepository.GetAsync(id);

            return ObjectMapper.Map<QuestionBank, GetQuestionBankForEditorOutput>(entity);
        }

        [Authorize(ExamPermissions.QuestionBanks.Create)]
        public virtual async Task<QuestionBankListDto> CreateAsync(QuestionBankCreateDto input)
        {
            QuestionBank repository = await QuestionRepoManager.CreateAsync(input.Title);
            repository.Remark = input.Remark;

            repository = await QuestionBankRepository.InsertAsync(repository);
            return ObjectMapper.Map<QuestionBank, QuestionBankListDto>(repository);
        }

        [Authorize(ExamPermissions.QuestionBanks.Update)]
        public virtual async Task<QuestionBankListDto> UpdateAsync(Guid id, QuestionBankUpdateDto input)
        {
            QuestionBank repository = await QuestionBankRepository.GetAsync(id);
            await QuestionRepoManager.SetTitleAsync(repository, input.Title);
            repository.Remark = input.Remark;
            repository = await QuestionBankRepository.UpdateAsync(repository);
            return ObjectMapper.Map<QuestionBank, QuestionBankListDto>(repository);
        }

        [Authorize(ExamPermissions.QuestionBanks.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            QuestionBank questionBank = await QuestionBankRepository.GetAsync(id);
            await QuestionBankManager.DeleteAsync(questionBank);
        }
    }
}