import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit, inject } from '@angular/core';
import { _HttpClient } from '@delon/theme';
import { SHARED_IMPORTS } from '@shared';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzUploadComponent } from 'ng-zorro-antd/upload';
import { zip } from 'rxjs';
import { ProfileDto, ProfileService, UpdateProfileDto } from '@proxy/volo/abp/account';
import { CoreModule, LocalizationService } from '@abp/ng.core';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzInputModule } from 'ng-zorro-antd/input';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

interface AccountSettingsUser {
  email: string;
  name: string;
  profile: string;
  country: string;
  address: string;
  phone: string;
  avatar: string;
  geographic: {
    province: {
      key: string;
    };
    city: {
      key: string;
    };
  };
}

@Component({
  selector: 'app-account-settings-base',
  templateUrl: './base.component.html',
  styleUrls: ['./base.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [...SHARED_IMPORTS, CoreModule, NzFormModule, NzGridModule, NzInputModule, ReactiveFormsModule]
})
export class AccountSettingsBaseComponent implements OnInit {
  private readonly http = inject(_HttpClient);
  private readonly cdr = inject(ChangeDetectorRef);
  private readonly msg = inject(NzMessageService);
  private readonly profileService = inject(ProfileService);
  private readonly fb = inject(FormBuilder);
  private readonly localizationService = inject(LocalizationService);

  user?: ProfileDto;
  form!: FormGroup;
  avatar = '';
  userLoading = true;

  ngOnInit(): void {
    this.initForm();
    this.profileService.get().subscribe(profile => {
      this.user = profile;
      this.form.patchValue({
        userName: profile.userName,
        name: profile.name,
        surname: profile.surname,
        email: profile.email,
        phoneNumber: profile.phoneNumber
      });
      this.userLoading = false;
      this.cdr.markForCheck();
    });
  }

  private initForm(): void {
    this.form = this.fb.group({
      userName: ['', [Validators.required]],
      name: [''],
      surname: [''],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['']
    });
  }

  save(): void {
    if (this.form.invalid) {
      Object.values(this.form.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
      return;
    }

    const updateDto: UpdateProfileDto = {
      ...this.form.value,
      concurrencyStamp: this.user?.concurrencyStamp
    };

    this.profileService.update(updateDto).subscribe(() => {
      this.msg.success(this.localizationService.instant('AbpAccount::PersonalSettingsSaved'));
    });
  }
}
