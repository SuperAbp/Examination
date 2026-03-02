import { Injectable, inject, OnDestroy } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { environment } from '../../../environments/environment';
import { ToasterService } from '@abp/ng.theme.shared';
import { NotificationHelper } from '../utils/notification-helper';
import { BehaviorSubject } from 'rxjs';

export interface NotificationData {
  id: string;
  type: string;
  data: string;
  creationTime: string;
  relatedEntityId?: string;
  relatedEntityType?: string;
}

@Injectable({
  providedIn: 'root',
})
export class NotificationHubService implements OnDestroy {
  private hubConnection?: HubConnection;
  private toaster = inject(ToasterService);
  private progressSubject = new BehaviorSubject<number>(0);

  readonly progress$ = this.progressSubject.asObservable();

  startConnection(): void {
    if (this.hubConnection?.state === 'Connected') {
      return;
    }

    const apiUrl = environment.apis.default.url;
    const signalRUrl = apiUrl.replace(/\/$/, '') + '/signalr-hubs/notification';

    const getAccessToken = () => {
      const token = localStorage.getItem('access_token');
      return token || '';
    };

    this.hubConnection = new HubConnectionBuilder()
      .withUrl(signalRUrl, {
        accessTokenFactory: getAccessToken,
      })
      .withAutomaticReconnect()
      .build();

    this.setupEventHandlers();

    this.hubConnection
      .start()
      .then(() => {
        console.log('[NotificationHub] Connected');
      })
      .catch(error => {
        console.error('[NotificationHub] Connection error:', error);
      });
  }

  stopConnection(): void {
    if (this.hubConnection) {
      this.hubConnection.stop();
      this.hubConnection = undefined;
    }
  }

  private setupEventHandlers(): void {
    if (!this.hubConnection) return;

    this.hubConnection.on('ReceiveNotification', (notification: NotificationData) => {
      this.handleNotification(notification);
    });

    this.hubConnection.on('ReceiveProgress', (progressValue: number) => {
      this.progressSubject.next(progressValue);
    });

    this.hubConnection.onreconnecting(() => {
      console.log('[NotificationHub] Reconnecting...');
    });

    this.hubConnection.onreconnected(() => {
      console.log('[NotificationHub] Reconnected');
    });

    this.hubConnection.onclose(() => {
      console.log('[NotificationHub] Connection closed');
    });
  }

  private handleNotification(notification: NotificationData): void {
    const content = NotificationHelper.getNotificationContent(notification);
    this.toaster.info(content, NotificationHelper.getNotificationTitle(Number(notification.type)));

    window.dispatchEvent(
      new CustomEvent('notification-received', {
        detail: { notification },
      }),
    );
  }

  ngOnDestroy(): void {
    this.stopConnection();
  }
}
