import { Component, OnInit, ViewChild } from '@angular/core';
import { Validators, FormBuilder } from '@angular/forms';
import { AuthenticationService } from 'src/app/services/authentication/authentication.service';
import { CarouselComponent } from 'angular-bootstrap-md';

@Component({
  selector: 'app-login-help',
  templateUrl: './login-help.component.html',
  styleUrls: ['./login-help.component.scss']
})
export class LoginHelpComponent implements OnInit {
  
  @ViewChild(CarouselComponent, {static: true}) item: CarouselComponent;

  backgroundSrc = "background/loginHelpBackground.jpg";
  sender = "Email";
  sendTypes: string[] = ["Email", "Text message (SMS)"];
  sendForm = this.formBuilder.group({
    email: [undefined, [Validators.email]]
  });
  error = false;
  constructor(private readonly formBuilder: FormBuilder,
              private readonly auth: AuthenticationService) { }

  ngOnInit() {
  }

  onSubmit() {
    this.auth.restorePassword(this.sendForm.controls["email"].value).subscribe(
      res => {
        this.item.nextSlide();
      },
      error => {
        this.error = true;
      });
  }

}
