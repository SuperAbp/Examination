import { Component, Input } from '@angular/core';

@Component({
  selector: 'blank',
  template: `
    <nz-space nzDirection="vertical" style="width: 100%">
      <ng-container *ngFor="let answer of answers">
        <input *nzSpaceItem nz-input [ngModel]="answer" [disabled]="true" />
      </ng-container>
    </nz-space>
  `
})
export class BlankComponent {
  @Input()
  answers: string[];
}
