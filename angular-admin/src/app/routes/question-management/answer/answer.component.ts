import { AfterViewInit, Component, Input, OnInit } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { QuestionService } from '@proxy/admin/controllers';
import { QuestionAnswerDto } from '@proxy/admin/question-management/questions';

interface QuestionAnswerTemp extends QuestionAnswerDto {
  id?: string;
}
@Component({
  selector: 'app-question-management-answer',
  template: '',
  styles: [
    `
      button {
        margin-bottom: 10px;
      }
      nz-input-number {
        width: 100%;
      }
      .required:before {
        display: inline-block;
        margin-right: 4px;
        color: #ff4d4f;
        font-size: 14px;
        font-family: SimSun, sans-serif;
        line-height: 1;
        content: '*';
      }
    `
  ],
  standalone: true
})
export class QuestionManagementAnswerComponent implements AfterViewInit {
  @Input()
  questionId: string;
  @Input()
  questionType: number;
  @Input()
  questionForm: FormGroup;
  @Input()
  answers: QuestionAnswerDto[];

  removeIds: string[] = [];
  loading = false;

  constructor(
    protected fb: FormBuilder,
    protected questionService: QuestionService
  ) {}
  ngAfterViewInit(): void {
    if (this.answers.length < 0) {
      this.batchAdd(2);
    } else {
      this.answers.forEach(item => {
        this.add({
          id: item.id,
          sort: item.sort,
          right: item.right,
          content: item.content,
          analysis: item.analysis
        });
      });
    }
  }

  get options() {
    return this.questionForm.get('options') as FormArray;
  }
  batchAdd(length) {
    this.options.clear();
    for (let index = 0; index < length; index++) {
      this.add({ right: false, sort: 0 });
    }
  }

  add(item: QuestionAnswerTemp = {} as QuestionAnswerTemp) {
    let fg = this.createAttribute(item);
    this.options.push(fg);
  }
  createAttribute(item: QuestionAnswerTemp) {
    return this.fb.group({
      id: [item.id || null],
      right: [item.right || false],
      content: [item.content || null, [Validators.required]],
      analysis: [item.analysis || null],
      sort: [item.sort || 0]
    });
  }
  delete(index: number, item: AbstractControl) {
    if (item.value.id && item.value.id !== null) {
      this.questionService.deleteAnswer(this.questionId, item.value.id).subscribe(() => {
        this.options.removeAt(index);
      });
    } else {
      this.options.removeAt(index);
    }
  }
}
