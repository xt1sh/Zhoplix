import { Component, OnChanges, OnInit, NgZone } from '@angular/core';
import { Router, NavigationStart, NavigationEnd } from '@angular/router';
import { AuthenticationService } from './services/authentication/authentication.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {

  title = 'app';
  constructor(private readonly router: Router,
              private readonly auth: AuthenticationService,
              private readonly ngZone: NgZone) {}

  async ngOnInit() {
    await this.auth.createFingerprint();
    if (this.auth.isLoggedIn) {
      this.auth.verifySession(this.auth.fingerPrint).subscribe(res => {
      }, () => {
        this.auth.deleteTokens();
        this.ngZone.run(() => this.router.navigate(['']));
      });
    }
  }
}
