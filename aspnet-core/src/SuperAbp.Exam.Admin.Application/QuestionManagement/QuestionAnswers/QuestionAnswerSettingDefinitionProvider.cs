using Volo.Abp.Settings;

namespace SuperAbp.Exam.Admin.QuestionManagement.QuestionAnswers
{
    /// <summary>
    /// SettingDefinitionProvider
    /// </summary>
    public class QuestionAnswerSettingDefinitionProvider : SettingDefinitionProvider
    {
    /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(
                new SettingDefinition(
                    QuestionAnswerSettings.MaxPageSize,
                    "100",
                    isVisibleToClients: true
                )
            );
        }
    }
}
