using Volo.Abp.Settings;

namespace SuperAbp.Exam.Admin.PaperManagement.PaperQuestionRules
{
    /// <summary>
    /// SettingDefinitionProvider
    /// </summary>
    public class PaperQuestionRuleSettingDefinitionProvider : SettingDefinitionProvider
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(
                new SettingDefinition(
                    PaperQuestionRuleSettings.MaxPageSize,
                    "100",
                    isVisibleToClients: true
                )
            );
        }
    }
}