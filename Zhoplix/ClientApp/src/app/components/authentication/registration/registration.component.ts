import { Component, OnInit, AfterViewInit, ElementRef, Renderer2, ViewChild } from '@angular/core';
import {SlideComponent, CarouselComponent} from 'angular-bootstrap-md';
@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements AfterViewInit {

  @ViewChild(CarouselComponent, null) item: ElementRef;
  firstLogo = 'tempLogos/firstLogo.png';
  constructor(private rd: Renderer2) { }

    onClickOne() {
      this.item.nextSlide();
  }

  ngAfterViewInit() {
  }
}
