using SuperAbp.Exam.QuestionManagement.Questions;
using System;

namespace SuperAbp.Exam.Favorites;

public class FavoriteListDto
{
    public Guid Id { get; set; }

    public Guid QuestionId { get; set; }

    public required string QuestionContent { get; set; }

    public required QuestionType QuestionType { get; set; }

    public DateTime CreationTime { get; set; }
}