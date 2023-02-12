using Volo.Abp.Settings;

namespace SuperAbp.Exam.Admin.PaperManagement.Papers
{
    /// <summary>
    /// SettingDefinitionProvider
    /// </summary>
    public class PaperSettingDefinitionProvider : SettingDefinitionProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(
                new SettingDefinition(
                    PaperSettings.MaxPageSize,
                    "100",
                    isVisibleToClients: true
                )
            );
        }
    }
}
