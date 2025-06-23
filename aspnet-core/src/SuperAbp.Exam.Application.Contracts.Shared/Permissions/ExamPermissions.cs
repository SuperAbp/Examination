namespace SuperAbp.Exam.Permissions;

public static class ExamPermissions
{
    public const string GroupName = "Exam";

    public static class Questions
    {
        public const string Default = GroupName + ".Question";
        public const string Management = Default + ".Management";
        public const string Import = Default + ".Import";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static class KnowledgePoints
    {
        public const string Default = GroupName + ".KnowledgePoint";
        public const string Management = Default + ".Management";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static class QuestionAnswers
    {
        public const string Default = GroupName + ".QuestionAnswer";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static class QuestionBanks
    {
        public const string Default = GroupName + ".QuestionBank";
        public const string Management = Default + ".Management";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static class Papers
    {
        public const string Default = GroupName + ".Paper";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static class PaperQuestionRules
    {
        public const string Default = GroupName + ".PaperQuestionRule";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static class Exams
    {
        public const string Default = GroupName + ".Exam";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Publish = Default + ".Publish";
        public const string Cancel = Default + ".Cancel";
        public const string Delete = Default + ".Delete";
    }
}