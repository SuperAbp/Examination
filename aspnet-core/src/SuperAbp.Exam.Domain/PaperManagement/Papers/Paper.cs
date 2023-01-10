using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace SuperAbp.Exam.PaperManagement.Papers
{
    /// <summary>
    /// 试卷
    /// </summary>
    public class Paper : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 考试Id
        /// </summary>
        public Guid ExamId { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
    }
}