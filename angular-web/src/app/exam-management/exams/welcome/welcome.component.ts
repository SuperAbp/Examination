import { Component, inject, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ExaminationService, UserExamService } from '@proxy/controllers';
import { ExamDetailDto } from '@proxy/exam-management/exams';
import { CoreModule } from '@abp/ng.core';
import { CommonModule } from '@angular/common';
import { NgbProgressbar } from '@ng-bootstrap/ng-bootstrap';
import { ButtonComponent } from '@abp/ng.theme.shared';
import * as signalR from '@microsoft/signalr';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-welcome',
  templateUrl: './welcome.component.html',
  imports: [CoreModule, CommonModule, ButtonComponent, NgbProgressbar],
})
export class ExamsWelcomeComponent implements OnInit, OnDestroy {
  id?: string;
  exam?: ExamDetailDto;
  progress: number = 0;
  btnLoading: boolean = false;
  private hubConnection?: signalR.HubConnection;

  private readonly examinationService = inject(ExaminationService);
  private readonly userExamService = inject(UserExamService);
  private readonly activatedRoute = inject(ActivatedRoute);
  private readonly router = inject(Router);

  constructor() {
    this.activatedRoute.params.subscribe(params => {
      this.id = params['id'];
    });
  }

  ngOnInit() {
    this.examinationService.get(this.id!).subscribe(res => {
      this.exam = res;
    });
  }

  ngOnDestroy() {
    if (this.hubConnection) {
      this.hubConnection.stop();
    }
  }

  async start() {
    this.btnLoading = true;

    this.userExamService.create({ examId: this.id! }).subscribe({
      next: async () => {
        await this.setupSignalRConnection();
      },
      error: () => {
        this.btnLoading = false;
      },
    });
  }

  private async setupSignalRConnection() {
    try {
      const apiUrl = environment.apis.default.url;
      const signalRUrl = apiUrl.replace(/\/$/, '') + '/signalr-hubs/progress';

      // 获取访问令牌
      const getAccessToken = () => {
        const token = localStorage.getItem('access_token');
        return token || '';
      };

      this.hubConnection = new signalR.HubConnectionBuilder()
        .withUrl(signalRUrl, {
          accessTokenFactory: getAccessToken,
        })
        .withAutomaticReconnect()
        .build();

      this.hubConnection.on('ReceiveProgress', (progressValue: number) => {
        this.progress = progressValue;
        if (this.progress >= 100) {
          this.hubConnection?.stop();
          this.router.navigate(['/exam/start', this.id]);
        }
      });

      await this.hubConnection.start();
    } catch (error) {
      console.error('SignalR Connection Error: ', error);
      this.btnLoading = false;
    }
  }
}
