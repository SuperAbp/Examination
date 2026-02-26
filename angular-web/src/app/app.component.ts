import { Component, inject } from '@angular/core';
import { DynamicLayoutComponent } from '@abp/ng.core';
import { LoaderBarComponent, NavItemsService } from '@abp/ng.theme.shared';
import { NotificationIconComponent } from './shared/components/notification-icon';

@Component({
  selector: 'app-root',
  template: `
    <abp-loader-bar />
    <abp-dynamic-layout />
  `,
  imports: [LoaderBarComponent, DynamicLayoutComponent],
})
export class AppComponent {
  private navItems = inject(NavItemsService);

  constructor() {
    this.navItems.addItems([
      {
        id: 'NotificationIcon',
        order: 99,
        component: NotificationIconComponent,
      },
    ]);
  }
}
