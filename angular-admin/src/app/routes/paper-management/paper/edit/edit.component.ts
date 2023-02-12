import { LocalizationService } from '@abp/ng.core';
import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NzMessageService } from 'ng-zorro-antd/message';
import { finalize, tap } from 'rxjs/operators';
import { NzModalRef } from 'ng-zorro-antd/modal';
import { ActivatedRoute, Router } from '@angular/router';
import { dateTimePickerUtil } from '@delon/util';
import { DisabledTimeFn, DisabledTimePartial } from 'ng-zorro-antd/date-picker';
import { PaperManagementRepositoryComponent } from '../../repository/repository.component';
import { GetPaperForEditorOutput } from '@proxy/super-abp/exam/admin/paper-management/papers';
import { PaperRepoService, PaperService } from '@proxy/super-abp/exam/admin/controllers';

@Component({
  selector: 'app-exam-management-examing-edit',
  templateUrl: './edit.component.html',
  styles: [
    `
      .ant-form-item-label {
        width: 95px;
      }
    `
  ]
})
export class PaperManagementPaperEditComponent implements OnInit {
  examingId: string;
  examing: GetPaperForEditorOutput;

  @ViewChild('ExamRepository')
  examRepositoryComponent: PaperManagementRepositoryComponent;

  loading = false;
  isConfirmLoading = false;
  showExamingTime: boolean;
  form: FormGroup = null;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private messageService: NzMessageService,
    private localizationService: LocalizationService,
    private paperService: PaperService,
    private paperRepositoryService: PaperRepoService
  ) {}

  get isLimitedTime() {
    return this.form.get('isLimitedTime');
  }
  get examingTimes() {
    return this.form.get('examingTimes');
  }
  get startTime() {
    return this.form.get('startTime');
  }
  get endTime() {
    return this.form.get('endTime');
  }
  get score() {
    return this.form.get('score');
  }
  range(start: number, end: number): number[] {
    const result: number[] = [];
    for (let i = start; i < end; i++) {
      result.push(i);
    }
    return result;
  }
  disabledDate = (current: Date): boolean => dateTimePickerUtil.getDiffDays(current, new Date()) < 0;

  ngOnInit(): void {
    this.loading = true;
    this.route.paramMap.subscribe(params => {
      let id = params.get('id');
      this.examingId = id;
      if (this.examingId) {
        this.paperService
          .getEditor(this.examingId)
          .pipe(
            tap(response => {
              this.examing = response;
              this.buildForm();
              this.loading = false;
            })
          )
          .subscribe();
      } else {
        this.examing = {} as GetPaperForEditorOutput;
        this.buildForm();
        this.loading = false;
      }
    });
  }

  buildForm() {
    this.form = this.fb.group({
      name: [this.examing.name || '', [Validators.required]],
      description: [this.examing.description || ''],
      score: [this.examing.score || 0],
      // passingScore: [this.examing.passingScore || 0],
      // totalTime: [this.examing.totalTime || 0],
      isLimitedTime: [false],
      examingTimes: [[]],
      // startTime: [this.examing.startTime || new Date()],
      // endTime: [this.examing.endTime || new Date()],
      repositories: this.fb.array([])
    });
    // if (this.examing.startTime && this.examing.endTime) {
    //   this.showExamingTime = true;
    //   this.isLimitedTime.setValue(true);
    //   this.examingTimes.setValue([this.examing.startTime, this.examing.endTime]);
    // }
  }

  save() {
    if (!this.form.valid || this.isConfirmLoading) {
      for (const key of Object.keys(this.form.controls)) {
        this.form.controls[key].markAsDirty();
        this.form.controls[key].updateValueAndValidity();
      }
      return;
    }
    this.isConfirmLoading = true;

    var dynamicPara = {};
    // if (!this.isLimitedTime.value) {
    //   this.form.removeControl('examingTimes');
    //   this.form.removeControl('startTime');
    //   this.form.removeControl('endTime');
    //   this.examing.startTime = null;
    //   this.examing.endTime = null;
    // } else {
    //   dynamicPara['startTime'] = dateTimePickerUtil.format(this.startTime.value, 'yyyy-MM-dd HH:mm:ss');
    //   dynamicPara['endTime'] = dateTimePickerUtil.format(this.endTime.value, 'yyyy-MM-dd HH:mm:ss');
    // }
    if (this.examingId) {
      this.paperService
        .update(this.examingId, {
          ...this.examing,
          ...this.form.value,
          ...dynamicPara
        })
        .pipe(
          tap(res => {
            this.examRepositoryComponent
              .save(this.examingId)
              .pipe(
                tap(() => {
                  this.goback();
                }),
                finalize(() => (this.isConfirmLoading = false))
              )
              .subscribe();
          }),
          finalize(() => (this.isConfirmLoading = false))
        )
        .subscribe();
    } else {
      this.paperService
        .create({
          ...this.form.value,
          ...dynamicPara
        })
        .pipe(
          tap(res => {
            this.examRepositoryComponent
              .save(res.id)
              .pipe(
                tap(() => {
                  this.goback();
                }),
                finalize(() => (this.isConfirmLoading = false))
              )
              .subscribe();
          }),
          finalize(() => (this.isConfirmLoading = false))
        )
        .subscribe();
    }
  }

  examingTimeChange(e) {
    this.startTime.setValue(e[0]);
    this.endTime.setValue(e[1]);
  }
  changeExamingTimeStatus(e) {
    this.showExamingTime = e;
  }
  changeTotalScore(e) {
    this.score.setValue(e);
  }

  back(e: MouseEvent) {
    e.preventDefault();
    this.goback();
  }
  goback() {
    this.router.navigateByUrl('/paper-management/paper');
  }
}
