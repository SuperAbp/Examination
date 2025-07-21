using SuperAbp.Exam.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using static SuperAbp.Exam.Permissions.ExamPermissions;

namespace SuperAbp.Exam.Permissions;

public class ExamPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(ExamPermissions.GroupName, L("Permission:ExamManagement"));

        var questions = myGroup.AddPermission(ExamPermissions.Questions.Default, L("Permission:Questions"));
        questions.AddChild(ExamPermissions.Questions.Management, L("Permission:Management"));
        questions.AddChild(ExamPermissions.Questions.Import, L("Permission:Import"));
        questions.AddChild(ExamPermissions.Questions.Create, L("Permission:Create"));
        questions.AddChild(ExamPermissions.Questions.Update, L("Permission:Edit"));
        questions.AddChild(ExamPermissions.Questions.Delete, L("Permission:Delete"));

        var knowledgePoints = myGroup.AddPermission(ExamPermissions.KnowledgePoints.Default, L("Permission:KnowledgePoints"));
        knowledgePoints.AddChild(ExamPermissions.KnowledgePoints.Management, L("Permission:Management"));
        knowledgePoints.AddChild(ExamPermissions.KnowledgePoints.Create, L("Permission:Create"));
        knowledgePoints.AddChild(ExamPermissions.KnowledgePoints.Update, L("Permission:Edit"));
        knowledgePoints.AddChild(ExamPermissions.KnowledgePoints.Delete, L("Permission:Delete"));

        var questionBanks = myGroup.AddPermission(ExamPermissions.QuestionBanks.Default, L("Permission:QuestionBanks"));
        questionBanks.AddChild(ExamPermissions.QuestionBanks.Management, L("Permission:Management"));
        questionBanks.AddChild(ExamPermissions.QuestionBanks.Create, L("Permission:Create"));
        questionBanks.AddChild(ExamPermissions.QuestionBanks.Update, L("Permission:Edit"));
        questionBanks.AddChild(ExamPermissions.QuestionBanks.Delete, L("Permission:Delete"));

        var papers = myGroup.AddPermission(ExamPermissions.Papers.Default, L("Permission:Papers"));
        papers.AddChild(ExamPermissions.Papers.Create, L("Permission:Create"));
        papers.AddChild(ExamPermissions.Papers.Update, L("Permission:Edit"));
        papers.AddChild(ExamPermissions.Papers.Delete, L("Permission:Delete"));

        var paperQuestionRules = myGroup.AddPermission(ExamPermissions.PaperQuestionRules.Default, L("Permission:PaperQuestionRules"));
        paperQuestionRules.AddChild(ExamPermissions.PaperQuestionRules.Create, L("Permission:Create"));
        paperQuestionRules.AddChild(ExamPermissions.PaperQuestionRules.Update, L("Permission:Edit"));
        paperQuestionRules.AddChild(ExamPermissions.PaperQuestionRules.Delete, L("Permission:Delete"));

        var exams = myGroup.AddPermission(ExamPermissions.Exams.Default, L("Permission:Exams"));
        exams.AddChild(ExamPermissions.Exams.Create, L("Permission:Create"));
        exams.AddChild(ExamPermissions.Exams.Update, L("Permission:Edit"));
        exams.AddChild(ExamPermissions.Exams.Publish, L("Permission:Publish"));
        exams.AddChild(ExamPermissions.Exams.Cancel, L("Permission:Cancel"));
        exams.AddChild(ExamPermissions.Exams.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ExamResource>(name);
    }
}