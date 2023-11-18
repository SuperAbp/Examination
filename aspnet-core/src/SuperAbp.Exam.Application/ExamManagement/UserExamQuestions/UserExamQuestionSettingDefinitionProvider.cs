using Volo.Abp.Settings;

namespace SuperAbp.Exam.ExamManagement.UserExamQuestions
{
    /// <summary>
    /// SettingDefinitionProvider
    /// </summary>
    public class UserExamQuestionSettingDefinitionProvider : SettingDefinitionProvider
    {
    /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(
                new SettingDefinition(
                    UserExamQuestionSettings.MaxPageSize,
                    "100",
                    isVisibleToClients: true
                )
            );
        }
    }
}
