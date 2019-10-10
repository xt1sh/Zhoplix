import { Component, OnChanges } from '@angular/core';
import { Router } from '@angular/router';
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
    router.events.subscribe(() => {
      this.checkBackground();
    });
  }

  checkBackground(): void {
    BACKGROUNDS.forEach(element => {
      if(element.routes.includes(this.router.url.slice(1))) {
        this.imgSrc = element.imageSrc;
        this.url = this.router.url.slice(1);
      } else {
        this.imgSrc = '';
        this.url = '';
      }
    });
  }
}
