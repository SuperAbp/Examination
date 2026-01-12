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
        component: AccountSettingsBaseComponent
      },
      {
        path: 'password',
        component: AccountSettingsPasswordComponent
      }
    ]
  }
];
