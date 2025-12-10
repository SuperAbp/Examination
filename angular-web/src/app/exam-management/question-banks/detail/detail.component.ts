import { CoreModule } from '@abp/ng.core';
import { Component, inject, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { forkJoin } from 'rxjs';
import { QuestionBankService } from '@proxy/controllers';
import { QuestionBankDetailDto } from '@proxy/question-management/question-banks';

@Component({
  selector: 'app-question-banks-detail',
  templateUrl: './detail.component.html',
  styleUrls: ['./detail.component.scss'],
  imports: [CoreModule, CommonModule],
})
export class QuestionBanksDetailComponent implements OnInit {
  id: string;
  questionBank: QuestionBankDetailDto;
  questionTypes: number[];
  private readonly questionBankService = inject(QuestionBankService);
  private readonly router = inject(Router);
  private readonly activatedRoute = inject(ActivatedRoute);

  constructor() {
    this.activatedRoute.params.subscribe(params => {
      this.id = params['id'];
    });
  }
  ngOnInit(): void {
    forkJoin([
      this.questionBankService.get(this.id),
      this.questionBankService.getQuestionTypes(this.id),
    ]).subscribe(([questionBank, questionTypes]) => {
      this.questionBank = questionBank;
      this.questionTypes = questionTypes.items;
    });
    this.questionBankService.get(this.id).subscribe(res => {
      this.questionBank = res;
    });
  }

  goBack(): void {
    this.router.navigate(['/question-banks']);
  }

  start(mode: number, type?: number): void {
    this.router.navigate([`/question-banks/${this.id}/train`], {
      queryParams: { mode, type },
    });
  }
}
