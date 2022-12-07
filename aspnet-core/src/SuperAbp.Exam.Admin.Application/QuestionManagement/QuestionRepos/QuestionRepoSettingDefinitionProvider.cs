using Volo.Abp.Settings;

namespace SuperAbp.Exam.Admin.QuestionManagement.QuestionRepos
{
    /// <summary>
    /// SettingDefinitionProvider
    /// </summary>
    public class QuestionRepoSettingDefinitionProvider : SettingDefinitionProvider
    {
    /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(
                new SettingDefinition(
                    QuestionRepoSettings.MaxPageSize,
                    "100",
                    isVisibleToClients: true
                )
            );
        }
    }
}
