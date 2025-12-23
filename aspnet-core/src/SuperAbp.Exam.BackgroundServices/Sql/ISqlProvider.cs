namespace SuperAbp.Exam.BackgroundServices.Sql;

/// <summary>
/// SQL语句提供者接口
/// 用于不同数据库实现提供各自的SQL语句
/// </summary>
public interface ISqlProvider
{
    /// <summary>
    /// 获取最后执行时间
    /// </summary>
    string GetLastExecutedTime();

    /// <summary>
    /// 获取租户ID
    /// </summary>
    string GetTenantIdByName();

    /// <summary>
    /// 获取题库列表
    /// </summary>
    string GetQuestionBanks();

    /// <summary>
    /// 获取问题计数（按题目类型分组）
    /// </summary>
    string GetQuestionCountsByType();

    /// <summary>
    /// 获取问题列表
    /// </summary>
    string GetQuestions();

    /// <summary>
    /// 获取试卷列表
    /// </summary>
    string GetPapers();

    /// <summary>
    /// 插入试卷
    /// </summary>
    string InsertPaper();

    /// <summary>
    /// 插入试卷分节
    /// </summary>
    string InsertPaperSections();

    /// <summary>
    /// 插入试卷问题规则
    /// </summary>
    string InsertPaperQuestionRules();

    /// <summary>
    /// 插入试卷问题
    /// </summary>
    string InsertPaperQuestions();

    /// <summary>
    /// 插入考试
    /// </summary>
    string InsertExaminations();

    /// <summary>
    /// 插入初始数据执行日志
    /// </summary>
    string InsertInitialDataExecutionLog();

    /// <summary>
    /// 删除试卷问题
    /// </summary>
    string DeletePaperQuestions();

    /// <summary>
    /// 删除试卷问题规则
    /// </summary>
    string DeletePaperQuestionRules();

    /// <summary>
    /// 删除试卷分节
    /// </summary>
    string DeletePaperSections();

    /// <summary>
    /// 删除试卷
    /// </summary>
    string DeletePapers();

    /// <summary>
    /// 删除用户考试问题
    /// </summary>
    string DeleteUserExamQuestions();

    /// <summary>
    /// 删除用户考试分节
    /// </summary>
    string DeleteUserExamSections();

    /// <summary>
    /// 删除用户考试
    /// </summary>
    string DeleteUserExams();

    /// <summary>
    /// 删除知识点
    /// </summary>
    string DeleteKnowledgePoints();

    /// <summary>
    /// 删除考试
    /// </summary>
    string DeleteExaminations();

    /// <summary>
    /// 删除问题答案
    /// </summary>
    string DeleteQuestionOptions();

    /// <summary>
    /// 删除问题
    /// </summary>
    string DeleteQuestions();

    /// <summary>
    /// 删除问题知识点关系
    /// </summary>
    string DeleteQuestionKnowledgePoints();

    /// <summary>
    /// 删除题库
    /// </summary>
    string DeleteQuestionBanks();

    /// <summary>
    /// 插入题库
    /// </summary>
    string InsertQuestionBanks();

    /// <summary>
    /// 插入问题
    /// </summary>
    string InsertQuestions();

    /// <summary>
    /// 插入问题答案
    /// </summary>
    string InsertQuestionOptions();

    /// <summary>
    /// 插入问题知识点关系
    /// </summary>
    string InsertQuestionKnowledgePoints();
}