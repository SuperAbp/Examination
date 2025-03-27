using SuperAbp.Exam.QuestionManagement.Questions;
using System;

namespace SuperAbp.Exam.Favorites;

public class FavoriteWithDetails
{
    public Guid Id { get; set; }
    public required string QuestionContent { get; set; }
    public required QuestionType QuestionType { get; set; }

    public DateTime CreationTime { get; set; }
}