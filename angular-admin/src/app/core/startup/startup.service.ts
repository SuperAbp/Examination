import { Injectable, Inject, Provider, Injector, EnvironmentProviders, provideAppInitializer, inject } from '@angular/core';
import { DA_SERVICE_TOKEN, ITokenService } from '@delon/auth';
import { ALAIN_I18N_TOKEN, MenuService, SettingsService, TitleService } from '@delon/theme';
import { ACLService } from '@delon/acl';
import { I18NService } from '../i18n/i18n.service';
import { Observable, of, map, filter, take, switchMap } from 'rxjs';
import type { NzSafeAny } from 'ng-zorro-antd/core/types';
import { NzIconService } from 'ng-zorro-antd/icon';

import { ICONS } from '../../../style-icons';
import { ICONS_AUTO } from '../../../style-icons-auto';
import { AppService } from '@proxy/admin/controllers';
import { ConfigStateService, provideAbpCore, withOptions } from '@abp/ng.core';
import { environment } from '@env/environment';
import { registerLocale } from '@abp/ng.core/locale';

/**
 * Used for application startup
 * Generally used to get the basic data of the application, like: Menu Data, User Data, etc.
 */
export function provideStartup(): Array<Provider | EnvironmentProviders> {
  return [
    provideAbpCore(
      withOptions({
        environment,
        registerLocaleFn: registerLocale()
      })
    ),
    StartupService,
    provideAppInitializer(() => {
      const initializerFn = (
        (startupService: StartupService) => () =>
          startupService.load()
      )(inject(StartupService));
      return initializerFn();
    })
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
    @Inject(ALAIN_I18N_TOKEN) private i18n: I18NService,
    private injector: Injector,
    private appService: AppService
  ) {
    iconSrv.addIcon(...ICONS_AUTO, ...ICONS);
  }

  private viaHttp(): Observable<void> {
    var tokenService = this.injector.get(DA_SERVICE_TOKEN) as ITokenService;

    // Initialize i18n
    const defaultLang = this.i18n.defaultLang;
    this.i18n.loadLangData(defaultLang).subscribe(langData => {
      this.i18n.use(defaultLang, langData);
    });

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

  load(): Observable<void> {
    var configState = this.injector.get(ConfigStateService);
    return configState.getOne$('currentUser').pipe(
      filter(user => user !== undefined && user !== null),
      take(1),
      switchMap(() => {
        return this.viaHttp();
      })
    );
    // http

    // mock: Don’t use it in a production environment. ViaMock is just to simulate some data to make the scaffolding work normally
    // mock：请勿在生产环境中这么使用，viaMock 单纯只是为了模拟一些数据使脚手架一开始能正常运行
    // return this.viaMock();
  }
}
