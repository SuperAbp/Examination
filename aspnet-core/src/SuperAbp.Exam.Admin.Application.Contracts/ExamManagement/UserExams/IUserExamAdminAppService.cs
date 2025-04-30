using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SuperAbp.Exam.Admin.ExamManagement.UserExams;

public interface IUserExamAdminAppService : IApplicationService
{
    Task<PagedResultDto<UserExamListDto>> GetListAsync(GetUserExamsInput input);

    Task<UserExamDetailDto> GetDetailAsync(Guid userExamId);
}