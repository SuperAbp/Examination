import { AuthService } from '@abp/ng.core';
import { ChangeDetectionStrategy, Component, Inject, OnDestroy, OnInit, inject } from '@angular/core';
import { Router } from '@angular/router';
import { StartupService } from '@core';
import { DA_SERVICE_TOKEN, ITokenService, SocialService } from '@delon/auth';
import { _HttpClient } from '@delon/theme';
import { OAuthService } from 'angular-oauth2-oidc';

@Component({
  selector: 'passport-login',
  template: '',
  providers: [SocialService],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserLoginComponent implements OnInit {
  private router = inject(Router);
  private tokenService = inject(DA_SERVICE_TOKEN);
  private startupSrv = inject(StartupService);
  private oAuthService = inject(OAuthService);
  private authService = inject(AuthService);
  get hasLoggedIn(): boolean {
    return this.oAuthService.hasValidAccessToken();
  }
  ngOnInit(): void {
    if (this.hasLoggedIn) {
      this.tokenService.set({
        token: this.oAuthService.getAccessToken(),
        expired: this.oAuthService.getAccessTokenExpiration()
      });
      this.startupSrv.load().subscribe(() => {
        let url = this.tokenService.referrer?.url || '/';
        if (url.includes('/passport')) {
          url = '/';
        }
        this.router.navigateByUrl(url);
      });
    } else {
      this.authService.navigateToLogin();
    }
  }
}
