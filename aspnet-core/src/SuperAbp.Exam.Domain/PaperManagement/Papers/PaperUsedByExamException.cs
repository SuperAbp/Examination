using System;
using Volo.Abp;

namespace SuperAbp.Exam.PaperManagement.Papers;

[Serializable]
public class PaperUsedByExamException : BusinessException
{
    public PaperUsedByExamException() : base(ExamDomainErrorCodes.Papers.PaperUsedByExam)
    {
    }
}