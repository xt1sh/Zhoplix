import { Injectable } from '@angular/core';
import { AuthenticationService } from '../authentication/authentication.service';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {

  constructor(private readonly auth: AuthenticationService,
              private readonly jwt: JwtHelperService) { }

  getProfileImage():any {
    if (this.auth.isLoggedIn) {
      return this.jwt.decodeToken(localStorage.getItem('access_token'))['avatar'];
    }
  }

}
