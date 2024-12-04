using System.Collections.Generic;
using Volo.Abp.DependencyInjection;

namespace SuperAbp.Exam.Admin.QuestionManagement.Questions;

public class FillInTheBlanksAnalysis : IQuestionAnalysis, ITransientDependency
{
    public List<QuestionImportModel> Analyse(string[] lines)
    {
        throw new System.NotImplementedException();
    }
}