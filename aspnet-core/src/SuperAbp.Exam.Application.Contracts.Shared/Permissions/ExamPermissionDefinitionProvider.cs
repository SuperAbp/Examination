using SuperAbp.Exam.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace SuperAbp.Exam.Permissions;

public class ExamPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(ExamPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(ExamPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ExamResource>(name);
    }
}