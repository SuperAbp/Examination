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
        public string ExamName { get; set; }

        /// <summary>
        /// 总分
        /// </summary>
        public decimal TotalScore { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
