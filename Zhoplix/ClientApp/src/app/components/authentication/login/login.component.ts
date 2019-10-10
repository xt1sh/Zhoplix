import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { AuthenticationService } from 'src/app/services/authentication/authentication.service';
import { Router } from '@angular/router';


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

  backgroundImgSrc = 'background/background.jpg';

  constructor(private formBuilder: FormBuilder,
     private auth: AuthenticationService,
     private router: Router) { }

  ngOnInit() {

  }

  onSubmit() {
    this.auth.login(this.loginForm.value)
        .subscribe(res => {
          this.auth.setToken(res, this.loginForm.controls['rememberMe'].value);
        });
    if (this.auth.redirectUrl) {
      this.router.navigateByUrl(this.auth.redirectUrl);
      this.auth.redirectUrl = null;
    }
  }

}
