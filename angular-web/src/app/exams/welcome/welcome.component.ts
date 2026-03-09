import { Component, inject, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ExaminationService, UserExamService } from '@proxy/controllers';
import { ExamDetailDto } from '@proxy/exam-management/exams';
import { CoreModule } from '@abp/ng.core';

import { NgbAlert, NgbProgressbar } from '@ng-bootstrap/ng-bootstrap';
import { ButtonComponent } from '@abp/ng.theme.shared';
import { NotificationHubService } from '../../shared/services/notification-hub.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-exams-welcome',
  templateUrl: './welcome.component.html',
  imports: [CoreModule, ButtonComponent, NgbProgressbar, NgbAlert],
})
export class ExamsWelcomeComponent implements OnInit, OnDestroy {
  id?: string;
  exam?: ExamDetailDto;
  progress: number = 0;
  loading: boolean = true;
  btnLoading: boolean = false;

  private readonly examinationService = inject(ExaminationService);
  private readonly userExamService = inject(UserExamService);
  private readonly activatedRoute = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly notificationHub = inject(NotificationHubService);
  private progressSubscription?: Subscription;
  private currentUserExamId?: string;

  constructor() {
    this.activatedRoute.params.subscribe(params => {
      this.id = params['id'];
    });
  }

  ngOnInit() {
    this.examinationService.get(this.id!).subscribe(res => {
      this.exam = res;
      this.loading = false;
    });
  }

  ngOnDestroy() {
    this.progressSubscription?.unsubscribe();
  }

  start() {
    this.btnLoading = true;

    this.userExamService.create({ examId: this.id! }).subscribe({
      next: userExam => {
        this.currentUserExamId = userExam.id;
        this.setupSignalRConnection();
      },
      error: () => {
        this.btnLoading = false;
      },
    });
  }

  private setupSignalRConnection(): void {
    this.notificationHub.startConnection();
    this.progressSubscription = this.notificationHub.progress$.subscribe(progressValue => {
      this.progress = progressValue;
      if (this.progress >= 100) {
        this.router.navigate(['/exams/start', this.currentUserExamId]);
      }
    });
  }
}
