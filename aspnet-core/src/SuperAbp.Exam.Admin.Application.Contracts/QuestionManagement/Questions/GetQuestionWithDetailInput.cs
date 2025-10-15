using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.QuestionManagement.Questions
{
    public class GetQuestionWithDetailInput
    {
        public Guid? QuestionBankId { get; set; }
        public int? QuestionType { get; set; }

        public List<Guid>? IncludeIds { get; set; }
        public List<Guid>? ExcludeIds { get; set; }

        public int? Count { get; set; }
    }
}