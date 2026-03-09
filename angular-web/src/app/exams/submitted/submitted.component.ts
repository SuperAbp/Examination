import { Component, OnInit, inject } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { CoreModule } from '@abp/ng.core';

@Component({
  selector: 'app-exams-submitted',
  templateUrl: './submitted.component.html',
  styleUrls: ['./submitted.component.scss'],
  standalone: true,
  imports: [CoreModule],
})
export class ExamsSubmittedComponent implements OnInit {
  userExamId?: string;
  examName?: string;

  private readonly router = inject(Router);
  private readonly activatedRoute = inject(ActivatedRoute);

  ngOnInit() {
    this.activatedRoute.params.subscribe(params => {
      this.userExamId = params['id'];
    });

    this.activatedRoute.queryParams.subscribe(params => {
      this.examName = params['examName'];
    });
  }
}
