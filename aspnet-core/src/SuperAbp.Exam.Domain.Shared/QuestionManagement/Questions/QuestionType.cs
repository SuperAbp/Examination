using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperAbp.Exam.QuestionManagement.Questions;

/// <summary>
/// 问题类型
/// </summary>
public enum QuestionType
{
    /// <summary>
    /// 单选
    /// </summary>
    SingleSelect,

    /// <summary>
    /// 多选
    /// </summary>
    MultiSelect,

    /// <summary>
    /// 判断
    /// </summary>
    Judge,

    /// <summary>
    /// 填空
    /// </summary>
    FillInTheBlanks
}