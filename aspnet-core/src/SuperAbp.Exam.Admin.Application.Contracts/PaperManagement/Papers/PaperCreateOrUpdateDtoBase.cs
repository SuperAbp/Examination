using System;

namespace SuperAbp.Exam.Admin.PaperManagement.Papers
{
    public class PaperCreateOrUpdateDtoBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Score { get; set; }
    }
}