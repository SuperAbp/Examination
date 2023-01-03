import { LocalizationService, PermissionService } from '@abp/ng.core';
import { Component, OnInit, ViewChild } from '@angular/core';
import { STChange, STColumn, STComponent, STData, STPage } from '@delon/abc/st';
import { SFSchema } from '@delon/form';
import { ModalHelper } from '@delon/theme';
import { QuestionManagementQuestionEditComponent } from './edit/edit.component';
import { NzMessageService } from 'ng-zorro-antd/message';
import { tap } from 'rxjs/operators';
import { GetQuestionsInput, QuestionListDto } from '@proxy/super-abp/exam/admin/question-management/questions';
import { QuestionService } from '@proxy/super-abp/exam/admin/controllers';
import { Router } from '@angular/router';

@Component({
  selector: 'app-question-management-question',
  templateUrl: './question.component.html'
})
export class QuestionManagementQuestionComponent implements OnInit {
  questions: QuestionListDto[];
  total: number;
  loading = false;
  params: GetQuestionsInput;
  page: STPage = {
    show: true,
    showSize: true,
    front: false,
    pageSizes: [10, 20, 30, 40, 50]
  };
  searchSchema: SFSchema = {
    properties: {
      // TODO:搜索参数
    }
  };
  @ViewChild('st', { static: false }) st: STComponent;
  columns: STColumn[] = [
    { title: this.localizationService.instant('Exam::QuestionRepository'), index: 'questionRepository' },
    {
      title: this.localizationService.instant('Exam::QuestionType'),
      index: 'questionType',
      type: 'tag',
      tag: {
        0: { text: this.localizationService.instant('Exam::QuestionType:0'), color: 'default' },
        1: { text: this.localizationService.instant('Exam::QuestionType:1'), color: 'processing' },
        2: { text: this.localizationService.instant('Exam::QuestionType:2'), color: 'success' }
      }
    },
    { title: this.localizationService.instant('Exam::QuestionContent'), index: 'content' },
    { title: this.localizationService.instant('Exam::CreationTime'), index: 'creationTime' },
    {
      title: this.localizationService.instant('Exam::Actions'),
      buttons: [
        {
          icon: 'edit',
          type: 'modal',
          tooltip: this.localizationService.instant('Exam::Edit'),
          iif: () => {
            return this.permissionService.getGrantedPolicy('Exam.Question.Update');
          },
          click: (record: STData, modal?: any, instance?: STComponent) => {
            this.router.navigateByUrl(`/question-management/question/${record['id']}/edit`);
          }
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
            return this.permissionService.getGrantedPolicy('Exam.Question.Delete');
          },
          click: (record, _modal, component) => {
            this.questionService.delete(record.id).subscribe(response => {
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
    private router: Router,
    private localizationService: LocalizationService,
    private messageService: NzMessageService,
    private permissionService: PermissionService,
    private questionService: QuestionService
  ) {}

  ngOnInit() {
    this.params = this.resetParameters();
    this.getList();
  }
  getList() {
    this.loading = true;
    this.questionService
      .getList(this.params)
      .pipe(tap(() => (this.loading = false)))
      .subscribe(response => ((this.questions = response.items), (this.total = response.totalCount)));
  }
  resetParameters(): GetQuestionsInput {
    return {
      skipCount: 0,
      maxResultCount: 10,
      sorting: 'Id Desc'
    };
  }
  change(e: STChange) {
    if (e.type === 'pi' || e.type === 'ps') {
      this.params.skipCount = (e.pi - 1) * e.ps;
      this.params.maxResultCount = e.ps;
      this.getList();
    } else if (e.type === 'sort') {
      this.params.sorting = `${e.sort?.column?.index as string} ${e.sort.value === 'ascend' ? 'asc' : 'desc'}`;
      this.getList();
    }
  }
  reset() {
    this.params = this.resetParameters();
    this.st.load(1);
  }
  search(e) {
    //if (e.name) {
    //  this.params.name = e.name;
    //} else {
    //  delete this.params.name;
    //}
    this.st.load(1);
  }
  add() {
    this.router.navigateByUrl('/question-management/question/create');
  }
}
