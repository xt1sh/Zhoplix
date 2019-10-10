import { Injectable } from '@angular/core';
import decode from 'jwt-decode';
import { AuthenticationService } from '../../authentication/authentication.service';
import { Router, CanActivate, ActivatedRouteSnapshot } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class RoleGuardService implements CanActivate {

  constructor(private readonly auth: AuthenticationService,
              private readonly router: Router) { }

  canActivate(route: ActivatedRouteSnapshot): boolean {
    const expectedRole = route.data.expectedRole;
    const tokenPayload = decode(localStorage.getItem('access_token'));
    if (
      !this.auth.isLoggedIn ||
      tokenPayload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] !== expectedRole
    ) {
      this.router.navigate(['login']);
      return false;
    }
    return true;
  }
}
