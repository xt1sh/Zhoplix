import { Component, OnChanges } from '@angular/core';
import { Router, NavigationStart, NavigationEnd } from '@angular/router';
import { BACKGROUNDS } from './services/background/backgrounds';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {

  title = 'app';
  imgSrc = '';
  url = '';
  constructor(private readonly router: Router) {
    router.events.subscribe(event => {
      if(event instanceof NavigationEnd) {
        this.checkBackground();
      }
    });
  }

  checkBackground(): void {
    BACKGROUNDS.forEach(element => {
      if(this.router.url.includes(element.route)) {
        this.imgSrc = element.imageSrc;
        this.url = this.router.url.slice(1);
      } else {
        this.imgSrc = '';
        this.url = '';
      }
    });
  }
}
