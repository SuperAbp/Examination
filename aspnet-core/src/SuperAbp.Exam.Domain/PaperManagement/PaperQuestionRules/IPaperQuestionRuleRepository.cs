using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace SuperAbp.Exam.PaperManagement.PaperQuestionRules
{
    /// <summary>
    /// 考试题库
    /// </summary>
    public interface IPaperQuestionRuleRepository : IRepository<PaperQuestionRule, Guid>
    {
    }
}