import { ConfigStateService, CoreModule, LocalizationService, PermissionService } from '@abp/ng.core';
import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { PageHeaderModule } from '@delon/abc/page-header';
import { STChange, STColumn, STComponent, STModule, STPage } from '@delon/abc/st';
import { DelonFormModule, SFSchema, SFStringWidgetSchema } from '@delon/form';
import { ModalHelper } from '@delon/theme';
import { KnowledgePointService } from '@proxy/admin/controllers';
import { GetKnowledgePointsInput, KnowledgePointNodeDto } from '@proxy/admin/knowledge-points';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';
import { NzTableModule } from 'ng-zorro-antd/table';
import { tap } from 'rxjs/operators';

import { SysKnowledgePointEditComponent } from './edit/edit.component';

export interface TreeNodeInterface extends KnowledgePointNodeDto {
  level?: number;
  expand?: boolean;
  parent?: TreeNodeInterface;
}

@Component({
  selector: 'app-sys-knowledge-point',
  templateUrl: './knowledge-point.component.html',
  standalone: true,
  imports: [
    CoreModule,
    PageHeaderModule,
    DelonFormModule,
    STModule,
    NzCardModule,
    NzButtonModule,
    NzTableModule,
    NzIconModule,
    NzPopconfirmModule
  ]
})
export class SysKnowledgePointComponent implements OnInit {
  private modal = inject(ModalHelper);
  private localizationService = inject(LocalizationService);
  private messageService = inject(NzMessageService);
  private permissionService = inject(PermissionService);
  private knowledgePointService = inject(KnowledgePointService);

  knowledgePoints: KnowledgePointNodeDto[];
  loading = false;
  params: GetKnowledgePointsInput;
  page: STPage = {
    show: false
  };

  mapOfExpandedData: { [key: string]: TreeNodeInterface[] } = {};

  ngOnInit() {
    this.params = this.resetParameters();
    this.getList();
  }
  getList() {
    this.loading = true;
    this.knowledgePointService
      .getAll(this.params)
      .pipe(tap(() => (this.loading = false)))
      .subscribe(response => {
        this.knowledgePoints = response.items;

        this.mapOfExpandedData = {};
        this.knowledgePoints.forEach(item => {
          this.mapOfExpandedData[item.id] = this.convertTreeToList(item);
        });
      });
  }
  convertTreeToList(root: KnowledgePointNodeDto): TreeNodeInterface[] {
    const stack: TreeNodeInterface[] = [];
    const array: TreeNodeInterface[] = [];
    const hashMap = {};
    stack.push({ ...root, level: 0, expand: true });

    while (stack.length !== 0) {
      const node = stack.pop()!;
      if (node.children && node.children.length > 0) {
        node.children.forEach(child => {
          stack.push({ ...child, level: node.level! + 1, expand: false, parent: node });
        });
      } else {
        delete node.children;
      }
      this.visitNode(node, hashMap, array);
    }
    console.log(array);

    return array;
  }
  visitNode(node: TreeNodeInterface, hashMap: { [id: string]: boolean }, array: TreeNodeInterface[]): void {
    if (!hashMap[node.id]) {
      hashMap[node.id] = true;
      array.push(node);
    }
  }
  collapse(array: TreeNodeInterface[], data: TreeNodeInterface, $event: boolean): void {
    if (!$event) {
      if (data.children) {
        data.children.forEach(d => {
          const target = array.find(a => a.id === d.id)!;
          target.expand = false;
          this.collapse(array, target, false);
        });
      } else {
        return;
      }
    }
  }

  resetParameters(): GetKnowledgePointsInput {
    return {};
  }
  reset() {
    this.params = this.resetParameters();
    this.getList();
  }
  edit(id) {
    this.modal.createStatic(SysKnowledgePointEditComponent, { knowledgePointId: id }).subscribe(() => this.getList());
  }
  add(id = null) {
    this.modal.createStatic(SysKnowledgePointEditComponent, { knowledgePointId: '', parentId: id }).subscribe(() => this.getList());
  }
  delete(record) {
    this.knowledgePointService.delete(record.id).subscribe(response => {
      this.messageService.success(this.localizationService.instant('Exam::DeletedSuccessfully', record.name));
      // tslint:disable-next-line: no-non-null-assertion
      this.knowledgePoints = this.knowledgePoints.filter(item => item.id !== record.id);
    });
  }
}
