import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { DynamicLayoutComponent } from '@abp/ng.core';
import { LoaderBarComponent, NavItemsService } from '@abp/ng.theme.shared';
import { NotificationIconComponent } from './shared/components/notification-icon';
import { NotificationHubService } from './shared/services/notification-hub.service';

@Component({
  selector: 'app-root',
  template: `
    <abp-loader-bar />
    <abp-dynamic-layout />
  `,
  imports: [LoaderBarComponent, DynamicLayoutComponent],
})
export class AppComponent implements OnInit, OnDestroy {
  private navItems = inject(NavItemsService);
  private notificationHub = inject(NotificationHubService);

  ngOnInit(): void {
    this.navItems.addItems([
      {
        id: 'NotificationIcon',
        order: 99,
        component: NotificationIconComponent,
      },
    ]);

    this.notificationHub.startConnection();
  }

  ngOnDestroy(): void {
    this.notificationHub.stopConnection();
  }
}
