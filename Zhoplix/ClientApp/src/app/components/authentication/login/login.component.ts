import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
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

  backgroundImgSrc = 'background/background.jpg';

  constructor(private formBuilder: FormBuilder,
     private auth: AuthenticationService,
     private router: Router,
     private readonly activatedRoute: ActivatedRoute
     ) { }

  ngOnInit() {

  }

  onSubmit() {
    this.auth.login(this.loginForm.value)
        .subscribe(res => {
          this.auth.setToken(res, this.loginForm.controls['rememberMe'].value);
        });
    this.activatedRoute.queryParams.subscribe(params => {
      const returnUrl = params['returnUrl'];
      if(returnUrl) {
        this.router.navigate([returnUrl]);
      }
    })
  }

}
