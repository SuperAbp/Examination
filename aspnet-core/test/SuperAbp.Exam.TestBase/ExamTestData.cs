using System;
using Volo.Abp.DependencyInjection;

namespace SuperAbp.Exam;

public class ExamTestData : ISingletonDependency
{
    public Guid User1Id = new Guid("2e701e62-0953-4dd3-910b-dc6cc93ccb0d");
    public Guid User2Id = Guid.NewGuid();
    public Guid User3Id = Guid.NewGuid();

    public Guid Question11Id = Guid.NewGuid();
    public Guid Question12Id = Guid.NewGuid();
    public Guid Question13Id = Guid.NewGuid();
    public Guid Question14Id = Guid.NewGuid();
    public Guid Question21Id = Guid.NewGuid();
    public Guid Question22Id = Guid.NewGuid();
    public Guid Question23Id = Guid.NewGuid();
    public Guid Question24Id = Guid.NewGuid();
    public string Question11Content = "Question11的Content";
    public string Question12Content = "Question12的Content";
    public string Question13Content = "Question13的Content";
    public string Question14Content = "Question14的Content";
    public string Question21Content = "Question21的Content";
    public string Question22Content = "Question22的Content";
    public string Question23Content = "Question23的Content";
    public string Question24Content = "Question24的Content";

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
    public Guid Answer131Id = Guid.NewGuid();
    public string Answer131Content = "Answer1的Content";
    public Guid Answer132Id = Guid.NewGuid();
    public string Answer132Content = "Answer2的Content";
    public Guid Answer141Id = Guid.NewGuid();
    public string Answer141Content = "Answer1的Content";

    public Guid Answer211Id = Guid.NewGuid();
    public string Answer211Content = "Answer1的Content";
    public Guid Answer212Id = Guid.NewGuid();
    public string Answer212Content = "Answer2的Content";
    public Guid Answer221Id = Guid.NewGuid();
    public string Answer221Content = "Answer1的Content";
    public Guid Answer222Id = Guid.NewGuid();
    public string Answer222Content = "Answer2的Content";
    public Guid Answer223Id = Guid.NewGuid();
    public string Answer223Content = "Answer3的Content";
    public Guid Answer224Id = Guid.NewGuid();
    public string Answer224Content = "Answer4的Content";
    public Guid Answer231Id = Guid.NewGuid();
    public string Answer231Content = "Answer1的Content";
    public Guid Answer232Id = Guid.NewGuid();
    public string Answer232Content = "Answer2的Content";
    public Guid Answer241Id = Guid.NewGuid();
    public string Answer241Content = "Answer1的Content";

    public Guid KnowledgePoint1Id = Guid.NewGuid();
    public string KnowledgePoint1Name = "KnowledgePoint1的Name";
    public Guid KnowledgePoint11Id = Guid.NewGuid();
    public string KnowledgePoint11Name = "KnowledgePoint11的Name";
    public Guid KnowledgePoint2Id = Guid.NewGuid();
    public string KnowledgePoint2Name = "KnowledgePoint2的Name";

    public Guid QuestionBank1Id = Guid.NewGuid();
    public Guid QuestionBank2Id = Guid.NewGuid();
    public string QuestionBank1Title = "Question Bank1的Title";
    public string QuestionBank2Title = "Question Bank2的Title";

    public Guid Paper1Id = Guid.NewGuid();
    public string Paper1Name = "Paper1的Name";

    public Guid Paper2Id = Guid.NewGuid();
    public string Paper2Name = "Paper2的Name";

    public Guid Examination11Id = Guid.NewGuid();
    public string Examination11Name = "Examination11的Name";

    public Guid Examination12Id = Guid.NewGuid();
    public string Examination12Name = "Examination12的Name";

    public Guid Examination13Id = Guid.NewGuid();
    public string Examination13Name = "Examination12的Name";

    public Guid Examination14Id = Guid.NewGuid();
    public string Examination14Name = "Examination12的Name";

    public Guid Examination15Id = Guid.NewGuid();
    public string Examination15Name = "Examination12的Name";

    public Guid Examination21Id = Guid.NewGuid();
    public string Examination21Name = "Examination21的Name";

    public Guid Examination22Id = Guid.NewGuid();
    public string Examination22Name = "Examination22的Name";

    public Guid Examination31Id = Guid.NewGuid();
    public string Examination31Name = "Examination31的Name";

    public Guid PaperQuestionRule1Id = Guid.NewGuid();

    public Guid PaperQuestionRule2Id = Guid.NewGuid();

    public Guid UserExam11Id = Guid.NewGuid();
    public Guid UserExam12Id = Guid.NewGuid();
    public Guid UserExam21Id = Guid.NewGuid();
    public Guid UserExam22Id = Guid.NewGuid();
    public Guid UserExam31Id = Guid.NewGuid();

    public Guid UserExamQuestion111Id = Guid.NewGuid();
    public Guid UserExamQuestion112Id = Guid.NewGuid();
    public Guid UserExamQuestion113Id = Guid.NewGuid();
    public Guid UserExamQuestion114Id = Guid.NewGuid();
    public Guid UserExamQuestion121Id = Guid.NewGuid();
    public Guid UserExamQuestion122Id = Guid.NewGuid();
    public Guid UserExamQuestion123Id = Guid.NewGuid();
    public Guid UserExamQuestion124Id = Guid.NewGuid();
    public Guid UserExamQuestion211Id = Guid.NewGuid();
    public Guid UserExamQuestion212Id = Guid.NewGuid();
    public Guid UserExamQuestion213Id = Guid.NewGuid();
    public Guid UserExamQuestion214Id = Guid.NewGuid();
    public Guid UserExamQuestion221Id = Guid.NewGuid();
    public Guid UserExamQuestion222Id = Guid.NewGuid();
    public Guid UserExamQuestion223Id = Guid.NewGuid();
    public Guid UserExamQuestion224Id = Guid.NewGuid();

    public Guid Training1Id = Guid.NewGuid();
    public Guid Training2Id = Guid.NewGuid();
}