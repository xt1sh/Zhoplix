import { Component, OnInit, AfterViewInit, ChangeDetectorRef, NgZone } from '@angular/core';
import { AuthenticationService } from 'src/app/services/authentication/authentication.service';
import { ProfileService } from 'src/app/services/profile/profile.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, Validators, FormGroup, ValidationErrors, FormControl, AbstractControl } from '@angular/forms';
import { TokenPasswordReset } from 'src/app/models/token-password-reset';
import { TouchSequence } from 'selenium-webdriver';
import { MatSnackBar } from '@angular/material';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent implements OnInit {

  newPasswordForm = this.formBuilder.group({
    password: [null, [Validators.required, Validators.pattern('((?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{6,30})')]],
    passwordConfirmation: [null, [Validators.required]],
    signOutOfAll: [false]
  }, {validator: this.passwordMismatch});

  code: string;
  userId: string;
  token: string;
  isValid: boolean;
  loading: boolean;

  constructor(private readonly auth: AuthenticationService,
              private readonly profile: ProfileService,
              private readonly route: ActivatedRoute,
              private readonly formBuilder:FormBuilder,
              private cdRef:ChangeDetectorRef,
              private readonly ngZone: NgZone,
              private readonly router: Router,
              private readonly snack: MatSnackBar) { 
                
              }

  ngOnInit() {
    this.loading = true;
    this.route.queryParams.subscribe(params => {
      this.userId = params['userId'];
      this.code = params['code'];
      this.token = params['token'];
      });
    if (this.isEmptyOrSpaces(this.userId) || this.isEmptyOrSpaces(this.code) || this.isEmptyOrSpaces(this.token)) {
      this.isValid = false;
      } 
    else {
        const sub = this.auth.createFingerprint().subscribe(value => {
          this.auth.verifyPasswordResetCode(this.userId, this.code, value).subscribe(res => {
            this.isValid = true;
            this.auth.setTokens(res);
            this.loading = false;
            this.cdRef.detectChanges();
            sub.unsubscribe();
          }, error  => {
            this.isValid = false;
            this.loading = false;
            this.cdRef.detectChanges();
            sub.unsubscribe();
          });
        });
      }

  }

  isEmptyOrSpaces(str: string){
    return str === null || str.match(/^ *$/) !== null;
  }

  passwordMismatch(control: AbstractControl): ValidationErrors | null {
    const password = control.get('password');
    const passwordConfirmation = control.get('passwordConfirmation');
    if (!password || !passwordConfirmation) return null;
    return password.value === passwordConfirmation.value ? null : {'passwordMismatch': true}; 
  } 

  onSubmit() {
    let model = Object.assign( new TokenPasswordReset(), {
      token: this.token,
      password: this.newPasswordForm.controls['password'].value,
      userId: this.userId,
      fingerprint: this.auth.fingerPrint,
      signOutOfAll: this.newPasswordForm.controls['signOutOfAll'].value
    });
    this.profile.changePasswordWithToken(model).subscribe(res => {
      this.ngZone.run(() => this.router.navigate(['profile']));
      this.snack.open(`Password was successfully changed`,
        'OK', {duration: 3000, panelClass: ['snack-success']});
    }, error => {
      this.snack.open(error.error[0].description,
        'OK', {duration: 3000, panelClass: ['snack-error']});
      this.newPasswordForm.controls['password'].reset();
      this.newPasswordForm.controls['passwordConfirmation'].reset();
    });
  }

}
