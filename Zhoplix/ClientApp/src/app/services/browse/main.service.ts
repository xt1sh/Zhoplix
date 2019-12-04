import { Injectable, Inject } from '@angular/core';
import { HttpResponse, HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IdName } from 'src/app/models/admin/id-name';
import { Titles } from 'src/app/models/titles';

@Injectable({
  providedIn: 'root'
})
export class MainService {

  constructor(private http: HttpClient,
              @Inject('BASE_URL') private readonly originUrl: string) { }

  getMyListPage(pageNumber: number, pageSize: number):  Observable<HttpResponse<Array<Titles>>> {
    return this.http.get<Array<Titles>>(`${this.originUrl}Browse/GetMyListPage/${pageNumber}/${pageSize}`, { observe: 'response' });
  }
}
