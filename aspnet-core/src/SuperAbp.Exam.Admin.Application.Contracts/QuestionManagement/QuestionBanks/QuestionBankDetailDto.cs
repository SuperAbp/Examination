using System;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.QuestionManagement.QuestionBanks
{
    /// <summary>
    /// 列表
    /// </summary>
    public class QuestionBankDetailDto : EntityDto<Guid>
    {
        public string Title { get; set; }
        public string Remark { get; set; }
    }
}