import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpResponse, HttpHeaders } from '@angular/common/http';
import { Login } from 'src/app/models/Login';
import { Observable } from 'rxjs';
import { CookieService } from 'ngx-cookie-service';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  public redirectUrl: string;

constructor(private readonly http: HttpClient,
            private readonly cookieService: CookieService,
            @Inject('BASE_URL') private readonly originUrl: string) { }

  login(userCredentials: Login): Observable<HttpResponse<any>>  {

    return this.http.post<Login>(`${this.originUrl}Authentication/Login`, userCredentials,
                                { observe: 'response' });
  }

  setToken(authResult, setRefresh = true): void {
    const expirationTime = new Date();
    expirationTime.setSeconds(authResult.body.expirationTime);
    localStorage.setItem('expires_in', expirationTime.toString());
    localStorage.setItem('access_token', authResult.body.accessToken);
    if (setRefresh) {
      this.cookieService.set('refresh_token', authResult.body.refreshToken, 30);
    }
  }

  getToken(): string {
    return localStorage.getItem('access_token');
  }

  get isLoggedIn(): boolean {
    const jwtHelper = new JwtHelperService();
    return !jwtHelper.isTokenExpired(localStorage.getItem('access_token'));
  }
}
