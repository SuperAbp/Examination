using System;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.ExamManagement.UserExams;

public class GetUserExamWithUsersInput : PagedAndSortedResultRequestDto
{
    public required Guid ExamId { get; set; }
}