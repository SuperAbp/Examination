using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Volo.Abp.DependencyInjection;

namespace SuperAbp.Exam.Admin.QuestionManagement.Questions;

public class SelectAnalysis : IQuestionAnalysis, ITransientDependency
{
    public List<QuestionImportModel> Analyse(string[] lines)
    {
        List<QuestionImportModel> questions = [];
        Regex answerRegex = new Regex(@"(?<=[\(（])([A-Za-z\s]+?)(?=[\）)])");
        Regex titleRegex = new Regex(@"^(?:\d+)(?:[.、]|\s)(.*)");
        Regex optionRequiredRegex = new Regex(@"^(?:[A-Za-z]+)(?:[.、]|\s)(.*)");
        Regex optionRegex = new Regex(@"[A-Za-z][、.](.+?)(?=\s*[A-Za-z][、.]|$)");

        foreach (string line in lines)
        {
            string formatLine = line.Trim();
            if (formatLine.Length == 0)
            {
                continue;
            }
            if (optionRequiredRegex.IsMatch(formatLine))
            {
                MatchCollection matches = optionRegex.Matches(formatLine);
                questions.Last().Options.AddRange(matches.Select(m => new QuestionImportModel.Option() { Content = m.Groups[^1].Value }));
            }
            else if (formatLine.StartsWith("答案："))
            {
                QuestionImportModel question = questions.Last();
                if (question.Options.Count > 0)
                {
                    continue;
                }

                question.Answers.AddRange(formatLine.Replace("答案：", String.Empty).Trim().Select(c => c - 65));
            }
            else if (formatLine.StartsWith("解析："))
            {
                questions.Last().Analysis = formatLine.Replace("解析：", String.Empty).Trim();
            }
            else
            {
                char[] answers = GetAnswersInTitle(answerRegex, ref formatLine);
                string title = formatLine;
                if (titleRegex.IsMatch(formatLine))
                {
                    Match match = titleRegex.Match(formatLine);
                    title = match.Groups[^1].Value.Trim();
                }
                questions.Add(new QuestionImportModel
                {
                    Title = title,
                    Answers = answers.Select(c => c - 65).ToList()
                });
            }
        }

        return questions;
    }

    private char[] GetAnswersInTitle(Regex answerRegex, ref string line)
    {
        if (!answerRegex.IsMatch(line))
        {
            return [];
        }
        MatchCollection matches = answerRegex.Matches(line);
        line = answerRegex.Replace(line, String.Empty);
        return matches.SelectMany(m => m.Value.Trim().ToCharArray()).ToArray();
    }
}