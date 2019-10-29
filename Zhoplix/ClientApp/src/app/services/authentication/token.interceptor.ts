import { AuthenticationService } from './authentication.service';
import {
    HttpEvent,
    HttpHandler,
    HttpInterceptor,
    HttpRequest
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {

    constructor(public auth: AuthenticationService) { }
    private isRefreshing = false;
    private refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(null);
    
    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        request = this.addToken(request, this.auth.getAccessToken());
        return next.handle(request);
    }
    addToken(request: HttpRequest<any>, token: string) {
        return request.clone({
                setHeaders: {
                //'Content-Type': 'application/json',
                Authorization: `Bearer ${this.auth.getAccessToken()}`
                }
        });
    }

    handle401Error(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        if (!this.isRefreshing) {
            this.isRefreshing = true;
            this.refreshTokenSubject.next(null);
            const observ = this.auth.createFingerprint()
            .subscribe(value => {
                this.auth.refreshTokens(this.auth.getRefreshToken(), value)
                .subscribe(res => {
                    this.isRefreshing = false;
                    this.refreshTokenSubject.next(res.body.accessToken);
                    this.auth.setToken(res);
                    return next.handle(this.addToken(request, res.body.accessToken));
                })
            });
          } 


    }
    
}
