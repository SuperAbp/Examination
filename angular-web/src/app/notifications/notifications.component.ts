import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NotificationService } from '@proxy/notifications';
import { NotificationListDto } from '@proxy/notifications/models';
import { GetNotificationsInput } from '@proxy/admin/notifications/models';
import { CoreModule } from '@abp/ng.core';

@Component({
  selector: 'app-notifications',
  templateUrl: './notifications.component.html',
  imports: [CommonModule, CoreModule, NgbPaginationModule],
  standalone: true,
})
export class NotificationsComponent implements OnInit {
  private notificationService = inject(NotificationService);

  notifications: NotificationListDto[] = [];
  loading = false;
  page = 1;
  totalCount = 0;
  input: GetNotificationsInput = {
    skipCount: 0,
    maxResultCount: 10,
    isRead: false,
  };

  ngOnInit() {
    this.loadNotifications();
  }

  loadNotifications(page: number = 1) {
    this.loading = true;
    this.page = page;
    this.input.skipCount = (page - 1) * this.input.maxResultCount;

    this.notificationService.getList(this.input).subscribe(result => {
      this.notifications = result.items || [];
      this.totalCount = result.totalCount || 0;
      this.loading = false;
    });
  }

  onFilterChange(isRead: boolean) {
    if (this.input.isRead === isRead) return;
    this.input.isRead = isRead;
    this.loadNotifications(1);
  }

  onNotificationClick(notification: NotificationListDto) {
    this.markAsRead(notification);
  }

  loadPage(page: number) {
    this.loadNotifications(page);
  }

  markAsRead(notification: NotificationListDto) {
    if (notification.isRead) {
      return;
    }

    this.notificationService.markAsRead(notification.id).subscribe(() => {
      this.notifications = this.notifications.filter(n => n.id !== notification.id);
      this.totalCount--;
    });
  }
  markAllAsRead() {
    this.notificationService
      .markAllAsRead()
      .subscribe(() => ((this.notifications = []), (this.totalCount = 0)));
  }
  getNotificationTitle(type: number): string {
    const titles: Record<number, string> = {
      1: '考试提醒',
      2: '成绩公布',
      3: '系统通知',
    };
    return titles[type] || '通知';
  }

  getNotificationContent(notification: NotificationListDto): string {
    if (!notification.data) {
      return '您有一条新通知';
    }

    try {
      const data = JSON.parse(notification.data);

      if (notification.type === 2) {
        // TODO: Use Template to display content;
        return `您参加的《${data.examName || '考试'}》成绩已发布`;
      }

      return data.message || '您有一条新通知';
    } catch {
      return '您有一条新通知';
    }
  }
}
