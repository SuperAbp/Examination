using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace SuperAbp.Exam.ExamManagement.Exams;

public class ExamManager(
    IExamRepository examRepository) : DomainService
{
    /// <summary>
    /// 检查考试时间
    /// </summary>
    /// <param name="examId">考试Id</param>
    /// <returns></returns>
    /// <exception cref="OutOfExamTimeException"></exception>
    /// <exception cref="OutOfExamTimeException"></exception>
    public async Task CheckCreateUserExamAsync(Guid examId)
    {
        Examination exam = await examRepository.GetAsync(examId);

        if (exam.Status != ExaminationStatus.Published)
        {
            throw new InvalidExamStatusException(exam.Status);
        }

        if (exam.StartTime > Clock.Now || exam.EndTime < Clock.Now)
        {
            throw new OutOfExamTimeException();
        }
    }
}