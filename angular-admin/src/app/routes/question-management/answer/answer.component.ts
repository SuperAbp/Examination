import { ConfigStateService, LocalizationService, PermissionService } from '@abp/ng.core';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { STChange, STColumn, STComponent, STData, STPage } from '@delon/abc/st';
import { SFSchema } from '@delon/form';
import { ModalHelper } from '@delon/theme';
import { QuestionAnswerService } from '@proxy/super-abp/exam/admin/controllers';
import { GetQuestionAnswersInput, QuestionAnswerListDto } from '@proxy/super-abp/exam/admin/question-management/question-answers';
import { NzMessageService } from 'ng-zorro-antd/message';
import { tap } from 'rxjs/operators';

@Component({
  selector: 'app-question-management-answer',
  templateUrl: './answer.component.html'
})
export class QuestionManagementAnswerComponent implements OnInit {
  @Input()
  questionId: string;

  answers: QuestionAnswerListDto[];
  loading = false;
  params: GetQuestionAnswersInput;
  page: STPage = {
    front: false,
    show: false
  };
  @ViewChild('st', { static: false }) st: STComponent;
  columns: STColumn[] = [
    { title: this.localizationService.instant('Exam::Right'), index: 'right' },
    { title: this.localizationService.instant('Exam::Content'), index: 'content', render: 'contentTpl' },
    { title: this.localizationService.instant('Exam::Analysis'), index: 'analysis' },
    {
      title: this.localizationService.instant('Exam::Actions'),
      buttons: [
        {
          icon: 'edit',
          iif: i => !i.edit && this.permissionService.getGrantedPolicy('Exam.QuestionAnswer.Update'),
          click: i => this.updateEdit(i, true),
          type: 'modal',
          tooltip: this.localizationService.instant('Exam::Edit')
        },
        {
          icon: 'delete',
          type: 'del',
          tooltip: this.localizationService.instant('Exam::Delete'),
          pop: {
            title: this.localizationService.instant('Exam::AreYouSure'),
            okType: 'danger',
            icon: 'star'
          },
          iif: () => {
            return this.permissionService.getGrantedPolicy('Exam.QuestionAnswer.Delete');
          },
          click: (record, _modal, component) => {
            this.answerService.delete(record.id).subscribe(response => {
              this.messageService.success(this.localizationService.instant('Exam::SuccessDeleted', record.name));
              // tslint:disable-next-line: no-non-null-assertion
              component!.removeRow(record);
            });
          }
        }
      ]
    }
  ];

  constructor(
    private modal: ModalHelper,
    private localizationService: LocalizationService,
    private messageService: NzMessageService,
    private permissionService: PermissionService,
    private answerService: QuestionAnswerService
  ) {}

  ngOnInit() {
    this.params = this.resetParameters();
    this.getList();
  }
  getList() {
    this.loading = true;
    this.answerService
      .getList(this.params)
      .pipe(tap(() => (this.loading = false)))
      .subscribe(response => (this.answers = response.items));
  }
  resetParameters(): GetQuestionAnswersInput {
    return {
      skipCount: 0,
      maxResultCount: 10,
      sorting: 'Id Desc',
      questionId: this.questionId
    };
  }

  private submit(i: STData): void {
    // this.msg.success(JSON.stringify(this.st.pureItem(i)));
    this.updateEdit(i, false);
  }

  private updateEdit(i: STData, edit: boolean): void {
    this.st.setRow(i, { edit }, { refreshSchema: true });
  }
}
