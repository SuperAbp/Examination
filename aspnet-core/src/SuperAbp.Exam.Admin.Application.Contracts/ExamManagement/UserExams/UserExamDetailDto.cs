using System.Collections.Generic;

namespace SuperAbp.Exam.Admin.ExamManagement.UserExams;

public class UserExamDetailDto
{
    public IReadOnlyList<UserExamQuestionDto> Questions { get; set; } = [];

    public class UserExamQuestionDto
    {
    }
}