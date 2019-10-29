import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpResponse, HttpHeaders } from '@angular/common/http';
import { Login } from 'src/app/models/login';
import { Observable, Subscriber } from 'rxjs';
import { CookieService } from 'ngx-cookie-service';
import { JwtHelperService } from '@auth0/angular-jwt';
import { CurrentUser } from 'src/app/models/current-user';
import decode from 'jwt-decode';
import { Registration } from 'src/app/models/registration';
import fingerprint from 'fingerprintjs2';
import { subscribeOn } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  public redirectUrl: string = '';
   constructor(private readonly http: HttpClient,
              private readonly cookieService: CookieService,
              @Inject('BASE_URL') private readonly originUrl: string) { }

  login(userCredentials: Login): Observable<HttpResponse<any>>  {
    return this.http.post<Login>(`${this.originUrl}Authentication/Login`, userCredentials,
                                { observe: 'response' });
  }

  signUp(userCredentials: Registration): Observable<HttpResponse<any>> {
    return this.http.post<Registration>(`${this.originUrl}Authentication/Registration`, userCredentials,
                                { observe: 'response' });
  }

  confirmEmail(userId:string, token:string, fingerPrint:string): Observable<HttpResponse<any>> {
    return this.http.post(`${this.originUrl}Authentication/ConfirmEmail`, {userId:userId, token:token, fingerPrint:fingerPrint},
                                { observe: 'response'});
  }

  refreshTokens(refreshToken:string, fingerPrint:string): Observable<HttpResponse<any>> {
    return this.http.post(`${this.originUrl}Authentication/RefreshTokens`, {refreshToken, fingerPrint},
                                { observe: 'response'});
  }

  getCurrentUser(): CurrentUser {
    let user: CurrentUser;
    const tokenPayload = decode(localStorage.getItem('access_token'));
    user.role = tokenPayload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
    return user;
  }

  setToken(authResult, setRefresh = true): void {
    const expirationTime = new Date();
    expirationTime.setSeconds(authResult.body.expirationTime);
    localStorage.setItem('expires_in', expirationTime.toString());
    localStorage.setItem('access_token', authResult.body.accessToken);
    if (setRefresh) {
      this.cookieService.set('refresh_token', authResult.body.refreshToken, 30,  "/");
    }
  }

  getAccessToken(): string {
    return localStorage.getItem('access_token');
  }
  
  getRefreshToken(): string {
    return this.cookieService.get('refresh_token');
  }

  get isLoggedIn(): boolean {
    const token = localStorage.getItem('access_token');
    if(!token) {
      return false;
    }
    const jwtHelper = new JwtHelperService();
    return !jwtHelper.isTokenExpired(token);
  }

  createFingerprint(): Observable<string> {
    return new Observable(observer => {
      fingerprint.get((result) => {
        const print = fingerprint.x64hash128(result.join(''));
        if(!print) {
          observer.error();
        } else {
          observer.next(print);
        }
        observer.complete();
      });
    });
  }
}
