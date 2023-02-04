import { ConfigStateService, LocalizationService, PermissionService } from '@abp/ng.core';
import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { STChange, STColumn, STComponent, STPage } from '@delon/abc/st';
import { SFSchema } from '@delon/form';
import { ModalHelper } from '@delon/theme';
import { ExamingRepoService, QuestionRepoService } from '@proxy/super-abp/exam/admin/controllers';
import { ExamingRepoCreateDto, ExamingRepoListDto, GetExamingReposInput } from '@proxy/super-abp/exam/admin/exam-management/exam-repos';
import { QuestionRepoListDto } from '@proxy/super-abp/exam/admin/question-management/question-repos';
import { NzMessageService } from 'ng-zorro-antd/message';
import { forkJoin, Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

export interface ExamingRepoCreateTemp extends ExamingRepoCreateDto {
  questionRepository: string;
  singleTotalCount: number;
  multiTotalCount?: number;
  judgeTotalCount?: number;
}
@Component({
  selector: 'app-exam-management-repository',
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
  ]
})
export class ExamManagementRepositoryComponent implements OnInit {
  @Input()
  examingId: string;

  @Input()
  examingForm: FormGroup;
  @Output()
  totalScoreChange = new EventEmitter();

  examRepositories: ExamingRepoListDto[];
  total: number;
  loading = false;
  modalIsShow = false;
  modalOkLoading = false;
  currentQuestionRepositoryId;
  repositoryItems: QuestionRepoListDto[];
  params: GetExamingReposInput;
  removeRepositoryIds: any[] = [];
  repositoryTemps: ExamingRepoCreateTemp[] = [];

  constructor(
    private fb: FormBuilder,
    private localizationService: LocalizationService,
    private messageService: NzMessageService,
    private permissionService: PermissionService,
    private repositoryService: ExamingRepoService,
    private questionRepositoryService: QuestionRepoService,
    private readonly examingRepositoryService: ExamingRepoService
  ) {}

  get repositories() {
    return this.examingForm.get('repositories') as FormArray;
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
    if (this.examingId) {
      this.getList();
    } else {
      this.loading = false;
    }
  }
  getList() {
    this.loading = true;
    this.repositoryService
      .getList(this.params)
      .pipe(tap(() => (this.loading = false)))
      .subscribe(response => ((this.examRepositories = response.items), (this.total = response.totalCount)));
  }

  handleOk(): void {
    let item = this.repositoryItems.find(i => i.id == this.currentQuestionRepositoryId);
    this.add({
      questionRepositoryId: item.id,
      examingId: this.examingId,
      questionRepository: item.title,
      singleTotalCount: item.singleCount,
      multiTotalCount: item.multiCount,
      judgeTotalCount: item.judgeCount
    });
    this.currentQuestionRepositoryId = null;
    this.modalIsShow = false;
  }

  handleCancel(): void {
    this.currentQuestionRepositoryId = null;
    this.modalIsShow = false;
  }

  add(item: ExamingRepoCreateTemp = {} as ExamingRepoCreateTemp) {
    if (this.repositoryTemps.findIndex(r => r.questionRepositoryId == item.questionRepositoryId) > -1) {
      this.messageService.error(this.localizationService.instant('Exam::QuestionRepositoryExists'));
      return;
    }
    let fg = this.createAttribute(item as ExamingRepoCreateDto);
    this.repositories.push(fg);
    this.repositoryTemps.push(item);
  }
  createAttribute(item: ExamingRepoCreateDto) {
    return this.fb.group({
      questionRepositoryId: [item.questionRepositoryId || null, [Validators.required]],
      singleCount: [item.singleCount || 0, [Validators.required]],
      singleScore: [item.singleScore || 0, [Validators.required]],
      judgeCount: [item.judgeCount || 0, [Validators.required]],
      judgeScore: [item.judgeScore || 0, [Validators.required]],
      multiCount: [item.multiCount || 0, [Validators.required]],
      multiScore: [item.multiScore || 0, [Validators.required]]
    });
  }
  delete(index: number, item: ExamingRepoCreateTemp) {
    this.removeRepositoryIds.push({ examingId: item.examingId, questionRepositoryId: item.questionRepositoryId });
    this.repositories.removeAt(index);
    this.repositoryTemps.splice(index, 1);
  }

  save(examingId) {
    var services: Array<Observable<any>> = [];
    this.repositories.controls.forEach(repository => {
      var value = repository.value;
      services.push(
        this.examingRepositoryService.createOrUpdate({
          examingId: this.examingId,
          ...value
        })
      );
    });
    if (this.removeRepositoryIds.length > 0) {
      this.removeRepositoryIds.forEach(id => {
        services.push(this.examingRepositoryService.delete(examingId, id));
      });
    }
    return forkJoin(services);
  }
  changeScore(e) {
    if (e) {
      let totalScore = 0;
      this.repositories.controls.forEach(c => {
        totalScore +=
          c.get('singleCount').value * c.get('singleScore').value +
          c.get('judgeCount').value * c.get('judgeScore').value +
          c.get('multiCount').value * c.get('multiScore').value;
      });
      this.totalScoreChange.emit(totalScore);
    }
  }

  resetParameters(): GetExamingReposInput {
    return {
      examingId: this.examingId,
      skipCount: 0,
      maxResultCount: 10,
      sorting: 'CreationTime DESC'
    };
  }
}
