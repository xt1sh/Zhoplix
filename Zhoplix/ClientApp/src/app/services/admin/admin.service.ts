import { Injectable, Inject } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpResponse, HttpHeaders, HttpClient, HttpEventType } from '@angular/common/http';

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

  public upload(data, videoId) {
    let uploadURL = `${this.originUrl}Admin/UploadVideo`;

    return this.http.post<any>(uploadURL, {data, videoId}, {
      reportProgress: true,
      observe: 'events'
    }).subscribe((event) => {

      switch (event.type) {

        case HttpEventType.UploadProgress:
          const progress = Math.round(100 * event.loaded / event.total);
          return { status: 'progress', message: progress };

        case HttpEventType.Response:
          return event.body;
        default:
          return `Unhandled event: ${event.type}`;
      }
    });
  }
}
