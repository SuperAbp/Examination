import { Component, Input } from '@angular/core';

@Component({
  selector: 'blank',
  template: `
    <nz-space nzDirection="vertical" style="width: 100%">
      @for (answer of answers; track $index) {
        <input *nzSpaceItem nz-input [ngModel]="answer" [disabled]="true" />
      }
    </nz-space>
  `,
  standalone: false
})
export class BlankComponent {
  @Input()
  answers: string[];
}
