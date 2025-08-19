using Volo.Abp.Settings;

namespace SuperAbp.Exam.Settings;

public class ExamSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        context.Add(new SettingDefinition(ExamSettings.BufferTime, "10"));
    }
}