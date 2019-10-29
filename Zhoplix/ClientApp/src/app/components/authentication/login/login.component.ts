import { Component, OnInit, NgZone } from '@angular/core';
import { Validators, FormBuilder } from '@angular/forms';
import { AuthenticationService } from 'src/app/services/authentication/authentication.service';
import { Router, ActivatedRoute } from '@angular/router';
import { Login } from 'src/app/models/login';


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
  incorrect: boolean;

  constructor(private formBuilder: FormBuilder,
     private auth: AuthenticationService,
     private router: Router,
     private route: ActivatedRoute,
     private readonly ngZone: NgZone) { }

  ngOnInit() {
    if(this.auth.isLoggedIn) {
      this.router.navigate(['']);
    }
    this.incorrect = false;
    this.loginSpinner = false;
    const query = this.route.snapshot.queryParams['returnUrl'];
    this.returnUrl = query ? query.slice(1) : '';
  }

  onSubmit() {
    this.loginSpinner = true;
    let password = this.loginForm.controls['password'];
    if (this.loginForm.valid) {
      let login: Login = this.loginForm.value;
      const loginObservable = this.auth.createFingerprint().subscribe(value => {
        login.fingerPrint = value;
        this.auth.login(login)
          .subscribe(res => {
            this.incorrect = false;
            this.auth.setToken(res, this.loginForm.controls['rememberMe'].value);
            this.ngZone.run(() => this.router.navigate([this.returnUrl])).then();
            this.loginSpinner = false;
            loginObservable.unsubscribe();
          }, error => {
            this.incorrect = true;
            this.loginSpinner = false;
            password.reset();
            loginObservable.unsubscribe();
          });
      })
    }
    else {
      this.loginSpinner = false;
    }

  }
}
