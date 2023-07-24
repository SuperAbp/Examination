import { LocalizationService, PermissionService } from '@abp/ng.core';
import { Component, OnInit, ViewChild } from '@angular/core';
import { STChange, STColumn, STComponent, STData, STPage } from '@delon/abc/st';
import { SFSchema, SFSchemaEnumType, SFSelectWidgetSchema, SFStringWidgetSchema } from '@delon/form';
import { ModalHelper } from '@delon/theme';
import { QuestionManagementQuestionEditComponent } from './edit/edit.component';
import { NzMessageService } from 'ng-zorro-antd/message';
import { map, tap } from 'rxjs/operators';
import { GetQuestionsInput, QuestionListDto } from '@proxy/super-abp/exam/admin/question-management/questions';
import { EnumService, QuestionRepoService, QuestionService } from '@proxy/super-abp/exam/admin/controllers';
import { Router } from '@angular/router';
import { QuestionType } from '@proxy/super-abp/exam/question-management/questions';
import { of } from 'rxjs';

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
      content: {
        type: 'string',
        title: '',
        ui: {
          placeholder: this.localizationService.instant('Exam::Placeholder', this.localizationService.instant('Exam::Title'))
        } as SFStringWidgetSchema
      },
      questionType: {
        type: 'string',
        title: '',
        ui: {
          placeholder: this.localizationService.instant('Exam::ChoosePlaceholder', this.localizationService.instant('Exam::QuestionType')),
          widget: 'select',
          width: 250,
          allowClear: true,
          asyncData: () =>
            of(
              Object.keys(QuestionType)
                .filter(k => !isNaN(Number(k)))
                .map(key => {
                  return { label: this.localizationService.instant('Exam::QuestionType:' + key), value: +key };
                })
            ).pipe()
        } as SFSelectWidgetSchema
      },
      repositoryId: {
        type: 'string',
        title: '',
        ui: {
          placeholder: this.localizationService.instant(
            'Exam::ChoosePlaceholder',
            this.localizationService.instant('Exam::QuestionRepository')
          ),
          widget: 'select',
          width: 250,
          allowClear: true,
          asyncData: () =>
            this.questionRepositoryService.getList({ skipCount: 0, maxResultCount: 100 }).pipe(
              map((res: any) => {
                const temp: SFSchemaEnumType[] = [];
                res.items.forEach(item => {
                  temp.push({ label: item.title, value: item.id });
                });
                return temp;
              })
            )
        } as SFSelectWidgetSchema
      }
    }
  };
  @ViewChild('st', { static: false }) st: STComponent;
  columns: STColumn[] = [
    { title: this.localizationService.instant('Exam::QuestionRepository'), index: 'questionRepository' },
    {
      title: this.localizationService.instant('Exam::QuestionType'),
      render: 'questionType'
    },
    { title: this.localizationService.instant('Exam::QuestionContent'), index: 'content' },
    { title: this.localizationService.instant('Exam::CreationTime'), index: 'creationTime', type: 'date' },
    {
      title: this.localizationService.instant('Exam::Actions'),
      buttons: [
        {
          icon: 'edit',
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
    private questionService: QuestionService,
    private questionRepositoryService: QuestionRepoService
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
      sorting: 'Id Desc',
      questionRepositoryIds: []
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
    if (e.repositoryId) {
      this.params.questionRepositoryIds = [e.repositoryId];
    } else {
      delete this.params.questionRepositoryIds;
    }
    if (e.content) {
      this.params.content = e.content;
    } else {
      delete this.params.content;
    }
    if (e.questionType > -1) {
      this.params.questionType = e.questionType;
    } else {
      delete this.params.questionType;
    }
    this.st.load(1);
  }
  add() {
    this.router.navigateByUrl('/question-management/question/create');
  }
}
