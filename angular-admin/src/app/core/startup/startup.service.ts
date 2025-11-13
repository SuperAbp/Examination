import { Injectable, Inject, Provider, APP_INITIALIZER, Injector } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { DA_SERVICE_TOKEN, ITokenService } from '@delon/auth';
import { MenuService, SettingsService, TitleService } from '@delon/theme';
import { ACLService } from '@delon/acl';
import { Observable, of, catchError, map } from 'rxjs';
import type { NzSafeAny } from 'ng-zorro-antd/core/types';
import { NzIconService } from 'ng-zorro-antd/icon';

import { ICONS } from '../../../style-icons';
import { ICONS_AUTO } from '../../../style-icons-auto';
import { AppService } from '@proxy/admin/controllers';
import { log } from '@delon/util';

/**
 * Used for application startup
 * Generally used to get the basic data of the application, like: Menu Data, User Data, etc.
 */
export function provideStartup(): Provider[] {
  return [
    StartupService,
    {
      provide: APP_INITIALIZER,
      useFactory: (startupService: StartupService) => () => startupService.load(),
      deps: [StartupService],
      multi: true
    }
  ];
}
@Injectable()
export class StartupService {
  constructor(
    iconSrv: NzIconService,
    private menuService: MenuService,
    private settingService: SettingsService,
    private aclService: ACLService,
    private titleService: TitleService,
    @Inject(DA_SERVICE_TOKEN) private tokenService: ITokenService,
    private injector: Injector,
    private appService: AppService,
    private httpClient: HttpClient,
    private router: Router
  ) {
    iconSrv.addIcon(...ICONS_AUTO, ...ICONS);
  }

  private viaHttp(): Observable<void> {
    var tokenService = this.injector.get(DA_SERVICE_TOKEN) as ITokenService;
    if (tokenService.get().token !== undefined && tokenService.get().token !== null && tokenService.get().token !== '') {
      return this.appService.getData().pipe(
        // catchError(res => {
        //   debugger;
        //   console.warn(`StartupService.load: Network request failed`, res);
        //   return of({});
        // }),
        map(appData => {
          // Application data
          const res: any = appData;
          // Application information: including site name, description, year
          this.settingService.setApp(res.app);
          // User information: including name, avatar, email address
          this.settingService.setUser(res.user);
          // ACL: Set the permissions to full, https://ng-alain.com/acl/getting-started
          this.aclService.setFull(true);
          // Menu data, https://ng-alain.com/theme/menu
          let menus = res.menu.map((item: NzSafeAny) => {
            item.children = item.children?.map((child: NzSafeAny) => {
              if (child.icon) {
                child.icon = { type: 'icon', value: child.icon };
              }
              return child;
            });
            return item;
          });
          this.menuService.add(menus);
          // Can be set page suffix title, https://ng-alain.com/theme/title
          this.titleService.suffix = res.app.name;
        })
      );
    } else {
      return of();
    }
  }

  private viaMock(): Observable<void> {
    // const tokenData = this.tokenService.get();
    // if (!tokenData.token) {
    //   this.router.navigateByUrl(this.tokenService.login_url!);
    //   return;
    // }
    // mock
    const app: any = {
      name: `Exam`,
      description: `Ng-zorro admin panel front-end framework`
    };
    const user: any = {
      name: 'Admin',
      avatar: './assets/tmp/img/avatar.jpg',
      email: '1007215202@qq.com',
      token: '123456789'
    };
    // Application information: including site name, description, year
    this.settingService.setApp(app);
    // User information: including name, avatar, email address
    this.settingService.setUser(user);
    // ACL: Set the permissions to full, https://ng-alain.com/acl/getting-started
    this.aclService.setFull(true);
    // Menu data, https://ng-alain.com/theme/menu
    this.menuService.add([
      {
        text: 'Main',
        group: true,
        children: [
          {
            text: 'Dashboard',
            link: '/dashboard',
            icon: { type: 'icon', value: 'appstore' }
          },
          {
            text: '系统管理',
            icon: { type: 'icon', value: 'setting' },
            children: [
              {
                text: '知识点',
                link: '/sys/knowledge-point'
              }
            ]
          },
          {
            text: '权限管理',
            icon: { type: 'icon', value: 'verified' },
            children: [
              {
                text: '用户管理',
                link: '/identity/user'
              },
              {
                text: '租户管理',
                link: '/tenant-management/tenant'
              },
              {
                text: '角色管理',
                link: '/identity/role'
              },
              {
                text: '菜单管理',
                link: '/menu-management/menu'
              }
            ]
          },
          {
            text: '考试管理',
            icon: { type: 'icon', value: 'appstore' },
            children: [
              {
                text: '题库管理',
                link: '/question-management/question-bank'
              },
              {
                text: '问题管理',
                link: '/question-management/question'
              },
              {
                text: '试卷管理',
                link: '/paper-management/paper'
              },
              {
                text: '考试管理',
                link: '/exam-management/exam'
              }
            ]
          }
        ]
      }
    ]);
    // Can be set page suffix title, https://ng-alain.com/theme/title
    this.titleService.suffix = app.name;

    return of(void 0);
  }

  load(): Observable<void> {
    // http
    return this.viaHttp();
    // mock: Don’t use it in a production environment. ViaMock is just to simulate some data to make the scaffolding work normally
    // mock：请勿在生产环境中这么使用，viaMock 单纯只是为了模拟一些数据使脚手架一开始能正常运行
    // return this.viaMock();
  }
}
