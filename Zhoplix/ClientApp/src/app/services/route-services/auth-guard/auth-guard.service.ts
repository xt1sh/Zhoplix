import { Injectable } from '@angular/core';
import { CanActivate, Router, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { AuthenticationService } from '../../authentication/authentication.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuardService implements CanActivate {

  constructor(private readonly auth: AuthenticationService,
              private readonly router: Router) { }

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    if(!this.auth.isLoggedIn) {
      this.auth.redirectUrl = state.url.slice(1);
      this.router.navigate(['login']);
      return false;
    }
    return true;
  }
}
