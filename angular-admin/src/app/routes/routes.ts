import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { startPageGuard } from '@core';
import { authSimpleCanActivate } from '@delon/auth';
import { environment } from '@env/environment';
// layout
import { LayoutBasicComponent } from '../layout/basic/basic.component';
import { LayoutPassportComponent } from '../layout/passport/passport.component';
// dashboard pages
import { DashboardComponent } from './dashboard/dashboard.component';
// single pages
import { CallbackComponent } from './passport/callback.component';
import { UserLockComponent } from './passport/lock/lock.component';
// passport pages
import { UserLoginComponent } from './passport/login/login.component';

export const routes: Routes = [
  {
    path: '',
    component: LayoutBasicComponent,
    canActivate: [startPageGuard, authSimpleCanActivate],
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: DashboardComponent, data: { title: '仪表盘', titleI18n: 'dashboard' } },
      { path: 'exception', loadChildren: () => import('./exception/exception.module').then(m => m.ExceptionModule) },
      // 业务子模块
      // { path: 'widgets', loadChildren: () => import('./widgets/widgets.module').then(m => m.WidgetsModule) },
      {
        path: 'identity',
        loadChildren: () => import('@super-abp/ng.identity').then(m => m.IdentityModule.forLazy())
      },
      {
        path: 'tenant-management',
        loadChildren: () => import('@super-abp/ng.tenant-management').then(m => m.routes)
      },
      {
        path: 'menu-management',
        loadChildren: () => import('@super-abp/ng.menu-management').then(m => m.routes)
      },
      {
        path: 'question-management',
        loadChildren: () => import('./question-management/routes').then(m => m.routes)
      },
      {
        path: 'paper-management',
        loadChildren: () => import('./paper-management/routes').then(m => m.routes)
      },
      {
        path: 'exam-management',
        loadChildren: () => import('./exam-management/routes').then(m => m.routes)
      },
      { path: 'sys', loadChildren: () => import('./sys/routes').then(m => m.routes) }
    ]
  },
  // 空白布局
  // {
  //     path: 'blank',
  //     component: LayoutBlankComponent,
  //     children: [
  //     ]
  // },
  // passport
  {
    path: 'passport',
    component: LayoutPassportComponent,
    children: [
      { path: 'login', component: UserLoginComponent, data: { title: '登录', titleI18n: 'pro-login' } },
      { path: 'lock', component: UserLockComponent, data: { title: '锁屏', titleI18n: 'lock' } }
    ]
  },
  // 单页不包裹Layout
  { path: 'passport/callback/:type', component: CallbackComponent },
  { path: '**', redirectTo: 'exception/404' }
];
