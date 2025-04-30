using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SuperAbp.Exam.ExamManagement.UserExamQuestions;
using Volo.Abp.Application.Dtos;
using SuperAbp.Exam.ExamManagement.UserExams;

namespace SuperAbp.Exam.Admin.ExamManagement.UserExams;

public class UserExamAdminAppService(IUserExamRepository userExamRepository) : ExamAppService, IUserExamAdminAppService
{
    protected IUserExamRepository UserExamRepository { get; } = userExamRepository;

    public virtual async Task<PagedResultDto<UserExamListDto>> GetListAsync(GetUserExamsInput input)
    {
        int totalCount = await UserExamRepository.GetCountAsync(examId: input.ExamId);
        List<UserExam> entities = await UserExamRepository.GetListAsync(
            input.Sorting ?? UserExamConsts.DefaultSorting, input.SkipCount, input.MaxResultCount,
            examId: input.ExamId);

        List<UserExamListDto> dtos = ObjectMapper.Map<List<UserExam>, List<UserExamListDto>>(entities);
        return new PagedResultDto<UserExamListDto>(totalCount, dtos);
    }

    public async Task<UserExamDetailDto> GetDetailAsync(Guid userExamId)
    {
        UserExam userExam = await UserExamRepository.GetAsync(userExamId);
        var dto = ObjectMapper.Map<UserExam, UserExamDetailDto>(userExam);
        dto.Questions =
            ObjectMapper.Map<ICollection<UserExamQuestion>, List<UserExamDetailDto.UserExamQuestionDto>>(userExam.Questions);
        return dto;
    }
}