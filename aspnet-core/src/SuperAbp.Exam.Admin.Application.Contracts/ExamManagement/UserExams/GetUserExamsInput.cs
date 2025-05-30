using System;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.ExamManagement.UserExams;

public class GetUserExamsInput : PagedAndSortedResultRequestDto
{
    public required Guid ExamId { get; set; }

    public required Guid UserId { get; set; }
}