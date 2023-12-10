using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperAbp.Exam.ExamManagement.UserExams
{
    public class UserExamWithExam
    {

        public Guid ExamId { get; set; }
        /// <summary>
        /// 考试名称
        /// </summary>
        public string ExamName { get; set; }
        /// <summary>
        /// 次数
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 最高分
        /// </summary>
        public decimal MaxScore { get; set; }

        public DateTime LastTime { get; set; }
    }
}
