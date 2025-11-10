using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SuperAbp.Exam.PaperManagement.PaperQuestionRules;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace SuperAbp.Exam.EntityFrameworkCore.PaperManagement.PaperQuestionRules;

/// <summary>
/// 考试题库
/// </summary>
public class PaperQuestionRuleRepository(IDbContextProvider<IExamDbContext> dbContextProvider)
    : EfCoreRepository<IExamDbContext, PaperQuestionRule, Guid>(dbContextProvider), IPaperQuestionRuleRepository
{
}