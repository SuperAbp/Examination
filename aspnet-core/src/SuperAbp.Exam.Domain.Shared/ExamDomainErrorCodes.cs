namespace SuperAbp.Exam;

public static class ExamDomainErrorCodes
{
    public static string ExistsUnfinishedExams = "Exam:000001";
    public static string OutOfExamTime = "Exam:000002";

    public static class Questions
    {
        public const string ContentAlreadyExists = "Exam:Question:0001";
    }
}