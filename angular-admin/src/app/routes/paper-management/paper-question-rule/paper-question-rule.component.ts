import { CoreModule, LocalizationService } from '@abp/ng.core';
import { Component, EventEmitter, Input, OnInit, Output, inject } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PaperQuestionRuleService, QuestionBankService } from '@proxy/admin/controllers';
import { GetPaperQuestionRulesInput, PaperQuestionRuleListDto } from '@proxy/admin/paper-management/paper-question-rules';
import { PaperCreateOrUpdateDtoBase_PaperSectionDto_PaperQuestionRuleDto } from '@proxy/admin/paper-management/papers';
import { QuestionBankListDto } from '@proxy/admin/question-management/question-banks';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzToolTipModule } from 'ng-zorro-antd/tooltip';
import { forkJoin, Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

export interface PaperQuestionRuleCreateTemp extends PaperCreateOrUpdateDtoBase_PaperSectionDto_PaperQuestionRuleDto {
  id?: string;
  questionBank: string;
  totalCount: number;
}
@Component({
  selector: 'app-paper-management-paper-question-rule',
  templateUrl: './paper-question-rule.component.html',
  styles: [
    `
      button {
        margin-bottom: 10px;
      }
      nz-select {
        width: 100%;
      }
    `
  ],
  standalone: true,
  imports: [
    CoreModule,
    NzButtonModule,
    NzTableModule,
    NzFormModule,
    NzInputNumberModule,
    NzPopconfirmModule,
    NzToolTipModule,
    NzModalModule,
    NzIconModule,
    NzSelectModule
  ]
})
export class PaperManagementPaperQuestionRuleComponent implements OnInit {
  @Input()
  paperId: string;

  @Input()
  paperForm: FormGroup;
  @Output()
  readonly totalScoreChange = new EventEmitter();

  examRepositories: PaperQuestionRuleListDto[];
  total: number;
  loading = false;
  modalIsShow = false;
  modalOkLoading = false;
  currentQuestionRepositoryId;
  repositoryItems: QuestionBankListDto[];
  params: GetPaperQuestionRulesInput;
  paperQuestionRuleTemps: PaperQuestionRuleCreateTemp[] = [];

  private fb = inject(FormBuilder);
  private localizationService = inject(LocalizationService);
  private messageService = inject(NzMessageService);
  private paperQuestionRuleService = inject(PaperQuestionRuleService);
  private questionBankService = inject(QuestionBankService);

  get paperQuestionRules() {
    return this.paperForm.get('paperQuestionRules') as FormArray;
  }

  ngOnInit() {
    this.loaded();
  }
  loaded() {
    if (this.loading) {
      return;
    }
    this.loading = true;
    this.params = this.resetParameters();
    this.questionBankService
      .getList({ skipCount: 0, maxResultCount: 100 })
      .pipe(
        tap(res => {
          this.repositoryItems = res.items;
        })
      )
      .subscribe();
    if (this.paperId) {
      this.getList();
    } else {
      this.loading = false;
    }
  }
  getList() {
    this.loading = true;
    this.paperQuestionRuleService
      .getList(this.params)
      .pipe(tap(() => (this.loading = false)))
      .subscribe(response => {
        response.items.forEach(repo => {
          this.questionBankService
            .getQuestionCount(repo.questionBankId)
            .pipe(
              tap(res => {
                // this.add({
                //   id: repo.id,
                //   questionBank: repo.questionBank,
                //   questionBankId: repo.questionBankId,
                //   questionType: repo.questionType,
                //   count: repo.count,
                //   score: repo.score,
                //   totalCount: res.totalCount
                // } as PaperQuestionRuleCreateTemp);
              })
            )
            .subscribe();
        });
      });
  }

  handleOk(): void {
    let item = this.repositoryItems.find(i => i.id == this.currentQuestionRepositoryId);
    // this.add({
    //   questionBankId: item.id,
    //   questionBank: item.title,
    //   singleTotalCount: item.singleCount,
    //   multiTotalCount: item.multiCount,
    //   judgeTotalCount: item.judgeCount,
    //   blankTotalCount: item.blankCount
    // });
    this.currentQuestionRepositoryId = null;
    this.modalIsShow = false;
  }

  handleCancel(): void {
    this.currentQuestionRepositoryId = null;
    this.modalIsShow = false;
  }

  add(item: PaperQuestionRuleCreateTemp = {} as PaperQuestionRuleCreateTemp) {
    if (this.paperQuestionRuleTemps.findIndex(r => r.questionBankId == item.questionBankId) > -1) {
      this.messageService.error(this.localizationService.instant('Exam::QuestionBankExists'));
      return;
    }
    let fg = this.createAttribute(item);
    this.paperQuestionRules.push(fg);
    this.paperQuestionRuleTemps.push(item);
  }
  createAttribute(item: PaperQuestionRuleCreateTemp) {
    return this.fb.group({
      id: [item.id || null],
      questionBankId: [item.questionBankId || null, [Validators.required]],
      singleCount: [item.count || 0, [Validators.required]],
      singleScore: [item.score || 0, [Validators.required]]
    });
  }
  delete(index: number, item: PaperQuestionRuleCreateTemp) {
    if (item.id == null) {
      this.paperQuestionRules.removeAt(index);
      this.paperQuestionRuleTemps.splice(index, 1);
      this.changeScore(null);
      return;
    }
    this.paperQuestionRuleService.delete(item.id).subscribe(() => {
      this.paperQuestionRules.removeAt(index);
      this.paperQuestionRuleTemps.splice(index, 1);
      this.changeScore(null);
    });
  }

  changeScore(e) {
    let totalScore = 0;
    this.paperQuestionRules.controls.forEach(c => {
      totalScore += c.get('count').value * c.get('score').value;
    });
    this.totalScoreChange.emit(totalScore);
  }

  resetParameters(): GetPaperQuestionRulesInput {
    return {
      paperId: this.paperId,
      skipCount: 0,
      maxResultCount: 10
    };
  }
}
