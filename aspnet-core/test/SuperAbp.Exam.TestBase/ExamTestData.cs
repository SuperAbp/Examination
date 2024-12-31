using System;
using Volo.Abp.DependencyInjection;

namespace SuperAbp.Exam;

public class ExamTestData : ISingletonDependency
{
    public Guid Question1Id = Guid.NewGuid();
    public Guid Question2Id = Guid.NewGuid();
    public string Question1Content = "Question1的Content";
    public Guid Answer1Id = Guid.NewGuid();
    public Guid Answer2Id = Guid.NewGuid();
    public Guid Answer3Id = Guid.NewGuid();
    public Guid Answer4Id = Guid.NewGuid();
    public Guid QuestionRepository1Id = Guid.NewGuid();
    public Guid QuestionRepository2Id = Guid.NewGuid();
    public string QuestionRepository1Title = "Question Repository1的Title";
}