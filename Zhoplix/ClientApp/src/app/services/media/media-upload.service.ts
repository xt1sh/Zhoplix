import { Injectable, Inject } from '@angular/core';
import { HttpRequest, HttpClient, HttpEventType } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MediaUploadService {

  progress: BehaviorSubject<number>;
  message: BehaviorSubject<string>;

  constructor(@Inject('BASE_URL') private readonly originUrl: string,
              private readonly http: HttpClient) {
                this.progress = new BehaviorSubject<number>(-1);
                this.message = new BehaviorSubject<string>('');
              }

  getProgress(): Observable<number> {
    return this.progress.asObservable();
  }

  getMessage(): Observable<string> {
    return this.message.asObservable();
  }

  uploadVideo(video) {
    if (!video)
      return;

    const formData = new FormData();

    formData.append(video.name, video);

    const uploadReq = new HttpRequest('POST', `${this.originUrl}Admin/UploadVideo`, formData, {
      reportProgress: true,
    });

    this.http.request(uploadReq).subscribe(event => {
      if (event.type === HttpEventType.UploadProgress)
        this.progress.next(Math.round(100 * event.loaded / event.total));
      else if (event.type === HttpEventType.Response) {
        this.message.next(event.body.toString());
      }
    });
  }
}
