using SuperAbp.Exam.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace SuperAbp.Exam.Permissions;

public class ExamPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(ExamPermissions.GroupName);

        var questions = myGroup.AddPermission(ExamPermissions.Questions.Default, L("Permission:Questions"));
        questions.AddChild(ExamPermissions.Questions.Create, L("Permission:Create"));
        questions.AddChild(ExamPermissions.Questions.Update, L("Permission:Edit"));
        questions.AddChild(ExamPermissions.Questions.Delete, L("Permission:Delete"));

        var questionAnswers = myGroup.AddPermission(ExamPermissions.QuestionAnswers.Default, L("Permission:QuestionAnswers"));
        questionAnswers.AddChild(ExamPermissions.QuestionAnswers.Create, L("Permission:Create"));
        questionAnswers.AddChild(ExamPermissions.QuestionAnswers.Update, L("Permission:Edit"));
        questionAnswers.AddChild(ExamPermissions.QuestionAnswers.Delete, L("Permission:Delete"));

        var questionRepos = myGroup.AddPermission(ExamPermissions.QuestionRepos.Default, L("Permission:QuestionRepos"));
        questionRepos.AddChild(ExamPermissions.QuestionRepos.Create, L("Permission:Create"));
        questionRepos.AddChild(ExamPermissions.QuestionRepos.Update, L("Permission:Edit"));
        questionRepos.AddChild(ExamPermissions.QuestionRepos.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ExamResource>(name);
    }
}