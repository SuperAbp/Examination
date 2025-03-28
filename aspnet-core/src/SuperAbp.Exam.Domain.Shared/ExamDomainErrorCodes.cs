namespace SuperAbp.Exam;

public static class ExamDomainErrorCodes
{
    public static class Questions
    {
        public const string ContentAlreadyExists = "Exam:Question:0001";
        public const string CorrectCountError = "Exam:Question:0002";
    }

    public static class QuestionRepositories
    {
        public const string TitleAlreadyExists = "Exam: QuestionRepository:0001";
    }

    public static class QuestionAnswers
    {
        public const string ContentAlreadyExists = "Exam:Question:0001";
    }

    public static class Papers
    {
        public const string NameAlreadyExists = "Exam:Paper:0001";
    }

    public static class UserExams
    {
        public const string UnfinishedAlreadyExists = "Exam:UserExams:0001";
    }

    public static class Exams
    {
        public const string OutOfExamTime = "Exam:Exams:0001";
    }
}