using System;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.QuestionManagement.QuestionRepos
{
    /// <summary>
    /// 列表
    /// </summary>
    public class QuestionRepoListDto: EntityDto<Guid>
    {
        public string Title { get; set; }
        public string Remark { get; set; }
    }
}