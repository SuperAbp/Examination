import { Injectable, inject } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { environment } from '../../../environments/environment';
import { ToasterService } from '@abp/ng.theme.shared';
import { NotificationHelper } from '../utils/notification-helper';

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
export class NotificationHubService {
  private hubConnection?: HubConnection;
  private toaster = inject(ToasterService);
  private reconnectAttempts = 0;
  private readonly maxReconnectAttempts = 5;

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
        this.reconnectAttempts = 0;
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

    this.hubConnection.onreconnecting(() => {
      console.log('[NotificationHub] Reconnecting...');
    });

    this.hubConnection.onreconnected(() => {
      console.log('[NotificationHub] Reconnected');
      this.reconnectAttempts = 0;
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

  getConnectionState(): string {
    return this.hubConnection?.state ?? 'Disconnected';
  }
}
