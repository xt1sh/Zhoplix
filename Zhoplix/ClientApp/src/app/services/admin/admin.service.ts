import { Injectable, Inject } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpResponse, HttpHeaders, HttpClient, HttpEventType } from '@angular/common/http';
import { IdName } from 'src/app/models/admin/id-name';

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

  createMovie(movie: any): Observable<HttpResponse<any>> {
    return this.http.post<any>(`${this.originUrl}Admin/CreateMovie`, movie, { observe: 'response' });
  }

  getTitleViewByName(name: string): Observable<HttpResponse<string>> {
    return this.http.get<string>(`${this.originUrl}Admin/GetTitle/${name}`, { observe: 'response'});
  }

  getTitlesPage(pageNumber: number, pageSize: number): Observable<HttpResponse<Array<IdName>>> {
    return this.http.get<Array<IdName>>(`${this.originUrl}Admin/GetTitlesPage/${pageNumber}/${pageSize}`, { observe: 'response' });
  }

  findTitles(name: string): Observable<HttpResponse<Array<IdName>>> {
    return this.http.get<Array<IdName>>(`${this.originUrl}Admin/FindTitles/${name}`, { observe: 'response' });
  }

  getAllSeasonOfTitle(titleId: number): Observable<HttpResponse<Array<IdName>>> {
    return this.http.get<Array<IdName>>(`${this.originUrl}Admin/GetAllSeasonsOfTitle/${titleId}`, { observe: 'response' });
  }
}
