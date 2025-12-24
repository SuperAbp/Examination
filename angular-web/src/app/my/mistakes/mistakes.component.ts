import { CoreModule, ListService, LocalizationService, PagedResultDto } from '@abp/ng.core';
import { NgxDatatableDefaultDirective, NgxDatatableListDirective } from '@abp/ng.theme.shared';
import { Component, inject, OnInit } from '@angular/core';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { Router } from '@angular/router';
import { DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgSelectComponent } from '@ng-select/ng-select';
import { MistakeService, OptionService } from '@proxy/controllers';
import { MistakeListDto, GetMistakesInput } from '@proxy/mistakes';

@Component({
  selector: 'app-mistakes',
  templateUrl: './mistakes.component.html',
  styleUrls: ['./mistakes.component.scss'],
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
export class MistakesComponent implements OnInit {
  mistakes = { items: [], totalCount: 0 } as PagedResultDto<MistakeListDto>;
  protected readonly list = inject(ListService<GetMistakesInput>);
  private localizationService = inject(LocalizationService);
  private readonly mistakeService = inject(MistakeService);
  private readonly optionService = inject(OptionService);
  private readonly router = inject(Router);

  filters = {
    questionContent: null,
    questionType: null,
  };

  questionTypes: Array<{ value: number; label: string }> = [];

  ngOnInit() {
    this.loadQuestionTypes();

    this.list
      .hookToQuery(query => {
        query.questionContent = this.filters.questionContent || undefined;
        query.questionType = this.filters.questionType ?? undefined;
        return this.mistakeService.getList(query);
      })
      .subscribe(response => {
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

    this.router.navigate(['/my/mistakes/train'], { queryParams });
  }
}
