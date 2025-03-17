using System;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.QuestionManagement.Questions;

public class QuestionNumberDto
{
    public QuestionType QuestionType { get; set; }
    public int TotalCount { get; set; }
    public decimal TotalScore { get; set; }
}