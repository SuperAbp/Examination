namespace SuperAbp.Exam.Admin.QuestionManagement.QuestionBanks
{
    public class QuestionBankCreateOrUpdateDtoBase
    {
        public required string Title { get; set; }
        public string? Remark { get; set; }
    }
}