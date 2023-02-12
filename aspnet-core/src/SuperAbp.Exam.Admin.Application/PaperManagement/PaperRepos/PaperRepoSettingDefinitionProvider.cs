using Volo.Abp.Settings;

namespace SuperAbp.Exam.Admin.PaperManagement.PaperRepos
{
    /// <summary>
    /// SettingDefinitionProvider
    /// </summary>
    public class PaperRepoSettingDefinitionProvider : SettingDefinitionProvider
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(
                new SettingDefinition(
                    PaperRepoSettings.MaxPageSize,
                    "100",
                    isVisibleToClients: true
                )
            );
        }
    }
}