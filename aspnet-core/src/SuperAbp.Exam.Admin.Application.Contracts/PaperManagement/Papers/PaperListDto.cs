using SuperAbp.Exam.PaperManagement.Papers;
using System;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.PaperManagement.Papers
{
    /// <summary>
    /// 列表
    /// </summary>
    public class PaperListDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public decimal Score { get; set; }

        public int PaperType { get; set; }
    }
}