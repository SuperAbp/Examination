using System.Linq;
using FluentValidation;
using Microsoft.Extensions.Localization;
using SuperAbp.Exam.Localization;
using SuperAbp.Exam.PaperManagement.Papers;
using SuperAbp.Exam.PaperManagement.PaperSections;
using SuperAbp.Exam.QuestionManagement.Questions;

namespace SuperAbp.Exam.Admin.PaperManagement.Papers;

public class PaperCreateOrUpdateDtoBaseValidator : AbstractValidator<PaperCreateOrUpdateDtoBase>
{
    public PaperCreateOrUpdateDtoBaseValidator(IStringLocalizer<ExamResource> local)
    {
        RuleFor(q => q.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage(local["The {0} field is required.", "{PropertyName}"])
            .MaximumLength(PaperConsts.MaxNameLength)
            .WithMessage(local["The {0} field must not exceed {1} characters.", "{PropertyName}", "256"]);

        RuleFor(q => q.PaperType)
            .Must(type => PaperType.TryFromValue(type, out _))
            .WithMessage(local["The field {0} is invalid.", "{PropertyName}"]);

        RuleFor(q => q.Score)
            .GreaterThan(0)
            .WithMessage(local["The field {0} must be greater than 0.", "{PropertyName}"])
            .Must((a, score) => score == a.Sections.Sum(r => r.TotalScore))
            .WithMessage(local["The field {0} does not match the sum of section scores.", "{PropertyName}"]);

        RuleFor(q => q.Sections)
            .NotNull()
            .NotEmpty()
            .WithMessage(local["The {0} field is required.", "{PropertyName}"]);

        RuleForEach(q => q.Sections)
            .SetValidator((dto) => new PaperSectionDtoValidator(local, dto.PaperType));
    }
}

public class PaperSectionDtoValidator : AbstractValidator<PaperCreateOrUpdateDtoBase.PaperSectionDto>
{
    private readonly int _paperType;

    public PaperSectionDtoValidator(IStringLocalizer<ExamResource> local, int paperType = 0)
    {
        _paperType = paperType;
        RuleFor(s => s.Title)
            .NotNull()
            .NotEmpty()
            .WithMessage(local["The {0} field is required.", "{PropertyName}"])
            .MaximumLength(PaperSectionConsts.MaxTitleLength)
            .WithMessage(local["The {0} field must not exceed {1} characters.", "{PropertyName}", "256"]);

        RuleFor(s => s.TotalScore)
            .GreaterThan(0)
            .WithMessage(local["The {0} field must be greater than 0.", "{PropertyName}"]);

        RuleFor(s => s.TotalCount)
            .GreaterThan(0)
            .WithMessage(local["The {0} field must be greater than 0.", "{PropertyName}"]);

        RuleFor(s => s.ScoreEach)
            .GreaterThan(0)
            .WithMessage(local["The {0} field must be greater than 0.", "{PropertyName}"])
            .Must((section, scoreEach) => scoreEach * section.TotalCount == section.TotalScore)
            .WithMessage(local["The field {0} multiplied by TotalCount does not equal TotalScore.", "{PropertyName}"]);

        RuleFor(s => s.Order)
            .GreaterThanOrEqualTo(0)
            .WithMessage(local["The {0} field must be greater than or equal to 0.", "{PropertyName}"]);

        RuleFor(s => s.PaperQuestions)
            .NotNull()
            .WithMessage(local["The {0} field is required.", "{PropertyName}"])
            .NotEmpty()
            .WithMessage(local["The {0} field is required.", "{PropertyName}"])
            .Must((section, questions) => section.PaperQuestions.Count == section.TotalCount)
            .WithMessage(local["The number of {0} must match TotalCount.", "{PropertyName}"])
            .When(s => _paperType == PaperType.Fixed.Value);

        RuleForEach(s => s.PaperQuestions)
            .SetValidator(new PaperQuestionDtoValidator(local))
            .When(s => s.PaperQuestions != null && s.PaperQuestions.Count > 0 && _paperType == PaperType.Fixed.Value);

        RuleFor(s => s.PaperQuestionRules)
            .NotNull()
            .WithMessage(local["The {0} field is required.", "{PropertyName}"])
            .NotEmpty()
            .WithMessage(local["The {0} field is required.", "{PropertyName}"])
            .When(s => _paperType != PaperType.Fixed.Value);

        RuleForEach(s => s.PaperQuestionRules)
            .SetValidator(new PaperQuestionRuleDtoValidator(local))
            .When(s => s.PaperQuestionRules != null && s.PaperQuestionRules.Count > 0 && _paperType != PaperType.Fixed.Value);
    }
}

public class PaperQuestionDtoValidator : AbstractValidator<PaperCreateOrUpdateDtoBase.PaperSectionDto.PaperQuestionDto>
{
    public PaperQuestionDtoValidator(IStringLocalizer<ExamResource> local)
    {
        RuleFor(q => q.QuestionId)
            .NotEmpty()
            .WithMessage(local["The {0} field is required.", "{PropertyName}"]);

        RuleFor(q => q.Score)
            .GreaterThan(0)
            .WithMessage(local["The {0} field must be greater than 0.", "{PropertyName}"]);

        RuleFor(q => q.Order)
            .GreaterThanOrEqualTo(0)
            .WithMessage(local["The {0} field must be greater than or equal to 0.", "{PropertyName}"]);
    }
}

public class PaperQuestionRuleDtoValidator : AbstractValidator<PaperCreateOrUpdateDtoBase.PaperSectionDto.PaperQuestionRuleDto>
{
    public PaperQuestionRuleDtoValidator(IStringLocalizer<ExamResource> local)
    {
        RuleFor(r => r.QuestionBankId)
            .NotEmpty()
            .WithMessage(local["The {0} field is required.", "{PropertyName}"]);

        RuleFor(r => r.QuestionType)
            .Must(type => QuestionType.TryFromValue(type, out _))
            .WithMessage(local["The field {0} is invalid.", "{PropertyName}"]);

        RuleFor(r => r.Count)
            .GreaterThan(0)
            .WithMessage(local["The {0} field must be greater than 0.", "{PropertyName}"]);

        RuleFor(r => r.Score)
            .GreaterThan(0)
            .WithMessage(local["The {0} field must be greater than 0.", "{PropertyName}"]);
    }
}