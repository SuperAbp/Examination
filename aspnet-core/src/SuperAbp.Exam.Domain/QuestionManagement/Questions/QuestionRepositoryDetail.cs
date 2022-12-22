using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperAbp.Exam.QuestionManagement.Questions
{
    public class QuestionRepositoryDetail
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 题库
        /// </summary>
        public string QuestionRepository { get; set; }

        public QuestionType QuestionType { get; set; }

        /// <summary>
        /// 题干
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 解析
        /// </summary>
        public string Analysis { get; set; }
    }
}
