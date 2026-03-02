import { NotificationListDto } from '@proxy/notifications/models';

export interface NotificationData {
  id: string;
  type: string;
  data: string;
  creationTime: string;
  relatedEntityId?: string;
  relatedEntityType?: string;
}

export class NotificationHelper {
  static getNotificationTitle(type: number): string {
    const titles: Record<number, string> = {
      1: '考试提醒',
      2: '成绩公布',
      3: '系统通知',
    };
    return titles[type] || '通知';
  }

  static getNotificationContent(notification: NotificationListDto | NotificationData): string {
    const dataStr = 'data' in notification ? notification.data : notification.data;

    if (!dataStr) {
      return '您有一条新通知';
    }

    try {
      const data = JSON.parse(dataStr);
      const type =
        'type' in notification
          ? Number(notification.type)
          : (notification as NotificationListDto).type;

      if (type === 2) {
        return `您参加的《${data.examName || '考试'}》成绩已发布`;
      }

      return data.message || '您有一条新通知';
    } catch {
      return '您有一条新通知';
    }
  }
}
