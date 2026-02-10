import { permissionGuard } from '@abp/ng.core';
import { Routes } from '@angular/router';
import { authJWTCanActivate } from '@delon/auth';

import { SysKnowledgePointComponent } from './knowledge-point/knowledge-point.component';
import { SysAnnouncementComponent } from './announcement/announcement.component';
import { SysAnnouncementCategoryComponent } from './announcement-category/announcement-category.component';

export const routes: Routes = [
  {
    path: 'knowledge-point',
    component: SysKnowledgePointComponent,
    canActivate: [authJWTCanActivate, permissionGuard],
    data: {
      requiredPolicy: 'Exam.KnowledgePoints.Management'
    }
  },
  {
    path: 'announcement',
    component: SysAnnouncementComponent,
    canActivate: [authJWTCanActivate, permissionGuard],
    data: {
      requiredPolicy: 'Exam.Announcements'
    }
  },
  {
    path: 'announcement-category',
    component: SysAnnouncementCategoryComponent,
    canActivate: [authJWTCanActivate, permissionGuard],
    data: {
      requiredPolicy: 'Exam.AnnouncementCategories'
    }
  }
];
