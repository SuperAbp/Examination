using Volo.Abp.Settings;

namespace SuperAbp.Exam.Admin.QuestionManagement.QuestionBanks
{
    /// <summary>
    /// SettingDefinitionProvider
    /// </summary>
    public class QuestionBankSettingDefinitionProvider : SettingDefinitionProvider
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(
                new SettingDefinition(
                    QuestionBankSettings.MaxPageSize,
                    "100",
                    isVisibleToClients: true
                )
            );
        }
    }
}