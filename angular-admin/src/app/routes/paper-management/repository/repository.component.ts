import { CoreModule, LocalizationService } from '@abp/ng.core';
import { Component, EventEmitter, Input, OnInit, Output, inject } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PaperRepoService, QuestionRepoService } from '@proxy/admin/controllers';
import { GetPaperReposInput, PaperRepoListDto } from '@proxy/admin/paper-management/paper-repos';
import { PaperCreateOrUpdatePaperRepoDto } from '@proxy/admin/paper-management/papers';
import { QuestionRepoListDto } from '@proxy/admin/question-management/question-repos';
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

export interface PaperRepoCreateTemp extends PaperCreateOrUpdatePaperRepoDto {
  id?: string;
  questionRepository: string;
  singleTotalCount: number;
  multiTotalCount?: number;
  judgeTotalCount?: number;
  blankTotalCount?: number;
}
@Component({
  selector: 'app-paper-management-repository',
  templateUrl: './repository.component.html',
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
export class PaperManagementRepositoryComponent implements OnInit {
  @Input()
  paperId: string;

  @Input()
  paperForm: FormGroup;
  @Output()
  totalScoreChange = new EventEmitter();

  examRepositories: PaperRepoListDto[];
  total: number;
  loading = false;
  modalIsShow = false;
  modalOkLoading = false;
  currentQuestionRepositoryId;
  repositoryItems: QuestionRepoListDto[];
  params: GetPaperReposInput;
  repositoryTemps: PaperRepoCreateTemp[] = [];

  private fb = inject(FormBuilder);
  private localizationService = inject(LocalizationService);
  private messageService = inject(NzMessageService);
  private paperRepositoryService = inject(PaperRepoService);
  private questionRepositoryService = inject(QuestionRepoService);

  get repositories() {
    return this.paperForm.get('repositories') as FormArray;
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
    this.questionRepositoryService
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
    this.paperRepositoryService
      .getList(this.params)
      .pipe(tap(() => (this.loading = false)))
      .subscribe(response => {
        response.items.forEach(repo => {
          this.questionRepositoryService
            .getQuestionCount(repo.questionRepositoryId)
            .pipe(
              tap(res => {
                this.add({
                  id: repo.id,
                  paperId: this.paperId,
                  questionRepository: repo.questionRepository,
                  questionRepositoryId: repo.questionRepositoryId,
                  singleTotalCount: res.singleCount,
                  singleCount: repo.singleCount,
                  singleScore: repo.singleScore,
                  multiTotalCount: res.multiCount,
                  multiCount: repo.multiCount,
                  multiScore: repo.multiScore,
                  judgeTotalCount: res.judgeCount,
                  judgeCount: repo.judgeCount,
                  judgeScore: repo.judgeScore,
                  blankTotalCount: res.blankCount,
                  blankCount: repo.blankCount,
                  blankScore: repo.blankScore
                } as PaperRepoCreateTemp);
              })
            )
            .subscribe();
        });
      });
  }

  handleOk(): void {
    let item = this.repositoryItems.find(i => i.id == this.currentQuestionRepositoryId);
    this.add({
      questionRepositoryId: item.id,
      questionRepository: item.title,
      singleTotalCount: item.singleCount,
      multiTotalCount: item.multiCount,
      judgeTotalCount: item.judgeCount,
      blankTotalCount: item.blankCount
    });
    this.currentQuestionRepositoryId = null;
    this.modalIsShow = false;
  }

  handleCancel(): void {
    this.currentQuestionRepositoryId = null;
    this.modalIsShow = false;
  }

  add(item: PaperRepoCreateTemp = {} as PaperRepoCreateTemp) {
    if (this.repositoryTemps.findIndex(r => r.questionRepositoryId == item.questionRepositoryId) > -1) {
      this.messageService.error(this.localizationService.instant('Exam::QuestionRepositoryExists'));
      return;
    }
    let fg = this.createAttribute(item);
    this.repositories.push(fg);
    this.repositoryTemps.push(item);
  }
  createAttribute(item: PaperRepoCreateTemp) {
    return this.fb.group({
      id: [item.id || null],
      questionRepositoryId: [item.questionRepositoryId || null, [Validators.required]],
      singleCount: [item.singleCount || 0, [Validators.required]],
      singleScore: [item.singleScore || 0, [Validators.required]],
      judgeCount: [item.judgeCount || 0, [Validators.required]],
      judgeScore: [item.judgeScore || 0, [Validators.required]],
      multiCount: [item.multiCount || 0, [Validators.required]],
      multiScore: [item.multiScore || 0, [Validators.required]],
      blankCount: [item.blankCount || 0, [Validators.required]],
      blankScore: [item.blankScore || 0, [Validators.required]]
    });
  }
  delete(index: number, item: PaperRepoCreateTemp) {
    this.paperRepositoryService.delete(item.id).subscribe(() => {
      this.repositories.removeAt(index);
      this.repositoryTemps.splice(index, 1);
      this.changeScore(null);
    });
  }

  changeScore(e) {
    let totalScore = 0;
    this.repositories.controls.forEach(c => {
      totalScore +=
        c.get('singleCount').value * c.get('singleScore').value +
        c.get('judgeCount').value * c.get('judgeScore').value +
        c.get('multiCount').value * c.get('multiScore').value +
        c.get('blankCount').value * c.get('blankScore').value;
    });
    this.totalScoreChange.emit(totalScore);
  }

  resetParameters(): GetPaperReposInput {
    return {
      paperId: this.paperId,
      skipCount: 0,
      maxResultCount: 10,
      sorting: 'CreationTime DESC'
    };
  }
}
