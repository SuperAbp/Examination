using Volo.Abp.Settings;

namespace SuperAbp.Exam.ExamManagement.UserExams
{
    /// <summary>
    /// SettingDefinitionProvider
    /// </summary>
    public class UserExamSettingDefinitionProvider : SettingDefinitionProvider
    {
    /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(
                new SettingDefinition(
                    UserExamSettings.MaxPageSize,
                    "100",
                    isVisibleToClients: true
                )
            );
        }
    }
}
