import { Component, OnInit, AfterViewInit, ChangeDetectorRef } from '@angular/core';
import { AuthenticationService } from 'src/app/services/authentication/authentication.service';
import { ProfileService } from 'src/app/services/profile/profile.service';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, Validators, FormGroup, ValidationErrors } from '@angular/forms';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent implements OnInit, AfterViewInit {

  newPasswordForm = this.formBuilder.group({
    password: [undefined, [Validators.required, Validators.pattern('((?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{6,30})')]],
    passwordConfirmation: [undefined, [Validators.required, this.passwordMismatch]],
    signOutOfAll: [false]
  });

  code: string;
  userId: string;
  token: string;
  isValid: boolean;
  loading: boolean;

  constructor(private readonly auth: AuthenticationService,
              private readonly profile: ProfileService,
              private readonly route: ActivatedRoute,
              private readonly formBuilder:FormBuilder,
              private readonly cdRef:ChangeDetectorRef) { 
                
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
        this.auth.createFingerprint().subscribe(value => {
          this.auth.verifyPasswordResetCode(this.userId, this.code, value).subscribe(res => {
            this.isValid = true;
            this.loading = false;
            this.auth.setTokens(res);
            this.cdRef.detectChanges();
          }, error  => {
            this.isValid = false;
            this.loading = false;
            this.cdRef.detectChanges();
          });
        });
      }
  }

  ngAfterViewInit() {
  }

  isEmptyOrSpaces(str: string){
    return str === null || str.match(/^ *$/) !== null;
  }

  passwordMismatch(control: FormGroup): ValidationErrors | null {
    const password = control.get('password');
    const passwordConfirmation = control.get('passwordConfirmation');
    return password == passwordConfirmation ? {'passwordMismatch': true} : null; 
  } 


}