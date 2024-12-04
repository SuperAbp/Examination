using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace ConsoleApp1;

internal class Program
{
    private static async Task Main(string[] args)
    {
        List<Question> questions;
        string content = await File.ReadAllTextAsync(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Doc.txt"));
        string[] lines = content.Split(["\r\n"], StringSplitOptions.None);
        questions = SelectMatch(lines);
        // questions = JudgeMatch(lines);
        // questions = BlackMatch(lines);
        for (int i = 0; i < questions.Count; i++)
        {
            Question question = questions[i];
            Console.WriteLine($"{i + 1}. {question.Title}");
            for (int j = 0; j < question.Options.Count; j++)
            {
                string option = question.Options[j];
                Console.WriteLine($"{(char)(65 + j)}. {option}");
            }
            string answerResult = String.Join("||", question.Answers);
            if (String.IsNullOrWhiteSpace(answerResult))
            {
                answerResult = String.Join("||", question.Answers1);
            }

            if (!String.IsNullOrWhiteSpace(answerResult))
            {
                Console.WriteLine($"答案：{answerResult}");
            }

            if (!String.IsNullOrWhiteSpace(question.Analysis))
            {
                Console.WriteLine($"解析：{question.Analysis}");
            }

            Console.WriteLine();
        }
        Console.WriteLine("成功");
        Console.ReadKey();
    }

    private static List<Question> BlackMatch(string[] lines)
    {
        char[] splitSymbol = ['|'];
        Regex titleRegex = new(@"^(?:\d+)(?:[.、]|\s)(.*)");
        Regex answerRegex = new Regex(@"【([^】]+)】");
        List<Question> questions = [];
        foreach (string line in lines)
        {
            string formatLine = line.Trim();
            if (String.IsNullOrWhiteSpace(formatLine))
            {
                continue;
            }
            if (formatLine.StartsWith("答案："))
            {
                Question lastQuestion = questions.Last();
                if (lastQuestion.Answers.Length > 0)
                {
                    continue;
                }

                string[] results = formatLine.Replace("答案：", String.Empty).Trim().Split(splitSymbol, StringSplitOptions.RemoveEmptyEntries);
                if (results.Length == 0)
                {
                    continue;
                }

                lastQuestion.Answers1 = results;
            }
            else if (formatLine.StartsWith("解析："))
            {
                questions.Last().Analysis = formatLine.Replace("解析：", String.Empty).Trim();
            }
            else
            {
                string[] answers = GetAnswersInTitleForBlack(answerRegex, ref formatLine);
                string title = formatLine;
                if (titleRegex.IsMatch(formatLine))
                {
                    Match match = titleRegex.Match(formatLine);
                    title = match.Groups[^1].Value.Trim();
                }
                questions.Add(new Question
                {
                    Title = title,
                    Answers1 = answers
                });
            }
        }

        return questions;
    }

    private static List<Question> JudgeMatch(string[] lines)
    {
        List<Question> questions = [];
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
                Question lastQuestion = questions.Last();
                if (lastQuestion.Answers.Length > 0)
                {
                    continue;
                }

                string result = formatLine.Replace("答案：", String.Empty).Trim();
                if (!right.Union(wrong).Contains(result))
                {
                    throw new Exception($"不支持的选项。请使用下列值：{String.Join("，", right.Union(wrong))}");
                }
                lastQuestion.Answers = GetResult(right, result);
            }
            else if (formatLine.StartsWith("解析："))
            {
                questions.Last().Analysis = formatLine.Replace("解析：", String.Empty).Trim();
            }
            else
            {
                char[] answers = GetAnswersInTitleForJudge(answerRegex, right, ref formatLine);
                string title = formatLine;
                if (titleRegex.IsMatch(formatLine))
                {
                    Match match = titleRegex.Match(formatLine);
                    title = match.Groups[^1].Value.Trim();
                }
                questions.Add(new Question
                {
                    Title = title,
                    Answers = answers
                });
            }
        }
        return questions;
    }

    private static List<Question> SelectMatch(string[] lines)
    {
        List<Question> questions = [];
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
                questions.Last().Options.AddRange(matches.Select(m => m.Groups[^1].Value));
            }
            else if (formatLine.StartsWith("答案："))
            {
                Question lastQuestion = questions.Last();
                if (lastQuestion.Answers.Length > 0)
                {
                    continue;
                }
                lastQuestion.Answers = formatLine.Replace("答案：", String.Empty).Trim().ToCharArray();
            }
            else if (formatLine.StartsWith("解析："))
            {
                questions.Last().Analysis = formatLine.Replace("解析：", String.Empty).Trim();
            }
            else
            {
                char[] answers = GetAnswersInTitleForSelect(answerRegex, ref formatLine);
                string title = formatLine;
                if (titleRegex.IsMatch(formatLine))
                {
                    Match match = titleRegex.Match(formatLine);
                    title = match.Groups[^1].Value.Trim();
                }
                questions.Add(new Question
                {
                    Title = title,
                    Answers = answers
                });
            }
        }

        return questions;
    }

    private static string[] GetAnswersInTitleForBlack(Regex answerRegex, ref string line)
    {
        if (!answerRegex.IsMatch(line))
        {
            return [];
        }
        MatchCollection matches = answerRegex.Matches(line);
        line = answerRegex.Replace(line, "___");
        return matches.Select(m => m.Groups[^1].Value.Trim()).ToArray();
    }

    private static char[] GetAnswersInTitleForSelect(Regex answerRegex, ref string line)
    {
        if (!answerRegex.IsMatch(line))
        {
            return [];
        }
        MatchCollection matches = answerRegex.Matches(line);
        line = answerRegex.Replace(line, String.Empty);
        return matches.SelectMany(m => m.Value.Trim().ToCharArray()).ToArray();
    }

    private static char[] GetAnswersInTitleForJudge(Regex answerRegex, string[] right, ref string line)
    {
        if (!answerRegex.IsMatch(line))
        {
            return ['B'];
        }
        Match match = answerRegex.Match(line);
        line = answerRegex.Replace(line, String.Empty);
        return GetResult(right, match.Value.Trim());
    }

    private static char[] GetResult(string[] right, string result)
    {
        return right.Contains(result) ? ['A'] : ['B'];
    }
}

internal class Question
{
    public required string Title { get; set; }

    public char[] Answers { get; set; } = [];
    public string? Analysis { get; set; }

    public int AnswerCount { get; set; }

    public string[] Answers1 { get; set; }

    public List<string> Options { get; set; } = [];
}