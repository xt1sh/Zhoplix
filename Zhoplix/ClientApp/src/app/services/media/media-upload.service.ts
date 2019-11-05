import { Injectable, Inject, OnInit } from '@angular/core';
import { HttpRequest, HttpClient, HttpEventType, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MediaUploadService {

  progress: BehaviorSubject<number>;
  message: BehaviorSubject<string>;
  body: BehaviorSubject<any>;

  constructor(@Inject('BASE_URL') private readonly originUrl: string,
              private readonly http: HttpClient) {
    this.progress = new BehaviorSubject<number>(-1);
    this.message = new BehaviorSubject<string>('');
    this.body = new BehaviorSubject<any>(null);
    }

  getProgress(): Observable<number> {
    return this.progress.asObservable();
  }

  getMessage(): Observable<string> {
    return this.message.asObservable();
  }

  getBody(): Observable<any> {
    return this.body.asObservable();
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
        this.body.next(event.body);
      }
    });
  }

  uploadPhoto(photo) {
    if(!photo)
      return;

    this.progress.next(0);
    let reader = new FileReader();
    reader.readAsDataURL(photo);
    reader.onload = () => {
      const uploadReq = new HttpRequest('POST', `${this.originUrl}Admin/UploadPhoto`, {photo: reader.result.toString().split(',')[1]}, {
        reportProgress: true,
      });
      this.http.request(uploadReq).subscribe(event => {
        if (event.type === HttpEventType.UploadProgress)
          this.progress.next(Math.round(100 * event.loaded / event.total));
        else if (event.type === HttpEventType.Response) {
          this.progress.next(-1);
          this.message.next(event.body['photoId'].toString());
        }
      });
    }
  }
}
