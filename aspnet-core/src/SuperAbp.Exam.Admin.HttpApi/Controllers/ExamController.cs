using SuperAbp.Exam.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace SuperAbp.Exam.Admin.Controllers;

/* Inherit your controllers from this class.
 */

public abstract class ExamController : AbpControllerBase
{
    protected ExamController()
    {
        LocalizationResource = typeof(ExamResource);
    }
}