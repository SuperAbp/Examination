using System;
using Volo.Abp.DependencyInjection;

namespace SuperAbp.Exam;

public class ExamTestData : ISingletonDependency
{
    public Guid UserId = new Guid("2e701e62-0953-4dd3-910b-dc6cc93ccb0d");

    public Guid Question11Id = Guid.NewGuid();
    public Guid Question12Id = Guid.NewGuid();
    public Guid Question21Id = Guid.NewGuid();
    public Guid Question22Id = Guid.NewGuid();
    public string Question11Content1 = "Question11的Content";
    public string Question12Content2 = "Question12的Content";
    public string Question21Content1 = "Question21的Content";
    public string Question22Content2 = "Question22的Content";

    public Guid Answer111Id = Guid.NewGuid();
    public string Answer111Content = "Answer1的Content";
    public Guid Answer112Id = Guid.NewGuid();
    public string Answer112Content = "Answer2的Content";
    public Guid Answer113Id = Guid.NewGuid();
    public string Answer113Content = "Answer3的Content";
    public Guid Answer114Id = Guid.NewGuid();
    public string Answer114Content = "Answer4的Content";
    public Guid Answer121Id = Guid.NewGuid();
    public string Answer121Content = "Answer1的Content";
    public Guid Answer122Id = Guid.NewGuid();
    public string Answer122Content = "Answer2的Content";
    public Guid Answer123Id = Guid.NewGuid();
    public string Answer123Content = "Answer3的Content";
    public Guid Answer124Id = Guid.NewGuid();
    public string Answer124Content = "Answer4的Content";

    public Guid Answer211Id = Guid.NewGuid();
    public string Answer211Content = "Answer1的Content";
    public Guid Answer212Id = Guid.NewGuid();
    public string Answer212Content = "Answer2的Content";
    public Guid Answer213Id = Guid.NewGuid();
    public string Answer213Content = "Answer3的Content";
    public Guid Answer214Id = Guid.NewGuid();
    public string Answer214Content = "Answer4的Content";
    public Guid Answer221Id = Guid.NewGuid();
    public string Answer221Content = "Answer1的Content";
    public Guid Answer222Id = Guid.NewGuid();
    public string Answer222Content = "Answer2的Content";
    public Guid Answer223Id = Guid.NewGuid();
    public string Answer223Content = "Answer3的Content";
    public Guid Answer224Id = Guid.NewGuid();
    public string Answer224Content = "Answer4的Content";

    public Guid QuestionCategory1Id = Guid.NewGuid();
    public string QuestionCategory1Name = "QuestionCategory1的Name";
    public Guid QuestionCategory11Id = Guid.NewGuid();
    public string QuestionCategory11Name = "QuestionCategory11的Name";
    public Guid QuestionCategory2Id = Guid.NewGuid();
    public string QuestionCategory2Name = "QuestionCategory2的Name";

    public Guid QuestionRepository1Id = Guid.NewGuid();
    public Guid QuestionRepository2Id = Guid.NewGuid();
    public string QuestionRepository1Title = "Question Repository1的Title";
    public string QuestionRepository2Title = "Question Repository2的Title";

    public Guid Paper1Id = Guid.NewGuid();
    public string Paper1Name = "Paper1的Name";

    public Guid Paper2Id = Guid.NewGuid();
    public string Paper2Name = "Paper2的Name";

    public Guid Examination11Id = Guid.NewGuid();
    public string Examination11Name = "Examination11的Name";

    public Guid Examination12Id = Guid.NewGuid();
    public string Examination12Name = "Examination12的Name";

    public Guid Examination21Id = Guid.NewGuid();
    public string Examination21Name = "Examination21的Name";

    public Guid Examination22Id = Guid.NewGuid();
    public string Examination22Name = "Examination22的Name";

    public Guid PaperRepository1Id = Guid.NewGuid();

    public Guid PaperRepository2Id = Guid.NewGuid();

    public Guid UserExam11Id = Guid.NewGuid();
    public Guid UserExam12Id = Guid.NewGuid();

    public Guid UserExam21Id = Guid.NewGuid();
    public Guid UserExam22Id = Guid.NewGuid();

    public Guid UserExamQuestion11Id = Guid.NewGuid();
    public Guid UserExamQuestion12Id = Guid.NewGuid();

    public Guid UserExamQuestion21Id = Guid.NewGuid();
    public Guid UserExamQuestion22Id = Guid.NewGuid();

    public Guid Training1Id = Guid.NewGuid();
    public Guid Training2Id = Guid.NewGuid();
}