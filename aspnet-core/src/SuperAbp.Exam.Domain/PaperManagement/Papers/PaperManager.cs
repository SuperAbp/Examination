using SuperAbp.Exam.ExamManagement.Exams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace SuperAbp.Exam.PaperManagement.Papers;

public class PaperManager(IPaperRepository paperRepository,
    IExamRepository examRepository) : DomainService
{
    protected IPaperRepository PaperRepository => paperRepository;
    protected IExamRepository ExamRepository => examRepository;

    public virtual async Task<Paper> CreateAsync(PaperType paperType, string name, bool manualReview)
    {
        await CheckNameAsync(name);
        return new Paper(GuidGenerator.Create(), paperType, name, manualReview);
    }

    public virtual async Task SetNameAsync(Paper question, string name)
    {
        if (name == question.Name)
        {
            return;
        }
        await CheckNameAsync(name);

        question.Name = name;
    }

    protected virtual async Task CheckNameAsync(string name)
    {
        if (await PaperRepository.NameExistsAsync(name))
        {
            throw new PaperNameAlreadyExistException(name);
        }
    }

    public virtual async Task DeleteAsync(Paper paper)
    {
        if (await examRepository.ExistsByPaperIdAsync(paper.Id))
        {
            throw new PaperUsedByExamException();
        }
        paper.PaperSections.Clear();
        await PaperRepository.DeleteAsync(paper);
    }

    /// <summary>
    /// 移除题目在所有试卷中的引用
    /// 用于题目删除时的清理工作，通过聚合根的方法确保数据一致性
    /// </summary>
    public virtual async Task RemoveQuestionFromAllPapersAsync(Guid questionId, Guid? tenantId = null)
    {
        // 获取所有包含该题目的试卷
        var papers = await PaperRepository.GetPapersByQuestionIdAsync(questionId);

        if (papers.Count == 0)
        {
            return;
        }

        // 批量处理：通过聚合根的方法移除题目引用，确保统计数据正确更新
        foreach (var paper in papers)
        {
            paper.RemoveQuestionByQuestionId(questionId);
        }

        // 批量更新，提高性能
        await PaperRepository.UpdateManyAsync(papers);
    }

    /// <summary>
    /// 移除知识点在所有试卷规则中的引用
    /// 用于知识点删除时的清理工作，通过聚合根的方法确保数据一致性
    /// </summary>
    public virtual async Task RemoveKnowledgePointFromAllPapersAsync(Guid knowledgePointId, Guid? tenantId = null)
    {
        var papers = await PaperRepository.GetPapersByKnowledgePointIdAsync(knowledgePointId);

        if (papers.Count == 0)
        {
            return;
        }

        foreach (var paper in papers)
        {
            paper.RemoveRulesByKnowledgePointId(knowledgePointId);
        }

        await PaperRepository.UpdateManyAsync(papers);
    }
}