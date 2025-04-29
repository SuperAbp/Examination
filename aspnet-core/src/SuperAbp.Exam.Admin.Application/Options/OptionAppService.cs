using SuperAbp.Exam.QuestionManagement.Questions;
using System.Collections.Generic;
using System.Linq;

namespace SuperAbp.Exam.Admin.Options;

public class OptionAppService : ExamAppService, IOptionAppService
{
    public Dictionary<int, string> GetQuestionTypes()
    {
        return QuestionType.List
            .Select(q => new { Key = q.Value, Value = q.Name })
            .ToDictionary(key => key.Key, value => value.Value);
    }
}