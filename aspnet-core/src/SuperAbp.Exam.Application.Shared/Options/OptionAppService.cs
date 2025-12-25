using SuperAbp.Exam.ExamManagement.Exams;
using SuperAbp.Exam.QuestionManagement.Questions;
using System.Collections.Generic;
using System.Linq;

namespace SuperAbp.Exam.Options;

public class OptionAppService : ExamAppServiceBase, IOptionAppService
{
    public Dictionary<int, string> GetQuestionTypes()
    {
        return QuestionType.List
            .Select(q => new { Key = q.Value, Value = q.Name })
            .ToDictionary(key => key.Key, value => value.Value);
    }

    public Dictionary<int, string> GetAnswerModes()
    {
        return AnswerMode.List
            .Select(a => new { Key = a.Value, Value = a.Name })
            .ToDictionary(key => key.Key, value => value.Value);
    }

    public Dictionary<int, string> GetReviewModes()
    {
        return ReviewMode.List
            .Select(r => new { Key = r.Value, Value = r.Name })
            .ToDictionary(key => key.Key, value => value.Value);
    }
}