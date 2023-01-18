using Volo.Abp.Settings;

namespace SuperAbp.Exam.Admin.ExamManagement.Exams
{
    /// <summary>
    /// SettingDefinitionProvider
    /// </summary>
    public class ExamingSettingDefinitionProvider : SettingDefinitionProvider
    {
    /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(
                new SettingDefinition(
                    ExamingSettings.MaxPageSize,
                    "100",
                    isVisibleToClients: true
                )
            );
        }
    }
}
