import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpResponse, HttpHeaders } from '@angular/common/http';
import { Login } from 'src/app/models/Login';
import { Observable } from 'rxjs';
import { CookieService } from 'ngx-cookie-service';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

constructor(private http: HttpClient,
            private cookieService: CookieService,
            @Inject('BASE_URL') private readonly originUrl: string) { }

  login(userCredentials: Login) : Observable<HttpResponse<any>>  {

    return this.http.post<Login>(`${this.originUrl}Authentication/Login`, userCredentials, {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      }),
        observe: 'response'});
     

  }

  setToken(authResult): void {
    var expirationTime = new Date();
    expirationTime.setSeconds(authResult.body.expirationTime);
    localStorage.setItem("expires_in", expirationTime.toString());
    localStorage.setItem("access_token", authResult.body.accessToken );
    this.cookieService.set("refresh_token", authResult.body.refreshToken, 30);
  }

  getToken(): string {
    return localStorage.getItem('accessToken');
  }

}
