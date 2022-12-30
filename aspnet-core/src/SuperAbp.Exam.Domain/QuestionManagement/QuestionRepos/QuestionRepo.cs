using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace SuperAbp.Exam.QuestionManagement.QuestionRepos;

/// <summary>
/// 题库
/// </summary>
public class QuestionRepo : FullAuditedAggregateRoot<Guid>
{
    public QuestionRepo()
    {
    }

    public QuestionRepo(Guid id, string title) : base(id)
    {
        Title = title;
    }

    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; }
}