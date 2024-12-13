using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Volo.Abp.DependencyInjection;

namespace SuperAbp.Exam.Admin.QuestionManagement.Questions;

public class JudgeAnalysis : IQuestionAnalysis, ITransientDependency
{
    public List<QuestionImportModel> Analyse(string[] lines)
    {
        List<QuestionImportModel> questions = [];
        Regex answerRegex = new(@"(?<=[\(（])\s*(正确|错误|对|错)\s*(?=[\）)])");
        Regex titleRegex = new(@"^(?:\d+)(?:[.、]|\s)(.*)");
        string[] right = ["正确", "对"];
        string[] wrong = ["错误", "错"];

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

                string result = formatLine.Replace("答案：", String.Empty).Trim();
                if (!right.Union(wrong).Contains(result))
                {
                    throw new Exception($"不支持的选项。请使用下列值：{String.Join("，", right.Union(wrong))}");
                }
                question.Answers = [GetResult(right, result) ? 0 : 1];
            }
            else if (formatLine.StartsWith("解析："))
            {
                questions.Last().Analysis = formatLine.Replace("解析：", String.Empty).Trim();
            }
            else
            {
                bool answer = GetAnswersInTitle(answerRegex, right, ref formatLine);
                string title = formatLine;
                if (titleRegex.IsMatch(formatLine))
                {
                    Match match = titleRegex.Match(formatLine);
                    title = match.Groups[^1].Value.Trim();
                }
                questions.Add(new QuestionImportModel
                {
                    Title = title,
                    Options =
                    [
                        new()
                        {
                            Content = "正确",
                        },
                        new()
                        {
                            Content = "错误",
                        }
                    ],
                    Answers = [answer ? 0 : 1]
                });
            }
        }
        return questions;
    }

    private bool GetAnswersInTitle(Regex answerRegex, string[] right, ref string line)
    {
        if (!answerRegex.IsMatch(line))
        {
            return false;
        }
        Match match = answerRegex.Match(line);
        line = answerRegex.Replace(line, String.Empty).Replace("()", "（）");
        return GetResult(right, match.Value.Trim());
    }

    private bool GetResult(string[] right, string result)
    {
        return right.Contains(result);
    }
}