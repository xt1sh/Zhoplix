import { Injectable, Inject } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpResponse, HttpHeaders, HttpClient, HttpEventType } from '@angular/common/http';
import { TitlePageView } from 'src/app/components/admin/create/season/season.component';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  constructor(private http: HttpClient,
              @Inject('BASE_URL') private readonly originUrl: string)
              { }

  createTitle(title: any): Observable<HttpResponse<any>>  {
    return this.http.post<any>(`${this.originUrl}Admin/CreateTitle`, title, { observe: 'response' });
  }

  createSeason(season: any): Observable<HttpResponse<any>> {
    return this.http.post<any>(`${this.originUrl}Admin/CreateSeason`, season, { observe: 'response' });
  }

  createEpisode(episode: any): Observable<HttpResponse<any>> {
    return this.http.post<any>(`${this.originUrl}Admin/CreateEpisode`, episode, { observe: 'response' });
  }

  getTitleViewByName(name: string): Observable<HttpResponse<string>> {
    return this.http.get<string>(`${this.originUrl}Admin/GetTitle/${name}`, { observe: 'response'});
  }

  getTitlesPage(pageNumber: number, pageSize: number): Observable<HttpResponse<Array<TitlePageView>>> {
    return this.http.get<Array<TitlePageView>>(`${this.originUrl}Admin/GetTitlesPage/${pageNumber}/${pageSize}`, { observe: 'response' });
  }

  findTitles(name: string): Observable<HttpResponse<Array<TitlePageView>>> {
    return this.http.get<Array<TitlePageView>>(`${this.originUrl}Admin/FindTitles/${name}`, { observe: 'response' });
  }
}
