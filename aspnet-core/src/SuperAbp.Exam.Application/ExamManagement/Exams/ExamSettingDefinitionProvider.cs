using Volo.Abp.Settings;

namespace SuperAbp.Exam.ExamManagement.Exams
{
    /// <summary>
    /// SettingDefinitionProvider
    /// </summary>
    public class ExamSettingDefinitionProvider : SettingDefinitionProvider
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(
                new SettingDefinition(
                    ExamSettings.MaxPageSize,
                    "100",
                    isVisibleToClients: true
                )
            );
        }
    }
}