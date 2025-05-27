using System;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.ExamManagement.UserExams;

public class GetUserExamsInput : PagedAndSortedResultRequestDto
{
    public Guid ExamId { get; set; }

    public Guid UserId { get; set; }
}