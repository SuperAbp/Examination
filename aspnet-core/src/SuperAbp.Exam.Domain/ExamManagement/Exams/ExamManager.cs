using System;
using System.Threading.Tasks;
using SuperAbp.Exam.Settings;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.Settings;

namespace SuperAbp.Exam.ExamManagement.Exams;

public class ExamManager(
    IExamRepository examRepository,
    ISettingProvider settingProvider) : DomainService
{
    /// <summary>
    /// 检查考试时间
    /// </summary>
    /// <param name="examId">考试Id</param>
    /// <returns></returns>
    public async Task CheckTimeAsync(Guid examId)
    {
        //TODO: 职责过多？
        Examination exam = await examRepository.GetAsync(examId);

        if (exam.Status != ExaminationStatus.Published)
        {
            throw new InvalidExamStatusException(exam.Status);
        }
        if (!exam.StartTime.HasValue || !exam.EndTime.HasValue)
        {
            return;
        }

        int bufferTime = await settingProvider.GetAsync<int>(ExamSettings.BufferTime);
        if (exam.StartTime.Value > Clock.Now || exam.EndTime.Value < Clock.Now.AddMinutes(exam.TotalTime + bufferTime))
        {
            throw new BusinessException(ExamDomainErrorCodes.Exams.OutOfExamTime);
        }
    }
}