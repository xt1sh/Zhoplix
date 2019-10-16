import { Component, OnInit, AfterViewInit, ElementRef, Renderer2, ViewChild } from '@angular/core';
import { SlideComponent, CarouselComponent } from 'angular-bootstrap-md';
import { FormBuilder, Validators } from '@angular/forms';
import { AuthenticationService } from 'src/app/services/authentication/authentication.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements AfterViewInit {

  @ViewChild(CarouselComponent, {static: true}) item: CarouselComponent;
  firstLogo = 'tempLogos/firstLogo.png';
  registrationForm = this.formBuilder.group({
    username: [undefined, Validators.required],
    email: [undefined, Validators.required],
    password: [undefined, Validators.required],
  });

  constructor(private rd: Renderer2, private formBuilder: FormBuilder, private auth: AuthenticationService) { }

  onClickOne() {
      this.item.nextSlide();
  }

  onSubmit() {
    this.auth.signUp(this.registrationForm.value)
    .subscribe(res => {
      this.auth.setToken(res);
      
    })
  }

  ngAfterViewInit() {
  }
}
