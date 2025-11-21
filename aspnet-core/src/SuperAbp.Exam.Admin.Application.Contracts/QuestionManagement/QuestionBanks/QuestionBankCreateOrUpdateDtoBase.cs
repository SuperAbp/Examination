using SuperAbp.Exam.QuestionManagement.QuestionBanks;
using System.ComponentModel.DataAnnotations;

namespace SuperAbp.Exam.Admin.QuestionManagement.QuestionBanks
{
    public class QuestionBankCreateOrUpdateDtoBase
    {
        [Required]
        [StringLength(QuestionBankConsts.MaxTitleLength)]
        public required string Title { get; set; }

        [StringLength(QuestionBankConsts.MaxRemarkLength)]
        public string? Remark { get; set; }
    }
}