import { Component, OnInit, NgZone } from '@angular/core';
import { AuthenticationService } from 'src/app/services/authentication/authentication.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-sign-out',
})
export class SignOutComponent implements OnInit {

  constructor(private readonly auth: AuthenticationService,
              private readonly router: Router,
              private readonly ngZone: NgZone) { }

  ngOnInit() {
    this.auth.createFingerprint().subscribe(value => {
      this.auth.signOut(value)
      .subscribe();
      this.auth.deleteTokens();
      this.ngZone.run(() => this.router.navigate([''])).then();
    })
  }

}
