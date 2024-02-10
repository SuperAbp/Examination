import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { finalize, tap } from 'rxjs/operators';
import { ActivatedRoute, Router } from '@angular/router';
import { dateTimePickerUtil } from '@delon/util';
import { PaperManagementRepositoryComponent } from '../../repository/repository.component';
import { GetPaperForEditorOutput } from '@proxy/super-abp/exam/admin/paper-management/papers';
import { PaperService } from '@proxy/super-abp/exam/admin/controllers';
import { CoreModule } from '@abp/ng.core';
import { PageHeaderModule } from '@delon/abc/page-header';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { FooterToolbarModule } from '@delon/abc/footer-toolbar';

@Component({
  selector: 'app-exam-management-paper-edit',
  templateUrl: './edit.component.html',
  styles: [
    `
      .ant-form-item-label {
        width: 95px;
      }
    `
  ],
  standalone: true,
  imports: [
    CoreModule,
    PageHeaderModule,
    FooterToolbarModule,
    NzSpinModule,
    NzCardModule,
    NzFormModule,
    NzInputModule,
    NzInputNumberModule,
    NzButtonModule,
    PaperManagementRepositoryComponent
  ]
})
export class PaperManagementPaperEditComponent implements OnInit {
  paperId: string;
  paper: GetPaperForEditorOutput;

  @ViewChild('PaperRepository')
  paperRepositoryComponent: PaperManagementRepositoryComponent;

  loading = false;
  isConfirmLoading = false;
  showPaperTime: boolean;
  form: FormGroup = null;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private paperService: PaperService
  ) {}
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
      this.paperId = id;
      if (this.paperId) {
        this.paperService
          .getEditor(this.paperId)
          .pipe(
            tap(response => {
              this.paper = response;
              this.buildForm();
              this.loading = false;
            })
          )
          .subscribe();
      } else {
        this.paper = {} as GetPaperForEditorOutput;
        this.buildForm();
        this.loading = false;
      }
    });
  }

  buildForm() {
    this.form = this.fb.group({
      name: [this.paper.name || '', [Validators.required]],
      description: [this.paper.description || ''],
      score: [this.paper.score || 0],
      repositories: this.fb.array([])
    });
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
    if (this.paperId) {
      this.paperService
        .update(this.paperId, {
          ...this.paper,
          ...this.form.value,
          ...dynamicPara
        })
        .pipe(
          tap(() => {
            this.paperRepositoryComponent
              .save(this.paperId)
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
            this.paperRepositoryComponent
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
