import { CoreModule } from '@abp/ng.core';
import { Component, OnInit, Input, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FooterToolbarModule } from '@delon/abc/footer-toolbar';
import { PageHeaderModule } from '@delon/abc/page-header';
import { KnowledgePointService } from '@proxy/admin/controllers';
import { GetKnowledgePointForEditorOutput } from '@proxy/admin/knowledge-points';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzModalRef } from 'ng-zorro-antd/modal';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { finalize, tap } from 'rxjs/operators';

@Component({
  selector: 'app-sys-knowledge-point-edit',
  templateUrl: './edit.component.html',
  standalone: true,
  imports: [
    CoreModule,
    PageHeaderModule,
    FooterToolbarModule,
    NzSpinModule,
    NzCardModule,
    NzGridModule,
    NzFormModule,
    NzSelectModule,
    NzInputModule,
    NzButtonModule
  ]
})
export class SysKnowledgePointEditComponent implements OnInit {
  @Input()
  knowledgePointId: string;
  @Input()
  parentId: string;
  knowledgePoint: GetKnowledgePointForEditorOutput;

  loading = false;
  isConfirmLoading = false;

  form: FormGroup = null;

  private fb = inject(FormBuilder);
  private modal = inject(NzModalRef);
  private knowledgePointService = inject(KnowledgePointService);

  ngOnInit(): void {
    this.loading = true;
    if (this.knowledgePointId) {
      this.knowledgePointService
        .getEditor(this.knowledgePointId)
        .pipe(
          tap(response => {
            this.knowledgePoint = response;
            this.buildForm();
            this.loading = false;
          })
        )
        .subscribe();
    } else {
      this.knowledgePoint = { parentId: this.parentId } as GetKnowledgePointForEditorOutput;
      this.buildForm();
      this.loading = false;
    }
  }

  buildForm() {
    this.form = this.fb.group({
      name: [this.knowledgePoint.name || '', [Validators.required]],
      parentId: [this.knowledgePoint.parentId || null]
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

    if (this.knowledgePointId) {
      this.knowledgePointService
        .update(this.knowledgePointId, {
          ...this.knowledgePoint,
          ...this.form.value
        })
        .pipe(
          tap(response => {
            this.modal.close(true);
          }),
          finalize(() => (this.isConfirmLoading = false))
        )
        .subscribe();
    } else {
      this.knowledgePointService
        .create({
          ...this.form.value
        })
        .pipe(
          tap(response => {
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
