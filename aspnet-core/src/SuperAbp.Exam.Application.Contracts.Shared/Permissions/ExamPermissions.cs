namespace SuperAbp.Exam.Permissions;

public static class ExamPermissions
{
    public const string GroupName = "Exam";

    public static class Questions
    {
        public const string Default = GroupName + ".Question";
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

    public static class QuestionRepositories
    {
        public const string Default = GroupName + ".QuestionRepository";
        public const string Management = Default + ".Management";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

public static class Examings
{
public const string Default = GroupName + ".Examing";
public const string Create = Default + ".Create";
public const string Update = Default + ".Update";
public const string Delete = Default + ".Delete";
}
}