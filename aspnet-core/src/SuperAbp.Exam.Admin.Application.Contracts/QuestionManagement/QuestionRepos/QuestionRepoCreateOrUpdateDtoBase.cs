namespace SuperAbp.Exam.Admin.QuestionManagement.QuestionRepos
{
    public class QuestionRepoCreateOrUpdateDtoBase
    {
        public required string Title { get; set; }
        public string? Remark { get; set; }
    }
}