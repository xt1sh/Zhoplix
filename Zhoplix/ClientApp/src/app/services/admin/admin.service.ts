import { CreateTitle } from 'src/app/models/createTitle';
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

  createTitle(title: CreateTitle) : Observable<HttpResponse<any>>  {
    return this.http.post<CreateTitle>(`${this.originUrl}Admin/CreateNewTitle`, title, {
      headers: new HttpHeaders ({
        'Content-Type': 'application/json'
      }),
      observe: 'response' });
  }
}
