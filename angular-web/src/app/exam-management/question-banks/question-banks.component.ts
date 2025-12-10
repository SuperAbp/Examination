import { CoreModule, ListService, PagedResultDto } from '@abp/ng.core';
import { Component, inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { NgxDatatableDefaultDirective, NgxDatatableListDirective } from '@abp/ng.theme.shared';
import { QuestionBankService } from '@proxy/controllers';
import { QuestionBankListDto } from '@proxy/question-management/question-banks';

@Component({
  selector: 'app-question-banks',
  templateUrl: './question-banks.component.html',
  providers: [ListService],
  imports: [
    CoreModule,
    NgxDatatableModule,
    NgxDatatableDefaultDirective,
    NgxDatatableListDirective,
  ],
})
export class QuestionBanksComponent implements OnInit {
  questionBanks = { items: [], totalCount: 0 } as PagedResultDto<QuestionBankListDto>;
  public readonly list = inject(ListService);
  private readonly questionBankService = inject(QuestionBankService);
  private readonly router = inject(Router);

  ngOnInit() {
    const questionBankStreamCreator = query => this.questionBankService.getList(query);
    this.list.hookToQuery(questionBankStreamCreator).subscribe(response => {
      this.questionBanks = response;
    });
  }

  goDetail(id: string) {
    this.router.navigate([`/question-banks/${id}`]);
  }
}
