import { AuthenticationService } from 'src/app/services/authentication/authentication.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  tvSrc = 'Images/Home/tv.png';
  mobileSrc = 'Images/Home/mobile.png';
  mobile2Src = 'Images/Home/mobile2.png';
  downloadSrc = 'Images/Home/download-icon.gif';
  devicesSrc = 'Images/Home/devices.png';
  tvVideo = 'Videos/Home/tvVideo.mp4';
  devicesVideo = 'Videos/Home/devices.mp4'

  constructor(private readonly router: Router) {}

  ngOnInit() {
    let video = document.getElementById('video') as HTMLVideoElement;
    video.muted = true;
    video = document.getElementById('video2') as HTMLVideoElement;
    video.muted = true;
  }

  navigateSignUp() {
    this.router.navigate(['signup']);
  }
}
