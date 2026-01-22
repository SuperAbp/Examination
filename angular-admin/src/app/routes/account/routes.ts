import { Routes } from '@angular/router';

import { AccountSettingsBaseComponent } from './settings/base/base.component';
import { AccountSettingsComponent } from './settings/settings.component';
import { AccountSettingsPasswordComponent } from './settings/password/password.component';

export const routes: Routes = [
  {
    path: 'settings',
    component: AccountSettingsComponent,
    children: [
      { path: '', redirectTo: 'base', pathMatch: 'full' },
      {
        path: 'base',
        component: AccountSettingsBaseComponent,
        data: { titleI18n: 'account.personalinfo' }
      },
      {
        path: 'password',
        component: AccountSettingsPasswordComponent,
        data: { titleI18n: 'account.password' }
      }
    ]
  }
];
