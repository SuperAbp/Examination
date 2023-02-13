import { LocalizationService } from '@abp/ng.core';
import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NzMessageService } from 'ng-zorro-antd/message';
import { finalize, tap } from 'rxjs/operators';
import { NzModalRef } from 'ng-zorro-antd/modal';
import { GetExamingForEditorOutput } from '@proxy/super-abp/exam/admin/exam-management/exams';
import { ExamingService, PaperService } from '@proxy/super-abp/exam/admin/controllers';
import { dateTimePickerUtil } from '@delon/util';
import { PaperListDto } from '@proxy/super-abp/exam/admin/paper-management/papers';

@Component({
  selector: 'app-examing-management-examing-edit',
  templateUrl: './edit.component.html',
  styles: [
    `
      nz-select {
        width: 100%;
      }

      .loading-icon {
        margin-right: 8px;
      }
    `
  ]
})
export class ExamingManagementExamingEditComponent implements OnInit {
  @Input()
  examingId: string;
  examing: GetExamingForEditorOutput;

  loading = false;
  isConfirmLoading = false;

  form: FormGroup = null;
  showExamingTime: boolean;
  papers: PaperListDto[] = [];

  constructor(
    private fb: FormBuilder,
    private modal: NzModalRef,
    private messageService: NzMessageService,
    private localizationService: LocalizationService,
    private examingService: ExamingService,
    private paperService: PaperService
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
  disabledDate = (current: Date): boolean => dateTimePickerUtil.getDiffDays(current, new Date()) < 0;
  ngOnInit(): void {
    this.loading = true;
    if (this.examingId) {
      this.examingService
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
      this.examing = {} as GetExamingForEditorOutput;
      this.buildForm();
      this.loading = false;
    }
  }

  buildForm() {
    this.paperService
      .getList({ skipCount: 0, maxResultCount: 100 })
      .pipe(
        tap(res => {
          this.papers = res.items;

          this.form = this.fb.group({
            name: [this.examing.name || '', [Validators.required]],
            description: [this.examing.description || ''],
            score: [this.examing.score || ''],
            passingScore: [this.examing.passingScore || 0],
            totalTime: [this.examing.totalTime || 0],
            paperId: [this.examing.paperId || ''],
            startTime: [this.examing.startTime || new Date()],
            endTime: [this.examing.endTime || new Date()],
            isLimitedTime: [false],
            examingTimes: [[]]
          });
          if (this.examing.startTime && this.examing.endTime) {
            this.showExamingTime = true;
            this.isLimitedTime.setValue(true);
            this.examingTimes.setValue([this.examing.startTime, this.examing.endTime]);
          }
        })
      )
      .subscribe();
  }

  searchPaper(value: string): void {
    let params = { skipCount: 0, maxResultCount: 100, name: value };
    if ((value || '') != '') {
      params.name = value;
    }
    this.paperService
      .getList(params)
      .pipe(
        tap(res => {
          this.papers = res.items;
        })
      )
      .subscribe();
  }

  examingTimeChange(e) {
    this.startTime.setValue(e[0]);
    this.endTime.setValue(e[1]);
  }
  changeExamingTimeStatus(e) {
    this.showExamingTime = e;
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
    if (!this.isLimitedTime.value) {
      this.form.removeControl('examingTimes');
      this.form.removeControl('startTime');
      this.form.removeControl('endTime');
      this.examing.startTime = null;
      this.examing.endTime = null;
    } else {
      dynamicPara['startTime'] = dateTimePickerUtil.format(this.startTime.value, 'yyyy-MM-dd HH:mm:ss');
      dynamicPara['endTime'] = dateTimePickerUtil.format(this.endTime.value, 'yyyy-MM-dd HH:mm:ss');
    }

    if (this.examingId) {
      this.examingService
        .update(this.examingId, {
          ...this.examing,
          ...this.form.value,
          ...dynamicPara
        })
        .pipe(
          tap(response => {
            this.messageService.success(this.localizationService.instant('*::SaveSucceed'));
            this.modal.close(true);
          }),
          finalize(() => (this.isConfirmLoading = false))
        )
        .subscribe();
    } else {
      this.examingService
        .create({
          ...this.form.value,
          ...dynamicPara
        })
        .pipe(
          tap(response => {
            this.messageService.success(this.localizationService.instant('*::SaveSucceed'));
            this.modal.close(true);
          }),
          finalize(() => (this.isConfirmLoading = false))
        )
        .subscribe();
    }
  }

  close() {
    this.modal.destroy();
  }
}
