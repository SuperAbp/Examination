import { ConfigStateService, LocalizationService, PermissionService } from '@abp/ng.core';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { STChange, STColumn, STComponent, STPage } from '@delon/abc/st';
import { SFSchema } from '@delon/form';
import { ModalHelper } from '@delon/theme';
import { ExamingRepoService } from '@proxy/super-abp/exam/admin/controllers';
import { ExamingRepoCreateDto, ExamingRepoListDto, GetExamingReposInput } from '@proxy/super-abp/exam/admin/exam-management/exam-repos';
import { QuestionRepoListDto } from '@proxy/super-abp/exam/admin/question-management/question-repos';
import { NzMessageService } from 'ng-zorro-antd/message';
import { tap } from 'rxjs/operators';

@Component({
  selector: 'app-exam-management-repository',
  templateUrl: './repository.component.html',
  styles: [
    `
      button {
        margin-bottom: 10px;
      }
    `
  ]
})
export class ExamManagementRepositoryComponent implements OnInit {
  @Input()
  examingId: string;

  @Input()
  examingForm: FormGroup;

  examRepositories: ExamingRepoListDto[];
  total: number;
  loading = false;
  modalIsShow = false;
  modalOkLoading = false;
  repositoryItems: QuestionRepoListDto[];
  params: GetExamingReposInput;
  removeIds: any[];

  constructor(
    private fb: FormBuilder,
    private localizationService: LocalizationService,
    private messageService: NzMessageService,
    private permissionService: PermissionService,
    private repositoryService: ExamingRepoService
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
    this.modalIsShow = false;
  }

  handleCancel(): void {
    this.modalIsShow = false;
  }

  add(item: ExamingRepoCreateDto = {} as ExamingRepoCreateDto) {
    let fg = this.createAttribute(item);
    this.repositories.push(fg);
  }
  createAttribute(item: ExamingRepoCreateDto) {
    return this.fb.group({
      questionRepositoryId: [item.questionRepositoryId || null],
      singleCount: [item.singleCount || 0],
      singleScore: [item.singleScore || 0],
      judgeCount: [item.judgeCount || 0],
      judgeScore: [item.judgeScore || 0],
      multiCount: [item.multiCount || 0],
      multiScore: [item.multiScore || 0]
    });
  }
  delete(index: number, item: AbstractControl) {
    this.removeIds.push({ examingId: item.value.examingId, questionRepositoryId: item.value.questionRepositoryId });
    this.repositories.removeAt(index);
  }

  resetParameters(): GetExamingReposInput {
    return {
      skipCount: 0,
      maxResultCount: 10,
      sorting: 'CreationTime DESC'
    };
  }
}
