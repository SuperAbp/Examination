import { ChangeDetectionStrategy, ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { SHARED_IMPORTS } from '@shared';
import { NzListModule } from 'ng-zorro-antd/list';
import { NzMessageService } from 'ng-zorro-antd/message';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { ChangePasswordInput, ProfileService } from '@proxy/volo/abp/account';
import { CoreModule, LocalizationService } from '@abp/ng.core';

@Component({
  selector: 'app-account-settings-password',
  templateUrl: './password.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [...SHARED_IMPORTS, NzListModule, NzFormModule, NzInputModule, ReactiveFormsModule, CoreModule]
})
export class AccountSettingsPasswordComponent implements OnInit {
  readonly msg = inject(NzMessageService);
  private readonly fb = inject(FormBuilder);
  private readonly profileService = inject(ProfileService);
  private readonly cdr = inject(ChangeDetectorRef);
  private readonly localizationService = inject(LocalizationService);

  passwordForm!: FormGroup;

  ngOnInit(): void {
    this.initForm();
  }

  private initForm(): void {
    this.passwordForm = this.fb.group(
      {
        currentPassword: ['', [Validators.required]],
        newPassword: ['', [Validators.required, Validators.minLength(6)]],
        confirmPassword: ['', [Validators.required]]
      },
      { validators: this.passwordMatchValidator }
    );
  }

  private passwordMatchValidator(control: AbstractControl): ValidationErrors | null {
    const newPassword = control.get('newPassword')?.value;
    const confirmPassword = control.get('confirmPassword')?.value;

    if (newPassword && confirmPassword && newPassword !== confirmPassword) {
      return { passwordMismatch: true };
    }
    return null;
  }

  save(): void {
    if (this.passwordForm.invalid) {
      Object.values(this.passwordForm.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
      return;
    }

    const input: ChangePasswordInput = {
      currentPassword: this.passwordForm.value.currentPassword,
      newPassword: this.passwordForm.value.newPassword
    };

    this.profileService.changePassword(input).subscribe(() => {
      this.passwordForm.reset();
      this.cdr.markForCheck();
      this.msg.success(this.localizationService.instant('AbpAccount::PasswordChangedMessage'));
    });
  }
}
