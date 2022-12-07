using System;
using System.Collections.Generic;
using System.Text;
using SuperAbp.Exam.Localization;
using Volo.Abp.Application.Services;

namespace SuperAbp.Exam;

/* Inherit your application services from this class.
 */
public abstract class ExamAppService : ApplicationService
{
    protected ExamAppService()
    {
        LocalizationResource = typeof(ExamResource);
    }
}
