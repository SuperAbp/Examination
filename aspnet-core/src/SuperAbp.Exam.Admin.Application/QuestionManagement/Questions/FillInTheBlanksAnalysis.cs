using SuperAbp.Exam.QuestionManagement.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Volo.Abp.DependencyInjection;
using static SuperAbp.Exam.Permissions.ExamPermissions;

namespace SuperAbp.Exam.Admin.QuestionManagement.Questions;

public class FillInTheBlanksAnalysis : IQuestionAnalysis, ITransientDependency
{
    public List<QuestionImportModel> Analyse(string[] lines)
    {
        char[] splitSymbol = ['|'];
        Regex titleRegex = new(@"^(?:\d+)(?:[.、]|\s)(.*)");
        Regex answerRegex = new Regex(@"【([^】]+)】");
        List<QuestionImportModel> questions = [];
        foreach (string line in lines)
        {
            string formatLine = line.Trim();
            if (String.IsNullOrWhiteSpace(formatLine))
            {
                continue;
            }
            if (formatLine.StartsWith("答案："))
            {
                QuestionImportModel question = questions.Last();
                if (question.Options.Count > 0)
                {
                    continue;
                }

                string[] results = formatLine.Replace("答案：", String.Empty).Trim().Split(splitSymbol, StringSplitOptions.RemoveEmptyEntries);
                if (results.Length == 0)
                {
                    continue;
                }

                question.Options.AddRange(results.Select(r => new QuestionImportModel.Option { Content = r }));
            }
            else if (formatLine.StartsWith("解析："))
            {
                questions.Last().Analysis = formatLine.Replace("解析：", String.Empty).Trim();
            }
            else
            {
                string[] options = GetOptionsInTitle(answerRegex, ref formatLine);
                string title = formatLine;
                if (titleRegex.IsMatch(formatLine))
                {
                    Match match = titleRegex.Match(formatLine);
                    title = match.Groups[^1].Value.Trim();
                }
                questions.Add(new QuestionImportModel
                {
                    Title = title,
                    Options = options.Select(r => new QuestionImportModel.Option { Content = r }).ToList()
                });
            }
        }

        return questions;
    }

    private string[] GetOptionsInTitle(Regex answerRegex, ref string line)
    {
        if (!answerRegex.IsMatch(line))
        {
            return [];
        }
        MatchCollection matches = answerRegex.Matches(line);
        line = answerRegex.Replace(line, "___");
        return matches.Select(m => m.Groups[^1].Value.Trim()).ToArray();
    }
}