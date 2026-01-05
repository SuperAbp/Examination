namespace SuperAbp.Exam.Permissions;

public static class ExamPermissions
{
    public const string GroupName = "Exam";

    public static class Questions
    {
        public const string Default = GroupName + ".Questions";
        public const string Management = Default + ".Management";
        public const string Import = Default + ".Import";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static class KnowledgePoints
    {
        public const string Default = GroupName + ".KnowledgePoints";
        public const string Management = Default + ".Management";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static class QuestionBanks
    {
        public const string Default = GroupName + ".QuestionBanks";
        public const string Management = Default + ".Management";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static class Papers
    {
        public const string Default = GroupName + ".Papers";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static class PaperQuestionRules
    {
        public const string Default = GroupName + ".PaperQuestionRules";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static class Exams
    {
        public const string Default = GroupName + ".Exams";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Publish = Default + ".Publish";
        public const string Terminate = Default + ".Terminate";
        public const string Complete = Default + ".Complete";
        public const string Cancel = Default + ".Cancel";
        public const string Invalidate = Default + ".Invalidate";
        public const string Delete = Default + ".Delete";
    }
}