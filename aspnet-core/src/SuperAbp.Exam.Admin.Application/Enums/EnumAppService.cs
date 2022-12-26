using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SuperAbp.Exam.QuestionManagement.Questions;
using Volo.Abp.Caching;

namespace SuperAbp.Exam.Admin.Enums;

public class EnumAppService:ExamAppService, IEnumAppService
{
    private readonly IDistributedCache<Dictionary<int, string>> _distributedCache;

    public EnumAppService(IDistributedCache<Dictionary<int, string>> distributedCache)
    {
        _distributedCache = distributedCache;
    }

    /// <summary>
    /// 银行卡类型
    /// </summary>
    /// <returns></returns>
    public async Task<IDictionary<int, string>> GetQuestionTypeAsync()
    {
        return await _distributedCache.GetOrAddAsync("enum_question_type",
            async () => await Task.FromResult(ToLocale(typeof(QuestionType))));
    }


    private Dictionary<int, string> ToLocale(Type enumType)
    {
        return Enum.GetValues(enumType).Cast<object>().ToDictionary(e => (int)e, e => L[$"{enumType.Name}:{e}"].Value);
    }

}