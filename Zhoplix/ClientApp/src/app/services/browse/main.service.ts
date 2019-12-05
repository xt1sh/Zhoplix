import { Injectable, Inject } from '@angular/core';
import { HttpResponse, HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IdName } from 'src/app/models/admin/id-name';
import { Title } from 'src/app/models/title';

@Injectable({
  providedIn: 'root'
})
export class MainService {

  constructor(private http: HttpClient,
              @Inject('BASE_URL') private readonly originUrl: string) { }

  getMyListPage(pageNumber: number, pageSize: number):  Observable<HttpResponse<Array<Title>>> {
    return this.http.get<Array<Title>>(`${this.originUrl}Browse/GetMyListPage/${pageNumber}/${pageSize}`, { observe: 'response' });
  }

  getMyListSize(): Observable<HttpResponse<any>> {
    return this.http.get<Array<Title>>(`${this.originUrl}Browse/GetMyListSize`, { observe: 'response' });
  }
}
