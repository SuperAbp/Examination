using System;

namespace SuperAbp.Exam.Jobs.SubmittedUserExam
{
    public class SubmitUserExamArgs
    {
        public Guid ExamId { get; set; }
        public Guid? TenantId { get; set; }
    }
}