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
import { subscribeOn, tap } from 'rxjs/operators';
import { Tokens } from 'src/app/models/tokens';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  public fingerPrint: string = '';
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
    return this.http.post<any>(`${this.originUrl}Authentication/ConfirmEmail`, {userId:userId, token:token, fingerPrint:fingerPrint},
                                { observe: 'response'});
  }

  refreshTokens(refreshToken:string, fingerPrint:string) {
    return this.http.post<any>(`${this.originUrl}Authentication/RefreshTokens`, {refreshToken, fingerPrint})
                        .pipe(tap((tokens: Tokens) => {
                          this.setTokens(tokens);
                        }));
  }

  signOut(fingerprint:string) {
    this.http.post<any>(`${this.originUrl}Authentication/SignOut`, {fingerprint:fingerprint},
                                ).subscribe();
  }

  signOutOfAll() {
    this.http.post<any>(`${this.originUrl}Authentication/SignOutOfAll`, {}).subscribe();
  }

  restorePassword(identifier: string) {
    return this.http.post<any>(`${this.originUrl}Authentication/ResetPassword`, {identifier:identifier});
  }

  verifyPasswordResetCode(userId: string, code: string, fingerprint: string) {
    return this.http.post<any>(`${this.originUrl}Authentication/VerifyPaswordResetCode`, {userId:userId, code:code, fingerprint:fingerprint});
  }

  verifySession(fingerprint: string) {
    return this.http.post<any>(`${this.originUrl}Authentication/VerifySession`, {fingerprint:fingerprint});
  }

  getCurrentUser(): CurrentUser {
    let user: CurrentUser;
    const tokenPayload = decode(localStorage.getItem('access_token'));
    user.role = tokenPayload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
    return user;
  }

  setTokens(tokens: Tokens): void {
    const expirationTime = new Date();
    expirationTime.setSeconds(tokens.expirationTime);
    localStorage.setItem('expires_in', expirationTime.toString());
    localStorage.setItem('access_token', tokens.accessToken);
    if (tokens.refreshToken) {
      this.cookieService.set('refresh_token', tokens.refreshToken, 30,  "/");
    }
  }

  getAccessToken(): string {
    return localStorage.getItem('access_token');
  }

  getRefreshToken(): string {
    return this.cookieService.get('refresh_token');
  }

  deleteAccessToken(): void {
    localStorage.removeItem('access_token');
    localStorage.removeItem('expires_in');
  }

  deleteRefreshToken(): void {
    this.cookieService.delete('refresh_token', '/');
  }

  deleteTokens(): void {
    this.deleteAccessToken();
    this.deleteRefreshToken();
  }

  get isLoggedIn(): boolean {
    return !!this.getAccessToken();
  }

  async createFingerprint() {
    const result = await fingerprint.getPromise(null);
    this.fingerPrint = await fingerprint.x64hash128(result.map(function (pair) { return pair.value }).join(), 31);
    console.log(this.fingerPrint);
  }
}
