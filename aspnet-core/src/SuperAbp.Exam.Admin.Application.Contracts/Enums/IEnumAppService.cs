using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace SuperAbp.Exam.Admin.Enums;


public interface IEnumAppService:IApplicationService
{
    /// <summary>
    /// 银行卡类型
    /// </summary>
    /// <returns></returns>
    Task<IDictionary<int, string>> GetQuestionTypeAsync();
}