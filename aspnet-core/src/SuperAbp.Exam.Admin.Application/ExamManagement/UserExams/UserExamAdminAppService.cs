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
using SuperAbp.Exam.QuestionManagement.QuestionAnswers;
using Volo.Abp.Identity;
using Volo.Abp.Users;

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

    public virtual async Task<PagedResultDto<UserExamWithUserDto>> GetListWithUserAsync(GetUserExamWithUsersInput input)
    {
        int totalCount = await UserExamRepository.GetCountAsync(examId: input.ExamId);

        List<UserExamWithUser> userExams = await UserExamRepository.GetListByExamIdAsync(input.ExamId,
            input.Sorting ?? UserExamConsts.DefaultSorting, input.SkipCount, input.MaxResultCount);
        List<UserExamWithUserDto> dtos = [];
        List<IdentityUser> users = await UserRepository.GetListByIdsAsync(userExams.Select(e => e.UserId));
        foreach (UserExamWithUser userExam in userExams)
        {
            UserExamWithUserDto dto = ObjectMapper.Map<UserExamWithUser, UserExamWithUserDto>(userExam);
            IdentityUser? user = users.SingleOrDefault(u => u.Id == userExam.UserId);
            if (user is not null)
            {
                dto.User = user.UserName;
            }
            dtos.Add(dto);
        }
        return new PagedResultDto<UserExamWithUserDto>(totalCount, dtos);
    }

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
            throw new UnfinishedException();
        }
        Examination examination = await ExamRepository.GetAsync(userExam.ExamId);
        IdentityUser user = await UserRepository.GetAsync(userExam.UserId);
        List<Guid> questionIds = userExam.Questions.Select(q => q.QuestionId).ToList();
        List<Question> questions = await QuestionRepository.GetByIdsAsync(questionIds);
        UserExamDetailDto dto = ObjectMapper.Map<UserExam, UserExamDetailDto>(userExam);
        dto.ExamName = examination.Name;
        dto.UserName = user.UserName;
        dto.Status = userExam.Status;
        List<UserExamDetailDto.QuestionDto> questionDtos = [];
        foreach (Question question in questions)
        {
            var questionDto = ObjectMapper.Map<Question, UserExamDetailDto.QuestionDto>(question);
            UserExamQuestion userExamQuestion = userExam.Questions.Single(q => q.QuestionId == question.Id);
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
            List<UserExamDetailDto.QuestionDto.OptionDto> answerDtos = [];
            foreach (QuestionAnswer answer in question.Answers)
            {
                UserExamDetailDto.QuestionDto.OptionDto optionDto = new()
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
        dto.Questions = questionDtos;
        return dto;
    }

    public async Task ReviewQuestionsAsync(Guid id, List<ReviewedQuestionDto> input)
    {
        UserExam userExam = await UserExamRepository.GetAsync(id);
        if (userExam.Status != UserExamStatus.Submitted)
        {
            throw new InvalidUserExamStatusException(userExam.Status);
        }
        Examination examination = await ExamRepository.GetAsync(userExam.ExamId);
        if (examination.Status != ExaminationStatus.Grading)
        {
            throw new InvalidExamStatusException(examination.Status);
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
        await UserExamRepository.UpdateAsync(userExam);
    }
}