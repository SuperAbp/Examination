using System;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.QuestionManagement.QuestionAnswers
{
    /// <summary>
    /// 列表
    /// </summary>
    public class QuestionAnswerListDto : EntityDto<Guid>
    {
        /// <summary>
        /// 是否正确
        /// </summary>
        public bool Right { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 解析
        /// </summary>
        public string Analysis { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
    }
}