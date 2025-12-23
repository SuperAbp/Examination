import { CoreModule, ListService, PagedResultDto } from '@abp/ng.core';
import { Component, inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { NgxDatatableDefaultDirective, NgxDatatableListDirective } from '@abp/ng.theme.shared';
import { FormsModule } from '@angular/forms';
import { QuestionBankService } from '@proxy/controllers';
import {
  QuestionBankListDto,
  GetQuestionBanksInput,
} from '@proxy/question-management/question-banks';

@Component({
  selector: 'app-question-banks',
  templateUrl: './question-banks.component.html',
  providers: [ListService],
  imports: [
    CoreModule,
    FormsModule,
    NgxDatatableModule,
    NgxDatatableDefaultDirective,
    NgxDatatableListDirective,
  ],
})
export class QuestionBanksComponent implements OnInit {
  questionBanks = { items: [], totalCount: 0 } as PagedResultDto<QuestionBankListDto>;
  protected readonly list = inject(ListService<GetQuestionBanksInput>);
  private readonly questionBankService = inject(QuestionBankService);
  private readonly router = inject(Router);

  filters: Partial<GetQuestionBanksInput> = {
    title: undefined,
  };

  ngOnInit() {
    this.list
      .hookToQuery(query => {
        query.title = this.filters.title;
        return this.questionBankService.getList(query);
      })
      .subscribe(response => {
        this.questionBanks = response;
      });
  }

  search() {
    this.list.get();
  }

  clearFilters() {
    this.filters = {
      title: undefined,
    };
    this.search();
  }

  goDetail(id: string) {
    this.router.navigate([`/question-banks/${id}`]);
  }
}
