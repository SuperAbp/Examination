import { Component, inject, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { NotificationService } from '@proxy/notifications';

@Component({
  selector: 'app-notification-icon',
  template: `
    <a routerLink="/notifications" class="nav-link">
      <span class="position-relative">
        <i class="fa fa-bell"></i>
        @if (unreadCount > 0) {
          <span
            class="badge bg-danger position-absolute top-0 start-100 translate-middle"
            style="font-size: .5rem; padding: 0.25rem 0.25rem;"
          >
            {{ unreadCount > 99 ? '99+' : unreadCount }}
          </span>
        }
      </span>
    </a>
  `,
  standalone: true,
  imports: [RouterLink],
})
export class NotificationIconComponent implements OnInit {
  private notificationService = inject(NotificationService);

  unreadCount = 0;

  ngOnInit(): void {
    this.loadUnreadCount();
  }

  private loadUnreadCount(): void {
    this.notificationService.getUnreadCount().subscribe(count => {
      this.unreadCount = count;
    });
  }
}
