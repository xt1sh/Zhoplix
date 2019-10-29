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

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
      request = request.clone({
          setHeaders: {
              //'Content-Type': 'application/json',
              Authorization: `Bearer ${this.auth.getAccessToken()}`
          }
      });

      return next.handle(request);
  }
}
