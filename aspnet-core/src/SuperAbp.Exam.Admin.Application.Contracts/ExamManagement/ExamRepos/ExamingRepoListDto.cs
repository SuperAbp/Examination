using System;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.ExamManagement.ExamRepos
{
    /// <summary>
    /// 列表
    /// </summary>
    public class ExamingRepoListDto : EntityDto
    {
        public int? SingleCount { get; set; }
        public decimal? SingleScore { get; set; }
        public int? MultiCount { get; set; }
        public decimal? MultiScore { get; set; }
        public int? JudgeCount { get; set; }
        public decimal? JudgeScore { get; set; }
    }
}