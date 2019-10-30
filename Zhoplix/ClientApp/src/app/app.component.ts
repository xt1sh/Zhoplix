import { Component, OnChanges, OnInit } from '@angular/core';
import { Router, NavigationStart, NavigationEnd } from '@angular/router';
import { BACKGROUNDS } from './services/background/backgrounds';
import { AuthenticationService } from './services/authentication/authentication.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {

  title = 'app';
  imgSrc = '';
  url = '';
  constructor(private readonly router: Router,
              private readonly auth: AuthenticationService) {}

  ngOnInit() {
    this.router.events.subscribe(event => {
      if(event instanceof NavigationEnd) {
        this.checkBackground();
      }
    });
    this.auth.createFingerprint().subscribe(value => {
      this.auth.fingerPrint = value;
    })
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
