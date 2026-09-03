import { Component, OnDestroy, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { NgxOtpInputComponent, OtpStatus } from 'ngx-otp-input';
import { AuthService } from '../../services/auth.service';
import { AppPageShellComponent } from '../app-page-shell/app-page-shell.component';
import { AppPageHeaderComponent } from '../app-page-header/app-page-header.component';
import { PageStatusComponent } from '../page-status/page-status.component';
import { FooterActionComponent } from '../footer-action/footer-action.component';
import { getApiErrorMessage } from '../../interceptors/api-error';

type LoginStep = 'credentials' | 'otp';

/** Minimal typing for the Web OTP API (Chrome Android). */
interface OtpCredential extends Credential {
  code: string;
}

interface OtpCredentialRequestOptions extends CredentialRequestOptions {
  otp?: { transport: Array<'sms'> };
  signal?: AbortSignal;
}

@Component({
  selector: 'app-login',
  imports: [
    ReactiveFormsModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    NgxOtpInputComponent,
    AppPageShellComponent,
    AppPageHeaderComponent,
    PageStatusComponent,
    FooterActionComponent
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnDestroy {
  private readonly fb = inject(FormBuilder);
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);

  private otpAbort: AbortController | null = null;

  readonly step = signal<LoginStep>('credentials');
  readonly loading = signal(false);
  readonly error = signal<string | null>(null);
  readonly hidePassword = signal(true);
  readonly otpStatus = signal<OtpStatus>('idle');

  readonly credentialsForm = this.fb.nonNullable.group({
    userName: ['', Validators.required],
    password: ['', Validators.required]
  });

  readonly otpForm = this.fb.nonNullable.group({
    code: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(6)]]
  });

  constructor() {
    if (this.auth.isAuthenticated()) {
      void this.router.navigateByUrl('/');
    }
  }

  ngOnDestroy(): void {
    this.stopWebOtpAutofill();
  }

  submitCredentials(): void {
    this.error.set(null);
    if (this.credentialsForm.invalid) {
      this.credentialsForm.markAllAsTouched();
      return;
    }

    const { userName, password } = this.credentialsForm.getRawValue();
    this.loading.set(true);
    this.auth.login(userName, password).subscribe({
      next: res => {
        this.loading.set(false);
        if (!res.requiresOtp) {
          this.error.set(res.message?.trim() || 'שם משתמש או סיסמה שגויים');
          return;
        }

        this.otpForm.reset({ code: '' });
        this.otpStatus.set('idle');
        this.step.set('otp');
        this.startWebOtpAutofill();
      },
      error: err => {
        this.loading.set(false);
        this.error.set(getApiErrorMessage(err));
      }
    });
  }

  submitOtp(): void {
    this.error.set(null);
    if (this.otpForm.invalid) {
      this.otpForm.markAllAsTouched();
      this.otpStatus.set('error');
      return;
    }

    this.stopWebOtpAutofill();

    const userName = this.credentialsForm.controls.userName.value;
    const code = this.otpForm.controls.code.value;
    this.loading.set(true);
    this.auth.verify(userName, code).subscribe({
      next: () => {
        this.loading.set(false);
        this.otpStatus.set('success');
        void this.router.navigateByUrl('/');
      },
      error: err => {
        this.loading.set(false);
        this.otpStatus.set('error');
        this.error.set(getApiErrorMessage(err));
      }
    });
  }

  onOtpComplete(code: string): void {
    this.otpForm.controls.code.setValue(code);
    this.submitOtp();
  }

  backToCredentials(): void {
    this.stopWebOtpAutofill();
    this.step.set('credentials');
    this.error.set(null);
    this.otpStatus.set('idle');
    this.otpForm.reset({ code: '' });
  }

  private startWebOtpAutofill(): void {
    if (!('OTPCredential' in window) || !navigator.credentials?.get) {
      return;
    }

    this.stopWebOtpAutofill();
    this.otpAbort = new AbortController();

    const options: OtpCredentialRequestOptions = {
      otp: { transport: ['sms'] },
      signal: this.otpAbort.signal
    };

    void navigator.credentials
      .get(options)
      .then(cred => {
        const code = (cred as OtpCredential | null)?.code?.trim();
        if (code && code.length === 6) {
          this.otpForm.controls.code.setValue(code);
          this.submitOtp();
        }
      })
      .catch(() => {
        // Cancelled, timed out, or unsupported — user can enter manually.
      });
  }

  private stopWebOtpAutofill(): void {
    this.otpAbort?.abort();
    this.otpAbort = null;
  }
}
