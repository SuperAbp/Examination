using SuperAbp.Exam.ExamManagement.Exams;
using SuperAbp.Exam.ExamManagement.UserExams;
using SuperAbp.Exam.QuestionManagement.Questions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperAbp.Exam.Options;

public class OptionAppService : ExamAppServiceBase, IOptionAppService
{
    public Task<Dictionary<int, string>> GetQuestionTypes()
    {
        return Task.FromResult(QuestionType.List
            .Select(q => new { Key = q.Value, Value = q.Name })
            .ToDictionary(key => key.Key, value => value.Value));
    }

    public Task<Dictionary<int, string>> GetAnswerModes()
    {
        return Task.FromResult(AnswerMode.List
            .Select(a => new { Key = a.Value, Value = a.Name })
            .ToDictionary(key => key.Key, value => value.Value));
    }

    public Task<Dictionary<int, string>> GetReviewModes()
    {
        return Task.FromResult(ReviewMode.List
            .Select(r => new { Key = r.Value, Value = r.Name })
            .ToDictionary(key => key.Key, value => value.Value));
    }

    public Task<Dictionary<int, string>> GetExaminationStatus()
    {
        return Task.FromResult(ExaminationStatus.List
            .Select(r => new { Key = r.Value, Value = r.Name })
            .ToDictionary(key => key.Key, value => value.Value));
    }

    public Task<Dictionary<int, string>> GetUserExamStatus()
    {
        return Task.FromResult(UserExamStatus.List
            .Select(r => new { Key = r.Value, Value = r.Name })
            .ToDictionary(key => key.Key, value => value.Value));
    }
}
