using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using SuperAbp.Exam.ExamManagement.UserExams;
using SuperAbp.Exam.KnowledgePoints;
using SuperAbp.Exam.QuestionManagement.Questions;
using System.Linq;
using SuperAbp.Exam.ExamManagement.Exams;
using SuperAbp.Exam.ExamManagement.UserExamQuestions;
using Volo.Abp.Identity;
using Volo.Abp.Users;
using Volo.Abp;
using SuperAbp.Exam.QuestionManagement.Questions.QuestionOptions;
using SuperAbp.Exam.Admin.ExamManagement.Exams;

namespace SuperAbp.Exam.Admin.ExamManagement.UserExams;

public class UserExamAdminAppService(IUserExamRepository userExamRepository,
    IQuestionRepository questionRepository,
    IIdentityUserRepository userRepository,
    IExamRepository examRepository,
    QuestionManager questionManager,
    UserExamManager userExamManager) : ExamAppService, IUserExamAdminAppService
{
    protected IQuestionRepository QuestionRepository { get; } = questionRepository;
    public IIdentityUserRepository UserRepository { get; } = userRepository;
    public IExamRepository ExamRepository { get; } = examRepository;
    protected QuestionManager QuestionManager { get; } = questionManager;
    public UserExamManager UserExamManager { get; } = userExamManager;
    protected IUserExamRepository UserExamRepository { get; } = userExamRepository;

    public async Task<ListResultDto<UserExamListDto>> GetListAsync(GetUserExamsInput input)
    {
        List<UserExam> userExams = await UserExamRepository.GetListAsync(examId: input.ExamId, userId: input.UserId);
        return new ListResultDto<UserExamListDto>(ObjectMapper.Map<List<UserExam>, List<UserExamListDto>>(userExams));
    }

    public async Task<UserExamDetailDto> GetAsync(Guid id)
    {
        UserExam userExam = await UserExamRepository.GetAsync(id);
        if (userExam.Status == UserExamStatus.InProgress)
        {
            throw new BusinessException(ExamDomainErrorCodes.UserExams.Unfinished);
        }
        Examination examination = await ExamRepository.GetAsync(userExam.ExamId);
        IdentityUser user = await UserRepository.GetAsync(userExam.UserId);
        List<Guid> questionIds = userExam.Sections.SelectMany(s => s.Questions).Select(q => q.QuestionId).ToList();
        List<Question> questions = await QuestionRepository.GetByIdsAsync(questionIds);
        UserExamDetailDto dto = ObjectMapper.Map<UserExam, UserExamDetailDto>(userExam);
        dto.ReviewMode = examination.ReviewMode.Value;
        dto.ExamStatus = examination.Status.Value;
        dto.ExamName = examination.Name;
        dto.UserName = user.UserName;
        dto.Status = userExam.Status;
        List<UserExamDetailDto.SectionDto> sectionDtos = [];
        foreach (UserExamSection section in userExam.Sections.OrderBy(s => s.Order))
        {
            var sectionDto = ObjectMapper.Map<UserExamSection, UserExamDetailDto.SectionDto>(section);
            List<UserExamDetailDto.SectionDto.QuestionDto> questionDtos = [];
            foreach (UserExamQuestion userExamQuestion in section.Questions.OrderBy(q => q.Order))
            {
                Question question = questions.Single(q => q.Id == userExamQuestion.QuestionId);
                var questionDto = ObjectMapper.Map<Question, UserExamDetailDto.SectionDto.QuestionDto>(question);
                questionDto.Right = userExamQuestion.Right;
                questionDto.Reason = userExamQuestion.Reason;
                questionDto.Score = userExamQuestion.Score;
                questionDto.Answers = userExamQuestion.Answers;
                questionDto.QuestionScore = userExamQuestion.QuestionScore;
                List<KnowledgePoint> knowledgePoints = await QuestionManager.GetKnowledgePointsAsync(question.Id);
                if (knowledgePoints.Count > 0)
                {
                    questionDto.KnowledgePoints = knowledgePoints.Select(kp => kp.Name).ToArray();
                }
                List<UserExamDetailDto.SectionDto.QuestionDto.OptionDto> answerDtos = [];
                foreach (QuestionOption answer in question.Options)
                {
                    UserExamDetailDto.SectionDto.QuestionDto.OptionDto optionDto = new()
                    {
                        Id = answer.Id,
                        Content = answer.Content,
                    };
                    if (userExam.IsSubmitted())
                    {
                        optionDto.Right = answer.Right;
                    }
                    answerDtos.Add(optionDto);
                }
                questionDto.Options = answerDtos;
                questionDtos.Add(questionDto);
            }
            sectionDto.Questions = questionDtos;
            sectionDtos.Add(sectionDto);
        }
        dto.Sections = sectionDtos;
        return dto;
    }

    public async Task ReviewQuestionsAsync(Guid id, List<ReviewedQuestionDto> input)
    {
        UserExam userExam = await UserExamRepository.GetAsync(id);
        Examination examination = await ExamRepository.GetAsync(userExam.ExamId);

        if (examination.ReviewMode == ReviewMode.Unified)
        {
            if (examination.Status != ExaminationStatus.Grading)
            {
                throw new InvalidExamStatusException(examination.Status);
            }
        }
        else if (examination.ReviewMode == ReviewMode.RealTime)
        {
            if (examination.Status != ExaminationStatus.Published && examination.Status != ExaminationStatus.Grading)
            {
                throw new InvalidExamStatusException(examination.Status);
            }
        }

        foreach (ReviewedQuestionDto question in input)
        {
            if (!question.Score.HasValue)
            {
                continue;
            }
            userExam.ReviewQuestion(GuidGenerator.Create(), question.QuestionId, question.Right, question.Score.Value, question.Reason);
        }
        userExam.UpdateTotalScore();
        userExam.CheckPassed(examination.PassingScore);
        userExam.Score();
        await UserExamRepository.UpdateAsync(userExam);
    }
}