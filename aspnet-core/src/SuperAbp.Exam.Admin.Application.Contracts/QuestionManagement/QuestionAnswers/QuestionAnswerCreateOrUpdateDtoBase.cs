namespace SuperAbp.Exam.Admin.QuestionManagement.QuestionAnswers
{
    public class QuestionAnswerCreateOrUpdateDtoBase
    {
        public bool Right { get; set; }
        public string Content { get; set; }
        public string Analysis { get; set; }
        public System.Guid QuestionId { get; set; }
    }
}