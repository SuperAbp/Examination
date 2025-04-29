import { permissionGuard } from '@abp/ng.core';
import { Routes } from '@angular/router';
import { authJWTCanActivate } from '@delon/auth';

import { SysKnowledgePointComponent } from './knowledge-point/knowledge-point.component';

export const routes: Routes = [
  {
    path: 'knowledge-point',
    component: SysKnowledgePointComponent,
    canActivate: [authJWTCanActivate, permissionGuard],
    data: {
      requiredPolicy: 'Exam.KnowledgePoint.Management'
    }
  }
];
