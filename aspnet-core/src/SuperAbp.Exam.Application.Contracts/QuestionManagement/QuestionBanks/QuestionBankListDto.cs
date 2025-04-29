using System;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.QuestionManagement.QuestionBanks
{
    /// <summary>
    /// 列表
    /// </summary>
    public class QuestionBankListDto : FullAuditedEntityDto<Guid>
    {
        public string Title { get; set; }
    }
}