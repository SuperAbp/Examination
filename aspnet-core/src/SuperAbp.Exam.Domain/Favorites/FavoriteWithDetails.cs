using System;

namespace SuperAbp.Exam.Favorites;

public class FavoriteWithDetails
{
    public Guid Id { get; set; }
    public required string QuestionName { get; set; }

    public DateTime CreationTime { get; set; }
}