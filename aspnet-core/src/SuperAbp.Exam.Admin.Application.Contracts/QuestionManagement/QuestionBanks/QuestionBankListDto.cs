using System;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.QuestionManagement.QuestionBanks
{
    /// <summary>
    /// 列表
    /// </summary>
    public class QuestionBankListDto : EntityDto<Guid>
    {
        public string Title { get; set; }
        public int SingleCount { get; set; }
        public int JudgeCount { get; set; }
        public int MultiCount { get; set; }
        public int BlankCount { get; set; }
    }
}