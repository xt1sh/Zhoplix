import { Component, OnInit, AfterViewInit, ElementRef, Renderer2, ViewChild } from '@angular/core';
import { SlideComponent, CarouselComponent } from 'angular-bootstrap-md';
import { FormBuilder, Validators } from '@angular/forms';
import { AuthenticationService } from 'src/app/services/authentication/authentication.service';
import { Router, NavigationEnd, ActivatedRoute, RoutesRecognized } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {

  currentSlide: number;

  @ViewChild(CarouselComponent, {static: true}) item: CarouselComponent;
  firstLogo = 'tempLogos/firstLogo.png';
  registrationForm = this.formBuilder.group({
    username: [undefined, Validators.required],
    email: [undefined, [Validators.required, Validators.email]],
    password: [undefined, [Validators.required,
      Validators.pattern('((?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{6,30})')]],
  });

  signUpSpinner: boolean;
  incorrect: boolean;
  error: string;

  constructor(private rd: Renderer2,
              private formBuilder: FormBuilder,
              private auth: AuthenticationService,
              private readonly router: Router,
              private readonly route: ActivatedRoute) { }

  ngOnInit() {
    this.currentSlide = 0;
    this.router.navigate(['signup/1']);
    this.router.events.subscribe(val => {
      if(val instanceof RoutesRecognized)
        if(+val.state.root.firstChild.params["id"] === this.currentSlide)
          this.item.nextSlide();
    });
    if(this.auth.isLoggedIn) {
      this.router.navigate(['']);
    }
    this.incorrect = false;
    this.signUpSpinner = false;
    this.error = null;
  }

  onClickOne(next: number) {
    this.router.navigate([`signup/${next}`]);
    this.currentSlide = next;
  }

  onSubmit() {
    this.signUpSpinner = true;
    let password = this.registrationForm.controls['password'];

    if (this.registrationForm.valid) {
    this.auth.signUp(this.registrationForm.value)
     .subscribe(res => {
       this.incorrect = false;
       this.onClickOne(4);
       this.signUpSpinner = false;
     }, error => {
       this.error = error.error.errors[0].description;
       this.incorrect = true;
       this.signUpSpinner = false;
       password.reset();
     });
    }
    else {
      this.signUpSpinner = false;
    }
  }
}
