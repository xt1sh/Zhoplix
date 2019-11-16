import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable, Inject } from '@angular/core';
import { Observable } from 'rxjs';
import { EpisodeForPlayerModel } from 'src/app/models/episode/episode-for-player-model';

@Injectable({
  providedIn: 'root'
})
export class EpisodesService {

  constructor(@Inject('BASE_URL') private readonly originUrl: string,
              private readonly http: HttpClient) { }

  getEpisodeById(id: number): Observable<HttpResponse<EpisodeForPlayerModel>> {
    return this.http.get<EpisodeForPlayerModel>(`${this.originUrl}Episode/GetTitleById/${id}`, {observe: 'response'});
  }
}
