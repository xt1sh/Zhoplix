import { Component, OnInit, AfterViewInit, ElementRef, Renderer2, ViewChild } from '@angular/core';
import { SlideComponent, CarouselComponent } from 'angular-bootstrap-md';
import { FormBuilder, Validators } from '@angular/forms';
import { AuthenticationService } from 'src/app/services/authentication/authentication.service';
import { Router, NavigationEnd, ActivatedRoute, RoutesRecognized } from '@angular/router';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements AfterViewInit, OnInit {

  currentSlide: number;

  @ViewChild(CarouselComponent, {static: true}) item: CarouselComponent;
  firstLogo = 'tempLogos/firstLogo.png';
  registrationForm = this.formBuilder.group({
    username: [undefined, Validators.required],
    email: [undefined, Validators.required],
    password: [undefined, Validators.required],
  });

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
  }

  onClickOne(next: number) {
    this.router.navigate([`signup/${next}`]);
    this.currentSlide = next;
  }

  onSubmit() {
    this.auth.signUp(this.registrationForm.value)
     .subscribe();
    this.onClickOne(4);
  }

  ngAfterViewInit() {
  }
}
