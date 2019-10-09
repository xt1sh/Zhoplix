import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { AuthenticationService } from 'src/app/services/authentication/authentication.service';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginForm;
  backgroundImgSrc = 'background/IMG_20191009_121937_054-02-01.jpeg';

  constructor(private auth:AuthenticationService) { }


  onSubmit() {
    this.auth.login(this.loginForm.value).subscribe(res => {this.auth.setToken(res)});
  }

  ngOnInit() {
    this.loginForm = new FormGroup({
      login: new FormControl(''),
      password: new FormControl(''),
    });
  }
}
