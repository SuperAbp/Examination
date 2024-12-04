using System.Collections.Generic;

namespace SuperAbp.Exam.Admin.QuestionManagement.Questions;

public interface IQuestionAnalysis
{
    List<QuestionImportModel> Analyse(string[] lines);
}