import { Injectable, Inject } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpResponse, HttpHeaders, HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  constructor(private http: HttpClient,
              @Inject('BASE_URL') private readonly originUrl: string)
              { }

  createTitle(title: any) : Observable<HttpResponse<any>>  {
    return this.http.post<any>(`${this.originUrl}Admin/CreateNewTitle`, title, { observe: 'response' });
  }
}
