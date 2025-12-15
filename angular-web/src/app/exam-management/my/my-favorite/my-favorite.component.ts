import { CoreModule, ListService, LocalizationService, PagedResultDto } from '@abp/ng.core';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { NgxDatatableDefaultDirective, NgxDatatableListDirective } from '@abp/ng.theme.shared';
import { Component, inject, OnInit } from '@angular/core';
import { FavoriteService, OptionService } from '@proxy/controllers';
import { Router } from '@angular/router';
import { FavoriteListDto } from '@proxy/favorites';
import { DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgSelectComponent } from '@ng-select/ng-select';

@Component({
  selector: 'app-my-favorite',
  templateUrl: './my-favorite.component.html',
  styleUrls: ['./my-favorite.component.scss'],
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
export class MyFavoriteComponent implements OnInit {
  favorites = { items: [], totalCount: 0 } as PagedResultDto<FavoriteListDto>;
  public readonly list = inject(ListService);
  private localizationService = inject(LocalizationService);
  private readonly favoriteService = inject(FavoriteService);
  private readonly optionService = inject(OptionService);
  private readonly router = inject(Router);

  filters = {
    questionContent: null,
    questionType: null,
  };

  questionTypes: Array<{ value: number; label: string }> = [];

  ngOnInit() {
    this.loadQuestionTypes();

    const questionBankStreamCreator = query => {
      const params = {
        ...query,
        questionContent: this.filters.questionContent || undefined,
        questionType: this.filters.questionType ?? undefined,
      };
      return this.favoriteService.getList(params);
    };
    this.list.hookToQuery(questionBankStreamCreator).subscribe(response => {
      this.favorites = response;
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

    // 如果传入了 questionId，则只训练该题
    if (questionId) {
      queryParams.questionId = questionId;
    } else {
      // 否则传递当前搜索条件
      if (this.filters.questionContent) {
        queryParams.questionContent = this.filters.questionContent;
      }
      if (this.filters.questionType !== null && this.filters.questionType !== undefined) {
        queryParams.type = this.filters.questionType;
      }
    }

    this.router.navigate(['/my/favorites/train'], { queryParams });
  }
}
