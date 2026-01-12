import { AuthService, CoreModule } from '@abp/ng.core';
import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import { DA_SERVICE_TOKEN, ITokenService } from '@delon/auth';
import { I18nPipe, SettingsService, User } from '@delon/theme';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { NzDropDownModule } from 'ng-zorro-antd/dropdown';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { tap } from 'rxjs';

@Component({
  selector: 'header-user',
  template: `
    <div class="alain-default__nav-item d-flex align-items-center px-sm" nz-dropdown nzPlacement="bottomRight" [nzDropdownMenu]="userMenu">
      <nz-avatar [nzSrc]="user.avatar" nzSize="small" class="mr-sm"></nz-avatar>
      {{ user.name }}
    </div>
    <nz-dropdown-menu #userMenu="nzDropdownMenu">
      <div nz-menu class="width-sm">
        <div nz-menu-item routerLink="/account/settings">
          <i nz-icon nzType="setting" class="mr-sm"></i>
          {{ 'AbpAccount::MyAccount' | abpLocalization }}
        </div>
        <li nz-menu-divider></li>
        <div nz-menu-item (click)="logout()">
          <i nz-icon nzType="logout" class="mr-sm"></i>
          {{ 'AbpAccount::Logout' | abpLocalization }}
        </div>
      </div>
    </nz-dropdown-menu>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [NzDropDownModule, NzMenuModule, NzIconModule, NzAvatarModule, CoreModule]
})
export class HeaderUserComponent {
  get user(): User {
    return this.settings.user;
  }

  constructor(
    private authService: AuthService,
    private settings: SettingsService,
    @Inject(DA_SERVICE_TOKEN) private tokenService: ITokenService
  ) {}

  logout(): void {
    this.authService
      .logout()
      .pipe(
        tap(() => {
          this.tokenService.clear();
        })
      )
      .subscribe();
  }
}
