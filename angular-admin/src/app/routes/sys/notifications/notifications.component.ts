import { Component, inject, OnInit, ViewChild, ChangeDetectorRef } from '@angular/core';
import { STComponent, STColumn, STData, STModule, STChange, STPage } from '@delon/abc/st';
import { DelonFormModule, SFSchema } from '@delon/form';
import { NotificationService } from '@proxy/admin/controllers';
import type { GetMyNotificationsInput, NotificationMyListDto } from '@proxy/admin/notifications/models';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCardModule } from 'ng-zorro-antd/card';
import { finalize } from 'rxjs/operators';
import { CoreModule, LocalizationService } from '@abp/ng.core';

@Component({
  selector: 'app-sys-notifications',
  templateUrl: './notifications.component.html',
  standalone: true,
  imports: [CoreModule, DelonFormModule, STModule, NzCardModule, NzButtonModule]
})
export class SysNotificationsComponent implements OnInit {
  private notificationService = inject(NotificationService);
  private message = inject(NzMessageService);
  private localizationService = inject(LocalizationService);
  private cdr = inject(ChangeDetectorRef);

  loading = false;
  notifications: NotificationMyListDto[] = [];
  total = 0;
  currentPage = 1;
  pageSize = 10;
  unreadCount = 0;
  params: GetMyNotificationsInput;

  searchSchema: SFSchema = {
    properties: {
      filter: {
        type: 'string',
        title: '',
        ui: {
          placeholder: this.localizationService.instant('Exam::Placeholder', this.localizationService.instant('Exam::Title'))
        }
      },
      isRead: {
        type: 'boolean',
        title: '',
        ui: {
          widget: 'select'
        },
        enum: [
          { label: this.localizationService.instant('::Unread'), value: false },
          { label: this.localizationService.instant('::Read'), value: true }
        ],
        default: false
      }
    }
  };

  page: STPage = {
    show: true,
    showSize: true,
    front: false,
    pageSizes: [10, 20, 30, 40, 50]
  };

  @ViewChild('st', { static: false }) st: STComponent;
  columns: STColumn[] = [
    {
      title: this.localizationService.instant('Exam::Content'),
      index: 'data',
      format: (item: STData) => this.getNotificationContent(item),
      className: 'text-wrap'
    },
    {
      title: this.localizationService.instant('Exam::CreationTime'),
      index: 'creationTime',
      type: 'date',
      width: 180
    },
    {
      title: this.localizationService.instant('Exam::Actions'),
      buttons: [
        {
          text: this.localizationService.instant('Exam::MarkAsRead'),
          iif: (item: STData) => !item['isRead'],
          pop: {
            title: this.localizationService.instant('Exam::MarkThisNotificationAsRead'),
            okType: 'primary'
          },
          click: (item: STData) => this.markAsRead(item)
        }
      ],
      width: 150,
      fixed: 'right'
    }
  ];

  ngOnInit() {
    this.params = this.resetParameters();
    this.getList();
    this.loadUnreadCount();
  }

  resetParameters(): GetMyNotificationsInput {
    return {
      skipCount: 0,
      maxResultCount: 10,
      isRead: false
    };
  }

  change(e: STChange) {
    if (e.type === 'pi' || e.type === 'ps') {
      this.params.skipCount = (e.pi - 1) * e.ps;
      this.params.maxResultCount = e.ps;
      this.getList();
    }
  }

  search(e: any) {
    if (e.filter) {
      this.params.filter = e.filter;
    } else {
      delete this.params.filter;
    }
    this.params.isRead = e.isRead;
    this.st.load(1);
  }

  reset() {
    this.params = this.resetParameters();
    this.st.load(1);
  }

  getList() {
    this.loading = true;
    this.notificationService
      .getMyList(this.params)
      .pipe(
        finalize(() => {
          this.loading = false;
          this.cdr.detectChanges();
        })
      )
      .subscribe({
        next: res => {
          this.notifications = res.items || [];
          this.total = res.totalCount || 0;
          this.cdr.detectChanges();
        },
        error: err => {
          console.error('Failed to load notifications:', err);
          this.message.error('Load Failed');
        }
      });
  }

  loadUnreadCount() {
    this.notificationService.getUnreadCount().subscribe({
      next: count => {
        this.unreadCount = count;
        this.cdr.detectChanges();
      }
    });
  }

  markAsRead(item: STData) {
    const notification = item as NotificationMyListDto;
    this.notificationService.markAsRead(notification.id).subscribe({
      next: () => {
        notification.isRead = true;
        this.unreadCount--;
        this.message.success('Marked as read');
        this.cdr.detectChanges();
        this.getList();
      },
      error: err => {
        console.error('Failed to mark as read:', err);
        this.message.error('Operation Failed');
      }
    });
  }

  markAllAsRead() {
    this.notificationService.markAllAsRead().subscribe(() => {
      this.notifications.forEach(n => (n.isRead = true));
      this.unreadCount = 0;
      this.cdr.detectChanges();
    });
  }

  getNotificationContent(notification): string {
    const data = JSON.parse(notification.data);
    if (notification.type === 2) {
      // TODO：Use Template to display content
      return `您参加的考试《${data.examName ?? ''}》已出分`;
    }
    return data.message;
  }
}
