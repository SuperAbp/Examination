using Volo.Abp.Settings;

namespace SuperAbp.Exam.Admin.QuestionManagement.Questions;

/// <summary>
/// SettingDefinitionProvider
/// </summary>
public class QuestionSettingDefinitionProvider : SettingDefinitionProvider
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="context"></param>
    public override void Define(ISettingDefinitionContext context)
    {
        context.Add(
            new SettingDefinition(
                QuestionSettings.MaxPageSize,
                "100",
                isVisibleToClients: true
            )
        );
    }
}