using AntDesign;
using Masuit.Tools;
using Microsoft.AspNetCore.Components;
using SuperAbp.Exam.Admin.PaperManagement.Papers;
using SuperAbp.Exam.Admin.QuestionManagement.Questions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using static SuperAbp.Exam.Admin.PaperManagement.Papers.PaperCreateOrUpdateDtoBase.PaperSectionDto;

namespace SuperAbp.Exam.Admin.Blazor.Client.Pages.Papers;

public partial class PaperFixComponent
{
    protected Dictionary<Guid, QuestionDetailDto> Questions { get; set; } = [];
    protected QuestionSelectModal QuestionSelectModal { get; set; }
    protected PaperCreateOrUpdateDtoBase.PaperSectionDto CurrentSection { get; set; }

    [CascadingParameter]
    protected PaperCreateDto Paper { get; set; }

    [Inject]
    protected IQuestionAdminAppService QuestionAppService { get; set; }

    [Inject]
    protected ModalService ModalService { get; set; }

    protected override void OnInitialized()
    {
        AddSection();
    }

    protected virtual List<PaperQuestionDto> GetQuestions(PaperCreateOrUpdateDtoBase.PaperSectionDto section)
    {
        return section.PaperQuestions;
    }

    protected virtual void AddSection()
    {
        Paper.Sections.Add(new PaperCreateOrUpdateDtoBase.PaperSectionDto()
        {
            Title = $"第{(Paper.Sections.Count + 1).ToChineseNumber()}大题",
            ScoreEach = 0
        });
    }

    protected virtual async void UpdateScore(PaperCreateOrUpdateDtoBase.PaperSectionDto section)
    {
        section.PaperQuestions.ForEach(q => { q.Score = section.ScoreEach; });
    }

    protected virtual async Task SelectQuestion(PaperCreateOrUpdateDtoBase.PaperSectionDto section)
    {
        CurrentSection = section;

        var modalConfig = new ModalOptions();
        modalConfig.Title = L["SelectQuestions"];
        modalConfig.Width = 1200;
        modalConfig.Footer = ModalFooter.DefaultOkFooter;

        ModalRef<IEnumerable<Guid>> modalRef = ModalService.CreateModal<QuestionSelectModal, IEnumerable<Guid>, IEnumerable<Guid>>
            (modalConfig, Paper.Sections.SelectMany(s => s.PaperQuestions.Select(q => q.QuestionId)));
        modalRef.OnOk = LoadQuestionsAsync;
    }

    protected virtual async Task LoadQuestionsAsync(IEnumerable<Guid> selectedIds)
    {
        ListResultDto<QuestionDetailDto> dtos = await QuestionAppService
            .GetListWithDetailAsync(new GetQuestionWithDetailInput() { IncludeIds = selectedIds.ToList() });

        foreach (var dto in dtos.Items)
        {
            Questions[dto.Id] = dto;
            CurrentSection.PaperQuestions.Add(new PaperQuestionDto
            {
                QuestionId = dto.Id,
                Order = 0
            });
        }
        await InvokeAsync(StateHasChanged);
    }

    protected void RandomAdditionQuestion(PaperCreateOrUpdateDtoBase.PaperSectionDto section)
    {
    }
}