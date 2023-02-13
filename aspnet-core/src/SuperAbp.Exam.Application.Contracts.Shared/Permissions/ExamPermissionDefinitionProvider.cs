using SuperAbp.Exam.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace SuperAbp.Exam.Permissions;

public class ExamPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(ExamPermissions.GroupName, L("Permission:ExamManagement"));

        var questions = myGroup.AddPermission(ExamPermissions.Questions.Default, L("Permission:Questions"));
        questions.AddChild(ExamPermissions.Questions.Management, L("Permission:Management"));
        questions.AddChild(ExamPermissions.Questions.Create, L("Permission:Create"));
        questions.AddChild(ExamPermissions.Questions.Update, L("Permission:Edit"));
        questions.AddChild(ExamPermissions.Questions.Delete, L("Permission:Delete"));

        var questionAnswers = myGroup.AddPermission(ExamPermissions.QuestionAnswers.Default, L("Permission:QuestionAnswers"));
        questionAnswers.AddChild(ExamPermissions.QuestionAnswers.Create, L("Permission:Create"));
        questionAnswers.AddChild(ExamPermissions.QuestionAnswers.Update, L("Permission:Edit"));
        questionAnswers.AddChild(ExamPermissions.QuestionAnswers.Delete, L("Permission:Delete"));

        var questionRepos = myGroup.AddPermission(ExamPermissions.QuestionRepositories.Default, L("Permission:QuestionRepositories"));
        questionRepos.AddChild(ExamPermissions.QuestionRepositories.Management, L("Permission:Management"));
        questionRepos.AddChild(ExamPermissions.QuestionRepositories.Create, L("Permission:Create"));
        questionRepos.AddChild(ExamPermissions.QuestionRepositories.Update, L("Permission:Edit"));
        questionRepos.AddChild(ExamPermissions.QuestionRepositories.Delete, L("Permission:Delete"));

        var papers = myGroup.AddPermission(ExamPermissions.Papers.Default, L("Permission:Papers"));
        papers.AddChild(ExamPermissions.Papers.Create, L("Permission:Create"));
        papers.AddChild(ExamPermissions.Papers.Update, L("Permission:Edit"));
        papers.AddChild(ExamPermissions.Papers.Delete, L("Permission:Delete"));

        var paperRepositories = myGroup.AddPermission(ExamPermissions.PaperRepos.Default, L("Permission:PaperRepositories"));
        paperRepositories.AddChild(ExamPermissions.PaperRepos.Create, L("Permission:Create"));
        paperRepositories.AddChild(ExamPermissions.PaperRepos.Update, L("Permission:Edit"));
        paperRepositories.AddChild(ExamPermissions.PaperRepos.Delete, L("Permission:Delete"));

        var examings = myGroup.AddPermission(ExamPermissions.Examings.Default, L("Permission:Examings"));
        examings.AddChild(ExamPermissions.Examings.Create, L("Permission:Create"));
        examings.AddChild(ExamPermissions.Examings.Update, L("Permission:Edit"));
        examings.AddChild(ExamPermissions.Examings.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ExamResource>(name);
    }
}