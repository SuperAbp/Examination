import { Component, inject, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NotificationService } from '@proxy/admin/controllers';
import { NzBadgeModule } from 'ng-zorro-antd/badge';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'header-notification',
  template: `
    <a class="alain-default__nav-item" routerLink="/sys/notifications">
      <nz-badge [nzCount]="unreadCount" [nzOverflowCount]="99" [nzOffset]="[2, -8]">
        <nz-icon nzType="bell" class="alain-default__nav-item-icon" />
      </nz-badge>
    </a>
  `,
  imports: [CommonModule, NzBadgeModule, NzIconModule, RouterLink]
})
export class HeaderNotificationComponent implements OnInit {
  private readonly notificationService = inject(NotificationService);
  private readonly cdr = inject(ChangeDetectorRef);

  unreadCount = 0;

  ngOnInit() {
    this.loadUnreadCount();
    this.setupRealtimeNotifications();
  }

  loadUnreadCount() {
    this.notificationService.getUnreadCount().subscribe(count => {
      this.unreadCount = count;
      this.cdr.detectChanges();
    });
  }

  private setupRealtimeNotifications() {}
}
