import { CoreModule, LocalizationService } from '@abp/ng.core';
import { Component, OnInit, Input, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { I18NService } from '@core';
import { r } from '@delon/mock';
import { dateTimePickerUtil } from '@delon/util';
import { ExaminationService, PaperService } from '@proxy/admin/controllers';
import { GetExamForEditorOutput } from '@proxy/admin/exam-management/exams';
import { PaperListDto } from '@proxy/admin/paper-management/papers';
import { EditorComponent, TINYMCE_SCRIPT_SRC } from '@tinymce/tinymce-angular';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzModalModule, NzModalRef } from 'ng-zorro-antd/modal';
import { NzRadioModule } from 'ng-zorro-antd/radio';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { finalize, tap } from 'rxjs/operators';

@Component({
  selector: 'app-exam-management-exam-edit',
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
  ],
  standalone: true,
  providers: [{ provide: TINYMCE_SCRIPT_SRC, useValue: 'tinymce/tinymce.min.js' }],
  imports: [
    CoreModule,
    NzModalModule,
    NzFormModule,
    NzSpinModule,
    NzSelectModule,
    NzInputModule,
    NzInputNumberModule,
    NzCheckboxModule,
    NzDatePickerModule,
    NzButtonModule,
    NzRadioModule,
    EditorComponent
  ]
})
export class ExamManagementExamEditComponent implements OnInit {
  @Input()
  examId: string;
  exam: GetExamForEditorOutput;

  loading = false;
  isConfirmLoading = false;

  form: FormGroup = null;
  showExamTime: boolean;
  papers: PaperListDto[] = [];

  private fb = inject(FormBuilder);
  private modal = inject(NzModalRef);
  private messageService = inject(NzMessageService);
  private localizationService = inject(LocalizationService);
  private i18n = inject(I18NService);
  private examService = inject(ExaminationService);
  private paperService = inject(PaperService);

  init: EditorComponent['init'] = {
    base_url: '/tinymce',
    suffix: '.min',
    plugins: 'preview fullscreen link table lists',
    toolbar:
      'undo redo | blocks fontfamily fontsize | bold italic underline strikethrough | align numlist bullist | forecolor backcolor removeformat | link table | fullscreen preview',
    toolbar_mode: 'sliding'
  };
  constructor() {
    if (this.i18n.defaultLang == 'zh-CN') {
      this.init['language'] = 'zh_CN';
      this.init['language_url'] = '/assets/tinymce/langs/zh_CN.js';
    }
  }

  get isLimitedTime() {
    return this.form.get('isLimitedTime');
  }
  get examTimes() {
    return this.form.get('examTimes');
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
    if (this.examId) {
      this.examService
        .getEditor(this.examId)
        .pipe(
          tap(response => {
            this.exam = response;
            this.buildForm();
            this.loading = false;
          })
        )
        .subscribe();
    } else {
      this.exam = {} as GetExamForEditorOutput;
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
            name: [this.exam.name || '', [Validators.required]],
            description: [this.exam.description || ''],
            score: [this.exam.score || 0],
            passingScore: [this.exam.passingScore || 0],
            totalTime: [this.exam.totalTime || 0],
            paperId: [this.exam.paperId || ''],
            startTime: [new Date()],
            endTime: [new Date()],
            isLimitedTime: [false],
            randomOrderOfOption: [this.exam.randomOrderOfOption || false],
            answerMode: [this.exam.answerMode || 0],
            examTimes: [[]]
          });
          if (this.exam.startTime && this.exam.endTime) {
            this.showExamTime = true;
            this.startTime.setValue(new Date(this.exam.startTime));
            this.endTime.setValue(new Date(this.exam.endTime));
            this.isLimitedTime.setValue(true);
            this.examTimes.setValue([this.exam.startTime, this.exam.endTime]);
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
  choosePaper(e) {
    let paper = this.papers.filter(p => p.id == e)[0];
    this.score.setValue(paper['score']);
  }

  examTimeChange(e) {
    this.startTime.setValue(e[0]);
    this.endTime.setValue(e[1]);
  }
  changeExamTimeStatus(e) {
    this.showExamTime = e;
  }

  save(publish: boolean = false) {
    if (!this.form.valid || this.isConfirmLoading) {
      for (const key of Object.keys(this.form.controls)) {
        this.form.controls[key].markAsDirty();
        this.form.controls[key].updateValueAndValidity();
      }
      return;
    }
    this.isConfirmLoading = true;

    var dynamicPara = {
      publish: publish
    };
    if (!this.isLimitedTime.value) {
      this.form.removeControl('examTimes');
      this.form.removeControl('startTime');
      this.form.removeControl('endTime');
      this.exam.startTime = null;
      this.exam.endTime = null;
    } else {
      dynamicPara['startTime'] = dateTimePickerUtil.format(this.startTime.value, 'yyyy-MM-dd HH:mm:ss');
      dynamicPara['endTime'] = dateTimePickerUtil.format(this.endTime.value, 'yyyy-MM-dd HH:mm:ss');
    }

    if (this.examId) {
      this.examService
        .update(this.examId, {
          ...this.exam,
          ...this.form.value,
          ...dynamicPara
        })
        .pipe(
          tap(response => {
            this.messageService.success(this.localizationService.instant('Exam::SaveSuccessfully'));
            this.modal.close(true);
          }),
          finalize(() => (this.isConfirmLoading = false))
        )
        .subscribe();
    } else {
      this.examService
        .create({
          ...this.form.value,
          ...dynamicPara
        })
        .pipe(
          tap(response => {
            this.messageService.success(this.localizationService.instant('Exam::SaveSuccessfully'));
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
