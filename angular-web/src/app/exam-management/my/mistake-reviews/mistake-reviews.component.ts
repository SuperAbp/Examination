import { CoreModule, ListService, LocalizationService, PagedResultDto } from '@abp/ng.core';
import { NgxDatatableDefaultDirective, NgxDatatableListDirective } from '@abp/ng.theme.shared';
import { Component, inject, OnInit } from '@angular/core';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { Router } from '@angular/router';
import { DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgSelectComponent } from '@ng-select/ng-select';
import { MistakesReviewService, OptionService } from '@proxy/controllers';
import { MistakesReviewListDto } from '@proxy/mistakes-reviews';

@Component({
  selector: 'app-mistake-reviews',
  templateUrl: './mistake-reviews.component.html',
  styleUrls: ['./mistake-reviews.component.scss'],
  providers: [ListService],
  imports: [
    CoreModule,
    DatePipe,
    FormsModule,
    NgSelectComponent,
    NgxDatatableModule,
    NgxDatatableDefaultDirective,
    NgxDatatableListDirective,
  ],
})
export class MistakeReviewsComponent implements OnInit {
  mistakes = { items: [], totalCount: 0 } as PagedResultDto<MistakesReviewListDto>;
  public readonly list = inject(ListService);
  private localizationService = inject(LocalizationService);
  private readonly mistakesReviewService = inject(MistakesReviewService);
  private readonly optionService = inject(OptionService);
  private readonly router = inject(Router);

  filters = {
    questionContent: null,
    questionType: null,
  };

  questionTypes: Array<{ value: number; label: string }> = [];

  ngOnInit() {
    this.loadQuestionTypes();

    const mistakeStreamCreator = query => {
      const params = {
        ...query,
        questionContent: this.filters.questionContent || undefined,
        questionType: this.filters.questionType ?? undefined,
        errorCount: 1, // 至少错误1次
      };
      return this.mistakesReviewService.getList(params);
    };
    this.list.hookToQuery(mistakeStreamCreator).subscribe(response => {
      this.mistakes = response;
    });
  }

  loadQuestionTypes() {
    this.optionService.getQuestionTypes().subscribe(types => {
      this.questionTypes = Object.entries(types).map(([value, label]) => ({
        value: Number(value),
        label: this.localizationService.instant('::QuestionType:' + value),
      }));
    });
  }

  search() {
    this.list.get();
  }

  clearFilters() {
    this.filters = {
      questionContent: null,
      questionType: null,
    };
    this.list.get();
  }

  startTraining(questionId?: string) {
    const queryParams: any = {
      mode: 0,
    };

    if (questionId) {
      queryParams.questionId = questionId;
    } else {
      if (this.filters.questionContent) {
        queryParams.questionContent = this.filters.questionContent;
      }
      if (this.filters.questionType !== null && this.filters.questionType !== undefined) {
        queryParams.type = this.filters.questionType;
      }
    }

    this.router.navigate(['/my/mistakes-reviews/train'], { queryParams });
  }
}
