using System;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.QuestionManagement.QuestionRepos
{
    /// <summary>
    /// 列表
    /// </summary>
    public class QuestionRepoListDto : FullAuditedEntityDto<Guid>
    {
        public string Title { get; set; }
    }
}