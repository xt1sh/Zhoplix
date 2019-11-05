import { Injectable, Inject } from '@angular/core';
import { AuthenticationService } from '../authentication/authentication.service';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Observable } from 'rxjs';
import { HttpResponse, HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {
  
  constructor(private readonly auth: AuthenticationService,
              private readonly jwt: JwtHelperService,
              private readonly http: HttpClient,
              @Inject('BASE_URL') private readonly originUrl: string) { }

  getProfileImage():any {
    if (this.auth.isLoggedIn) {
      return this.jwt.decodeToken(localStorage.getItem('access_token'))['avatar'];
    }
  }

  getProfileSettings(): Observable<HttpResponse<any>> {
    return this.http.get<any>(`${this.originUrl}YourAccount`, {observe: 'response'});
  }

}
