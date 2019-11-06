import { Component, OnChanges, OnInit } from '@angular/core';
import { Router, NavigationStart, NavigationEnd } from '@angular/router';
import { AuthenticationService } from './services/authentication/authentication.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {

  title = 'app';
  constructor(private readonly router: Router,
              private readonly auth: AuthenticationService) {
                this.auth.createFingerprint().subscribe(value => this.auth.fingerPrint = value);
              }

  ngOnInit() {
  }

}
