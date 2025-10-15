import { CoreModule, LocalizationService } from '@abp/ng.core';
import { Component, EventEmitter, inject, Input, OnInit, Output, ViewChild } from '@angular/core';
import { STChange, STColumn, STComponent, STData, STModule, STPage } from '@delon/abc/st';
import { DelonFormModule, SFSchema, SFSchemaEnumType, SFSelectWidgetSchema, SFStringWidgetSchema } from '@delon/form';
import { OptionService, QuestionBankService, QuestionService } from '@proxy/admin/controllers';
import { GetQuestionsInput, QuestionListDto } from '@proxy/admin/question-management/questions';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzModalModule, NzModalRef } from 'ng-zorro-antd/modal';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { finalize, map, tap } from 'rxjs';

@Component({
  selector: 'app-question-select',
  templateUrl: './question-select.component.html',
  standalone: true,
  imports: [CoreModule, NzButtonModule, NzSpinModule, NzModalModule, STModule, DelonFormModule]
})
export class QuestionSelectComponent implements OnInit {
  @Input()
  questionIds: string[] = [];

  private localizationService = inject(LocalizationService);
  private questionBankService = inject(QuestionBankService);
  private questionService = inject(QuestionService);
  private optionService = inject(OptionService);
  private modal = inject(NzModalRef);

  questions: QuestionListDto[];
  selectedQuestionIds = [];
  total: number;
  loading = false;
  isConfirmLoading = false;
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
            this.optionService.getQuestionTypes().pipe(
              map(res => {
                const temp: SFSchemaEnumType[] = [];
                Object.keys(res).forEach(key => {
                  temp.push({ label: this.localizationService.instant(`Exam::QuestionType:${key}`), value: key });
                });
                return temp;
              })
            )
        } as SFSelectWidgetSchema
      },
      repositoryId: {
        type: 'string',
        title: '',
        ui: {
          placeholder: this.localizationService.instant('Exam::ChoosePlaceholder', this.localizationService.instant('Exam::QuestionBank')),
          widget: 'select',
          width: 250,
          allowClear: true,
          asyncData: () =>
            this.questionBankService.getList({ skipCount: 0, maxResultCount: 100 }).pipe(
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
    {
      title: '',
      index: 'id',
      type: 'checkbox'
    },
    { title: this.localizationService.instant('Exam::QuestionBank'), index: 'questionBank', width: 180 },
    {
      title: this.localizationService.instant('Exam::QuestionType'),
      render: 'questionType',
      width: 60
    },
    { title: this.localizationService.instant('Exam::QuestionContent'), index: 'content' },
    { title: this.localizationService.instant('Exam::KnowledgePoint'), index: 'knowledgePoints', width: 150 }
  ];
  ngOnInit() {
    this.params = this.resetParameters();
    this.getList();
  }
  getList() {
    this.loading = true;
    this.questionService
      .getList(this.params)
      .pipe(
        tap(response => {
          this.questions = response.items;
          this.total = response.totalCount;
        }),
        finalize(() => {
          this.loading = false;
        })
      )
      .subscribe();
  }
  resetParameters(): GetQuestionsInput {
    return {
      skipCount: 0,
      maxResultCount: 10,
      questionBankIds: [],
      excludeIds: this.questionIds
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
    } else if (e.type === 'checkbox') {
      this.selectedQuestionIds = this.selectedQuestionIds.filter(i => !this.questions.map(q => q.id).includes(i));
      e.checkbox.forEach(element => {
        if (this.selectedQuestionIds.includes(element.id)) {
          return;
        }
        this.selectedQuestionIds.push(element.id);
      });
    }
  }
  reset() {
    this.params = this.resetParameters();
    this.st.load(1);
  }
  search(e) {
    if (e.repositoryId) {
      this.params.questionBankIds = [e.repositoryId];
    } else {
      delete this.params.questionBankIds;
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
  save() {
    this.modal.close(this.selectedQuestionIds);
  }
}
