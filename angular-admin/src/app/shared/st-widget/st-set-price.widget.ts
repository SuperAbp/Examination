import { ChangeDetectionStrategy, Component } from '@angular/core';
import { ModalHelper } from '@delon/theme';
import { NzMessageService } from 'ng-zorro-antd/message';

@Component({
  selector: 'st-widget-set-price',
  template: `{{ value }}
    <button nz-button nzType="text" (click)="show()">
      <i nz-icon nzType="dollar" nzTheme="outline" *ngIf="index === -1"></i>
      <i
        nz-icon
        nz-tooltip
        [nzTooltipTitle]="title"
        nzType="check-circle"
        nzTheme="twotone"
        nzTwotoneColor="#52c41a"
        *ngIf="index > -1"
      ></i>
    </button> `,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class STSetPriceWidget {
  constructor() {}
  static readonly KEY = 'set-price';

  item: any;
  value: string;
  cmp: any;
  get index() {
    return this.cmp.inputItems.findIndex(i => i.materialId == this.item.materialId);
  }
  get title() {
    return this.index > -1 ? `￥${this.cmp.inputItems[this.index].supplyPrice}` : '0';
  }
  show() {
    this.cmp.setPrice(this.item);
  }
}
