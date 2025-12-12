import { CoreModule, ListService, PagedResultDto } from '@abp/ng.core';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { NgxDatatableDefaultDirective, NgxDatatableListDirective } from '@abp/ng.theme.shared';
import { Component, inject, OnInit } from '@angular/core';
import { FavoriteService, OptionService } from '@proxy/controllers';
import { Router } from '@angular/router';
import { FavoriteListDto } from '@proxy/favorites';
import { DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-my-favorite',
  templateUrl: './my-favorite.component.html',
  styleUrls: ['./my-favorite.component.scss'],
  providers: [ListService],
  imports: [
    CoreModule,
    DatePipe,
    FormsModule,
    NgxDatatableModule,
    NgxDatatableDefaultDirective,
    NgxDatatableListDirective,
  ],
})
export class MyFavoriteComponent implements OnInit {
  favorites = { items: [], totalCount: 0 } as PagedResultDto<FavoriteListDto>;
  public readonly list = inject(ListService);
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
        label: '::QuestionType_' + value,
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

  goDetail(id: string) {
    this.router.navigate([`/question-banks/train/${id}`]);
  }
}
