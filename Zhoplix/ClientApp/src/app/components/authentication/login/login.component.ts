import { Component, OnInit } from '@angular/core';
import { Validators, FormBuilder } from '@angular/forms';
import { AuthenticationService } from 'src/app/services/authentication/authentication.service';
import { Router, ActivatedRoute } from '@angular/router';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginForm = this.formBuilder.group({
    login: [undefined, Validators.required],
    password: [undefined, Validators.required],
    rememberMe: [true]
  });

  returnUrl: string;
  loginSpinner: boolean;

  constructor(private formBuilder: FormBuilder,
     private auth: AuthenticationService,
     private router: Router,
     private route: ActivatedRoute) { }

  ngOnInit() {
    if(this.auth.isLoggedIn) {
      this.router.navigate(['']);
    }
    this.loginSpinner = false;
    const query = this.route.snapshot.queryParams['returnUrl'];
    this.returnUrl = query ? query.slice(1) : '';
  }

  onSubmit() {
    this.loginSpinner = true;
    let password = this.loginForm.controls['password'];

    this.auth.login(this.loginForm.value)
        .subscribe(res => {

          this.auth.setToken(res, this.loginForm.controls['rememberMe'].value);
          this.router.navigate([this.returnUrl]);
          this.loginSpinner = false;
        }, error => {
          this.loginSpinner = false;
          password.markAsUntouched();
          password.setValue('');
          password.setErrors({'incorrect': true});
        });
  }
}
